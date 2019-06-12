/*
 * Program:         a1.exe
 * Module:          Director.cs
 * Date:            June 10, 2019
 * Author:          Youngmin Chung
* Description:     Using input data file(here is .txt file), I create two kinds of
 *                  files contained html and markdown.
 *                  The txt file is consist of detailed information to create two files and
 *                  it can split using ':' and ';'. And the end of each senctence is '#'
 *                  After splitting, 
 *                  1st step figuring out Document type or Element type
 *                  2nd step if the result of first step is document. 
 *                      there are two option for "Html" and "Markdown"
 *                  3rd step if the result of element, we pass the elementType to
 *                      each class like HtmlImage... in DocumentFactory.cs
 *                  4th step the file created according to the fileName after each Document Factory works properly             
 */

using System;
using System.IO;
using System.Text.RegularExpressions;
using DocumentFactory;

namespace Director
{
    class Program
    {
        
        // This just makes sure we're reading and writing to the correct directory
        // You can ignore it for this project
        static string RelativeToAbsolutePath(string path)
        {
            var projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            return Path.Combine(projectFolder, "..", @path);
        }

        static void Main(string[] args)
        {
            string[] commands;
            string list = "";
            try
            {
                list = File.ReadAllText(RelativeToAbsolutePath("CreateDocumentScript.txt"));
            } catch(FileNotFoundException e)
            {
                Console.WriteLine("Input file not found (be sure CreateDocumentScript.txt is in the right directory: \"" + e.FileName + "\").");
                System.Environment.Exit(1);
            }

            commands = list.Split('#');

            // to be assigned below
            DocumentFactory.IDocument document = null;
            //DocumentFactory.IElement element = null;
            string factoryType = ""; // 'html' or 'markdown'
            string elementType = ""; // 'image','header','list','table'
            string props = ""; // string value of rest component except elementType
            foreach (var command in commands)
            {
                string strippedCommand = Regex.Replace(command, @"\t|\n|\r", ""); // this cleans up the text a bit
                string[] commandList = strippedCommand.Split(':');

                switch (commandList[0])
                {
                    case "Document":
                        string[] DocumentList = commandList[1].Split(';');
                        factoryType = DocumentList[0];
                        string fileName = DocumentList[1];

                        if (factoryType == "Html")
                            document = HtmlFactory.Get().CreateDocument(fileName); 
                        else if (factoryType == "Markdown")
                            document = MdFactory.Get().CreateDocument(fileName);
                        else
                            throw new NotImplementedException();
                        break;

                    case "Element":                   
                        //props = propsList[0];
                        elementType = commandList[1];
                        props = commandList[2];

                        if(factoryType == "Html")
                            document.AddElement(HtmlFactory.Get().CreateElement(elementType, props));
                        else if (factoryType == "Markdown")
                            document.AddElement(MdFactory.Get().CreateElement(elementType, props));

                        else
                            throw new NotImplementedException();

                        break;
                    case "Run":
                        File.WriteAllText(RelativeToAbsolutePath(document.GetFilename()), document.Run());
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
