using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace FileMetadataExport
{
    class Program
    {
        static void Main(string[] args)
        {

            //Use argument -l or --list to display all available property names for a file
            if (args[0] == "-l" || args[0] == "--list")
            {
                if (args.Length == 2)
                {
                    var filePath = args[1].TrimStart('"').TrimEnd('"');
                    if (File.Exists(filePath))
                    {
                        FileInfo file = new FileInfo(filePath);
                        DisplayFileProperties(file.FullName);
                    }
                    else
                    {
                        Console.WriteLine(filePath + " is not a valid file. Please specify a valid file path.");
                    }
                    Console.ReadLine();
                    return;
                }
                else
                {
                    Console.WriteLine("Must provide a single valid file path");
                    Console.ReadLine();
                    return;
                }
            }
            //Use argument -p or --params to provide a directory and a list of parameter names and output a list of all files and the respective parameter values in pipe delimited format.
            else if (args[0] == "-p" || args[0] == "--params")
            {

                if (args.Length >= 3)
                {
                    var path = args[1].TrimStart('"').TrimEnd('"');
                    if (Directory.Exists(path))
                    {
                        var extIndex = Array.FindIndex(args, s => s == "-e");
                        var numParams = args.Length - 2;
                        var extensions = "";

                        if (extIndex != -1)
                        {
                            numParams = extIndex - 2;
                            if (args.Length > extIndex + 1)
                                extensions = args[extIndex + 1];
                        }

                        string[] paramStrings = new string[numParams];
                        Array.Copy(args, 2, paramStrings, 0, numParams);
                        DisplaySelectedProperties(path, paramStrings, extensions);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Please provide a valid path");
                    }
                }
                else
                {
                    Console.WriteLine("Please provide a valid path and list of parameters to display");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Please provide a valid command switch");
                Console.ReadLine();
                return;
            }

        }

        static void DisplayFileProperties(string fileName)
        {
            using (var shell = ShellObject.FromParsingName(fileName))
            {

                var videoProps = shell.Properties.DefaultPropertyCollection;

                foreach (var prop in videoProps)
                {
                    Console.WriteLine(prop.CanonicalName);
                }

            }

        }

        static void DisplaySelectedProperties(string path, string[] paramStrings, string extensions)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            IEnumerable<FileInfo> files = dir.GetFiles().Where(f => extensions.Contains(f.Extension) || extensions == "");

            Console.Write("File Name");

            foreach (var param in paramStrings)
            {
                Console.Write("|" + param);
            }

            Console.Write("\n");

            foreach (var file in files)
            {

                Console.Write(file.Name);

                using (var shell = ShellObject.FromParsingName(file.FullName))
                {

                    foreach (var param in paramStrings)
                    {

                        Console.Write("|");

                        if (shell.Properties.DefaultPropertyCollection.Contains(param))
                            Console.Write(shell.Properties.GetProperty(param).ValueAsObject.ToString());

                    }

                    Console.Write("\n");
                }

            }

        }

    }
}