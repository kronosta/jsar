# jsar
 A human-readable uncompressed archive format for distributing multiple files in one textbox.

 JSAR now runs in .NET 6.0, so you will need to have that installed (previous versions used two standalone .NET Core 3.1 versions, for Linux and Windows; for those check out previous commits to this repo in December 2023, but be aware they have no archive functionality)

 Currently JSAR does not do binary files correctly, so it is suitable only for text files or other text-based formats (this includes JSAR archives if you like nesting). Note that JSAR archives can have commands, so you could try to generate them using those, however that isn't cross-platform.

# Command Line Flags
```
    -E  Enables executable code to be run.
    -i  Followed by a string for some JSAR (JSON) code to use instead of a file.
        [WINDOWS DIRECTIONS]
            You'll have to escape each double quote by using three double quotes instead.
            Weirdly, spaces don't work in strings and I have no idea why.
            Instead, use \u0020.
        [LINUX DIRECTIONS]
            Not sure about spaces in strings (you may or may not have to replace with \u0020),
            but quotes can be escaped inside of a backslash in a double quoted string.
    -I  Takes the JSON from standard input, terminated by a line containing ""@END"".
        ""@END"" must not have any whitespace around it or anything else!
    -r  Supposed to specify that it can extract to existing files, but that seems to
        be the regular behavior anyway.
    -o  Specifies an output directory to write to, relative to the current directory.
    -A  Archives the folder referred to by the first following argument to an archive
        named by the second following argument.
        Note that using -A will cause the program to not do any extraction on the other
        arguments.
        Example: dotnet JSAR.dll -A my-folder my-folder.jsa
    -v  Verbose output. If -A is also used, -v must come first for it to work right.
    -h  Pulls up the message you are reading right now.
--help  Alias for -h.
    -?  Alias for -h.
```
An argument ending in `.jsa` will be interpreted as a file name to read the JSON from. 

# Format
The format is a single `JSARTopLevel` in JSON format, representing the current directory that is active when JSAR is called, or alternatively, the directory specified by `-o`.
## JSARTopLevel
An object with the following keys, representing a directory:

`Files` : A dictionary of filenames (as strings) to `JSARFile`s. These files will be added to the directory corresponding to this `JSARTopLevel`. 

`Directories` : A dictionary of directory names (without a slash) to `JSARTopLevel`s. These directories and their contents will be added to the directory corresponding to this `JSARTopLevel`.

`Commands` : A list of `JSARCommand`s. Commands that will be executed in the directory corresponding to this `JSARTopLevel` if -E is specified.

## JSARFile
An object with the following keys (currently only one), representing a file:

`Contents` : A list of strings. The strings will be concatenated to form the file contents. A good pattern is to have one for each line, so that you can have them on separate lines in the archive, but remember that you still have to manually put in the `\r` and/or `\n` for the line ending.

## JSARCommand
An object with the following keys, representing a command line command:

`Executable` : A string. The name of the executable to run.

`Arguments` : A string. Everything else in the command (any quotes must be specified as part of the string, escaped and everything, in order to include spaces).

## An example
```
{
    "Files": {
        "MyTextFile.txt": {
            "Contents": [
                "This is a text file.\n",
                "We can have two lines.\n",
                "Or even three!\n",
                "Or as many as you need!"
            ]
        },
        "Hello.c": {
            "Contents": [
                "#include <stdio.h>\n",
                "int main(void) {\n",
                "  printf(\"Hello, world!\");\n",
                "}\n"
            ]
        }
    },
    "Directories": {
        "MyDirectory": {
            "Files": {
                "a file in a folder.txt": {
                    "Contents": ["welcome to my directory\n"]
                }
            },
            "Directories": {},
            "Commands": []
        }
    },
    "Commands": [
        {"Executable": "gcc", "Arguments": "-o hello Hello.c"}
    ]
}
```





