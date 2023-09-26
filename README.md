# jsar
 A human-readable uncompressed archive format for distributing multiple files in one textbox.

# Command Line Flags
`     -E  Enables executable code to be run.`
`     -i  Followed by a string for some JSAR (JSON) code to use instead of a file.`
`         [WINDOWS DIRECTIONS]`
`             You'll have to escape each double quote by using three double quotes instead.`
`             Weirdly, spaces don't work in strings and I have no idea why.`
`             Instead, use \u0020.`
`         [LINUX DIRECTIONS]`
`             Not sure about spaces in strings (you may or may not have to replace with \u0020),`
`             but quotes can be escaped inside of a backslash in a double quoted string.`
`    -I  Takes the JSON from standard input, terminated by a line containing "@END".`
`        "@END" must not have any whitespace around it or anything else!`
`    -r  Supposed to specify that it can extract to existing files, but that seems to`
`        be the regular behavior anyway.`
`    -o  Specifies an output directory to write to, relative to the current directory.`
`    -h  Pulls up the message you are reading right now.`
`--help  Alias for -h.`
`    -?  Alias for -h.`

