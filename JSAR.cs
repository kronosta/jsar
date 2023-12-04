using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

public class JSARTopLevel
{
    public Dictionary<string, JSARFile> Files { get; set; }
    public Dictionary<string, JSARTopLevel> Directories { get; set; }
    public List<JSARCommand> Commands { get; set; }
}

public class JSARFile
{
    public List<string> Contents { get; set; }
}

public class JSARCommand
{
    public string Executable { get; set; }
    public string Arguments { get; set; }
}

public class JSAR
{
    public static string jsarText = "";
    public static bool canExecute = false, canRewrite = false;
    public static void ParseArgs(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-E")
            {
                canExecute = true;
            }
            else if (args[i] == "-i")
            {
                i++;
                if (i >= args.Length)
                {
                    Console.WriteLine("JSAR Error: -i should be followed with a string representing a JSAR archive or part of it.");
                    Environment.Exit(1);
                }
                jsarText += args[i];
            }
            else if (args[i] == "-r")
            {
                canRewrite = true;
            }
            else if (args[i] == "-o")
            {
                i++;
                if (i >= args.Length)
                {
                    Console.WriteLine("JSAR Error: -o should be followed with a directory to extract into.");
                    Environment.Exit(1);
                }
                Environment.CurrentDirectory = args[i];
            }
            else if (args[i] == "-I")
            {
                string line = "";
                while (line != "@END")
                {
                    jsarText += line;
                    line = Console.ReadLine();
                }
            }
            else if (args[i] == "-h" || args[i] == "--help" || args[i] == "-?")
            {
                Console.WriteLine(@"
Command Line Flags:
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
    -h  Pulls up the message you are reading right now.
--help  Alias for -h.
    -?  Alias for -h.
    
");
                Environment.Exit(0);
            }
            else if (args[i].ToLower().EndsWith(".jsa"))
            {
                Console.WriteLine("reading file");
                using (TextReader tr = new StreamReader(args[i]))
                {
                    jsarText += tr.ReadToEnd();
                }
            }
        }
    }

    public static void Extract()
    {
        JSARTopLevel topLevel = JsonSerializer.Deserialize<JSARTopLevel>(jsarText);
        ExtractDirectory(topLevel);
    }

    public static void ExtractDirectory(JSARTopLevel jdir)
    {
        foreach (var file in jdir.Files)
        {
            using (FileStream fs = 
                File.Open(
                    file.Key, 
                    canRewrite ? FileMode.OpenOrCreate : FileMode.Create, 
                    FileAccess.Write
                )
            ) 
            {
                foreach (var str in file.Value.Contents)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(str);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }
        foreach (var dir in jdir.Directories)
        {
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, dir.Key))) 
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, dir.Key));
            var originalPath = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Path.Combine(Environment.CurrentDirectory, dir.Key);
            ExtractDirectory(dir.Value);
            Environment.CurrentDirectory = originalPath;
        }
        if (canExecute)
        {
            foreach (var command in jdir.Commands)
            {
                Process.Start(command.Executable, command.Arguments);
            }
        }
        else
        {

            if (jdir.Commands.Count > 0)
            {
                Console.WriteLine("The archive you extracted was denied to run executable commands");
                Console.WriteLine("that may possibly be required for its functionality.");
                Console.WriteLine();
                Console.WriteLine("You can allow it with the -E command line switch, but");
                Console.WriteLine("please please PLEASE look it over and make sure it");
                Console.WriteLine("isn't malicious, because it is fully capable of doing");
                Console.WriteLine("basically anything to your system.");
            }
        }
    }
    public static void Main(string[] args)
    {
        ParseArgs(args);
        Extract();
    }
=======
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

public class JSARTopLevel
{
    public Dictionary<string, JSARFile> Files { get; set; }
    public Dictionary<string, JSARTopLevel> Directories { get; set; }
    public List<JSARCommand> Commands { get; set; }
}

public class JSARFile
{
    public List<string> Contents { get; set; }
}

public class JSARCommand
{
    public string Executable { get; set; }
    public string Arguments { get; set; }
}

public class JSAR
{
    public static string jsarText = "";
    public static bool canExecute = false, canRewrite = false;
    public static void ParseArgs(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-E")
            {
                canExecute = true;
            }
            else if (args[i] == "-i")
            {
                i++;
                if (i >= args.Length)
                {
                    Console.WriteLine("JSAR Error: -i should be followed with a string representing a JSAR archive or part of it.");
                    Environment.Exit(1);
                }
                jsarText += args[i];
            }
            else if (args[i] == "-r")
            {
                canRewrite = true;
            }
            else if (args[i] == "-o")
            {
                i++;
                if (i >= args.Length)
                {
                    Console.WriteLine("JSAR Error: -o should be followed with a directory to extract into.");
                    Environment.Exit(1);
                }
                Environment.CurrentDirectory = args[i];
            }
            else if (args[i] == "-I")
            {
                string line = "";
                while (line != "@END")
                {
                    jsarText += line;
                    line = Console.ReadLine();
                }
            }
            else if (args[i] == "-h" || args[i] == "--help" || args[i] == "-?")
            {
                Console.WriteLine(@"
Command Line Flags:
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
    -h  Pulls up the message you are reading right now.
--help  Alias for -h.
    -?  Alias for -h.
    
");
                Environment.Exit(0);
            }
            else if (args[i].ToLower().EndsWith(".jsa"))
            {
                Console.WriteLine("reading file");
                using (TextReader tr = new StreamReader(args[i]))
                {
                    jsarText += tr.ReadToEnd();
                }
            }
        }
    }

    public static void Extract()
    {
        JSARTopLevel topLevel = JsonSerializer.Deserialize<JSARTopLevel>(jsarText);
        ExtractDirectory(topLevel);
    }

    public static void ExtractDirectory(JSARTopLevel jdir)
    {
        foreach (var file in jdir.Files)
        {
            using (FileStream fs = 
                File.Open(
                    file.Key, 
                    canRewrite ? FileMode.OpenOrCreate : FileMode.Create, 
                    FileAccess.Write
                )
            ) 
            {
                foreach (var str in file.Value.Contents)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(str);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }
        foreach (var dir in jdir.Directories)
        {
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, dir.Key))) 
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, dir.Key));
            var originalPath = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Path.Combine(Environment.CurrentDirectory, dir.Key);
            ExtractDirectory(dir.Value);
            Environment.CurrentDirectory = originalPath;
        }
        if (canExecute)
        {
            foreach (var command in jdir.Commands)
            {
                Process.Start(command.Executable, command.Arguments);
            }
        }
        else
        {

            if (jdir.Commands.Count > 0)
            {
                Console.WriteLine("The archive you extracted was denied to run executable commands");
                Console.WriteLine("that may possibly be required for its functionality.");
                Console.WriteLine();
                Console.WriteLine("You can allow it with the -E command line switch, but");
                Console.WriteLine("please please PLEASE look it over and make sure it");
                Console.WriteLine("isn't malicious, because it is fully capable of doing");
                Console.WriteLine("basically anything to your system.");
            }
        }
    }
    public static void Main(string[] args)
    {
        ParseArgs(args);
        Extract();
    }
}
