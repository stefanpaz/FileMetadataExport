using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMetadataExport
{
    internal class ParsedArgs
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public string[] ParamStrings { get; set; }
        public bool List { get; set; }
        public string[] Extensions { get; set; }

        public static ParsedArgs parseArgs(string[] args)
        {
            
            var parsedArgs = new ParsedArgs();

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-d":
                    case "--dir":

                        if (i + 1 < args.Length && Directory.Exists(args[i + 1].TrimStart('"').TrimEnd('"')))
                        {
                            parsedArgs.Path = args[i + 1].TrimStart('"').TrimEnd('"');
                            i++;
                        }
                        else
                        {
                            Console.WriteLine("Invalid or missing directory argument");
                            return null;
                        }
                        break;
                    case "-p":
                    case "--params":

                        if (i + 1 < args.Length && !string.IsNullOrEmpty(args[i + 1]))
                        {
                            parsedArgs.ParamStrings = args[i + 1].Split(',');
                            i++;
                        }
                        else
                        {
                            Console.WriteLine("Invalid or missing parameters argument");
                            return null;
                        }
                        break;
                    case "-l":
                    case "--list":
                        parsedArgs.List = true;
                        break;
                    case "-e":
                    case "--extensions":
                        if (i + 1 < args.Length && !string.IsNullOrEmpty(args[i + 1]))
                        {
                            parsedArgs.Extensions = args[i + 1].Split(',');
                            i++;
                        }
                        else
                        {
                            Console.WriteLine("Invalid or missing extensions argument");
                            return null;
                        }
                        break;
                    case "-f":
                    case "--file":

                        if (i + 1 < args.Length && File.Exists(args[i + 1].TrimStart('"').TrimEnd('"')))
                        {
                            parsedArgs.FileName = args[i + 1].TrimStart('"').TrimEnd('"');
                            i++;
                        }
                        else
                        {
                            Console.WriteLine("Invalid or missing file argument");
                            return null;
                        }
                        break;
                    default:
                        Console.WriteLine("Unknown argument: " + args[i]);
                        return null;
                }
            }


            if (parsedArgs.List && parsedArgs.ParamStrings != null)
            {
                Console.WriteLine("List option cannot be used at the same time as params options");
                return null;
            }

            if (!parsedArgs.List && parsedArgs.ParamStrings == null)
            {
                Console.WriteLine("Either params option or list option must be used");
                return null;
            }

            if (parsedArgs.List && parsedArgs.FileName == null)
            {
                Console.WriteLine("Missing file argument for list option");
                return null;
            }

            if (parsedArgs.ParamStrings != null && (parsedArgs.Path == null || parsedArgs.Extensions == null))
            {
                Console.WriteLine("Missing directory or extensions argument for params option");
                return null;
            }

            return parsedArgs;
        }
    }
}
