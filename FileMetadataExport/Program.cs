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

            ParsedArgs parsedArgs = ParsedArgs.parseArgs(args);

            if(parsedArgs == null)
            {
                Console.Read();
                return;
            }

            if (parsedArgs.List)
            {
                FileInfo file = new FileInfo(parsedArgs.FileName);
                DisplayFileProperties(file.FullName);
                Console.Read();
                return;
            }
            else
            {
                DisplaySelectedProperties(parsedArgs.Path, parsedArgs.ParamStrings, parsedArgs.Extensions);
                Console.Read();
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

        static void DisplaySelectedProperties(string path, string[] paramStrings, string[] extensions)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            IEnumerable<FileInfo> files = dir.GetFiles("*.*", SearchOption.AllDirectories).Where(f => extensions.Contains(f.Extension));

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