/*
 * Program:         a1.exe
 * Module:          DocumentFactory.cs
 * Date:            June 10, 2019
 * Author:          Youngmin Chung
 * Description:     Here is Document Factory for creating two kinds of files
 *                  Each Document Factory can create each Element facotry depends on the string element
 *                  Each Document Factory has Get() function as Singleton.
 *                  Each Element Factory can create each detailed element depends on the string value in string array                  
 */

using System;
using System.Collections.Generic;

namespace DocumentFactory
{
    public interface IDocument {
        
        // function for getting file name
        string GetFilename();
        // function for collecting string values 
        string Run();
        // function for adding each element to each element class
        void AddElement(IElement element);
    }

    public interface IElement
    {
        // this function get string value
        string GetStringTemplate();
    }

    public interface IDocumentFactory
    {   // interface function for create document for each document type
        IDocument CreateDocument(string fileName);
        // interface function for create element for each element type
        IElement CreateElement(string elementType, string props);
    }

    // put some classes here: concrete documents, concrete elements, and concrete factories...

    // Concrete Product HtmlDocument 
    public class HtmlDocument : IDocument
    {
        private string _fileName;
        List<IElement> elemList = new List<IElement>();

        public HtmlDocument(string fileName)
        {
            _fileName = fileName;
        }

        public void AddElement(IElement element)
        {
            elemList.Add(element);
        }

        public string GetFilename()
        {
            return _fileName;
        }

        public string Run()
        {
            // hardcoded for the starting point and end point in html document
            string printout = "<!DOCTYPE html><html><head></head ><body>\n";
            foreach (IElement element in elemList)
            {
                printout += element.GetStringTemplate();
            }

            return printout + "\n</body></html>";
        }
    }

    // Concrete Product HtmlHeader
    public class HtmlHeader : IElement
    {
        private string props;
        public HtmlHeader(string props)
        {
            this.props = props;
        }
        public string GetStringTemplate()
        {
            string[] propsList = props.Split(';');
            string headerNum = propsList[0];
            string headerContent = propsList[1];
            string printout = "";

            if (headerNum == "1")
                printout = "<h1>" + headerContent + "</h1>" + "\n";

            else if (headerNum == "2")
                printout = "<h2>" + headerContent + "</h2>" + "\n";

            else if (headerNum == "3")
                printout = "<h3>" + headerContent + "</h3>" + "\n";

            else
                throw new NotImplementedException();

            return printout;
        }


    }

    // Concrete Product HtmlImage
    public class HtmlImage : IElement
    {
        private string props;
        public HtmlImage(string props)
        {
            this.props = props;
        }
        public string GetStringTemplate()
        {
            string[] propsList = props.Split(';');
            return "<img alt='" + propsList[1] + "' title='" + propsList[2] + "' src='" + propsList[0] + "' /><br />" + "\n";
        }
    }

    // Concrete Product HtmlList
    public class HtmlList : IElement
    {
        private string props;
        public HtmlList(string props)
        {
            this.props = props;
        }
        public string GetStringTemplate()
        {
            string[] propsList = props.Split(';');
            string listType = propsList[0];
            string printout = "";

            if (listType == "Ordered")
            {
                for (var i = 1; i < propsList.Length; i++)
                    printout += "<li>" + propsList[i] + "</li>";
                printout = "<ol>" + printout + "</ol>" + "\n";
            }
            else if (listType == "Unordered")
            {
                for (var i = 1; i < propsList.Length; i++)
                    printout += "<li>" + propsList[i] + "</li>";
                printout = "<ul>" + printout + "</ul>" + "\n";
            }

            return printout;
        }
    }

    // Concrete Product HtmlTable
    public class HtmlTable : IElement
    {
        private string props;
        public HtmlTable(string props)
        {
            this.props = props;
        }

        public string GetStringTemplate()
        {
            string printout = "";
            printout += "\n<table>";
            string[] propsList = props.Split(";");

            for (int i = 0; i < propsList.Length; i++)
            {
                string[] tableList = propsList[i].Split("$");
                if (tableList[0] == "Head")
                {
                    tableList[0] = "";
                    printout += "<thead>";
                    printout += "<tr>";
                    for (int j = 1; j < tableList.Length; j++)
                    {
                        printout += "<th>";
                        printout += tableList[j];
                        printout += "</th>";
                    }
                    printout += "</tr>";
                    printout += "</thead>";
                }
                else
                {
                    printout += "<tr>";
                    for (int k = 0; k < tableList.Length; k++)
                    {
                        if (tableList[0] == "Row")
                        {
                            tableList[0] = "";
                            printout += tableList[k];
                        }
                        else
                        {
                            printout += "<td>";
                            printout += tableList[k];
                            printout += "</td>";
                        }
                    }
                    printout += "</tr>";
                }
            }
            printout += "</table>";
            return printout;
        }
    }

    // Concrete Factory 1 : Html Factory
    public class HtmlFactory : IDocumentFactory
    {

        public IDocument CreateDocument(string fileName)
        {
            return new HtmlDocument(fileName);
        }

        public IElement CreateElement(string elementType, string props)
        {
            if (elementType == "Image")
                return new HtmlImage(props);

            else if (elementType == "Header")
                return new HtmlHeader(props);

            else if (elementType == "List")
                return new HtmlList(props);

            else if (elementType == "Table")
                return new HtmlTable(props);

            else
                throw new NotImplementedException();
        }

        // Singleton here
        private static HtmlFactory _instance;

        public static HtmlFactory Get()
        {
            if (_instance == null)
            {
                _instance = new HtmlFactory();
            }
            return _instance;
        }
    }

    // Concrete Product Markdown Document
    public class MdDocument : IDocument
    {
        private string _fileName;
        List<IElement> elemList = new List<IElement>();

        public MdDocument(string fileName)
        {
            _fileName = fileName;
        }

        public void AddElement(IElement element)
        {
            elemList.Add(element);
        }

        public string GetFilename()
        {
            return _fileName;
        }

        public string Run()
        {
            string printout = "";
            foreach (IElement element in elemList)
            {
                printout += element.GetStringTemplate();
            }

            return printout;
        }
    }

    // Concrete Product Markdown Header
    public class MdHeader : IElement
    {
        private string props;
        public MdHeader(string props)
        {
            this.props = props;
        }
        public string GetStringTemplate()
        {
            string[] propsList = props.Split(';');
            string headerNum = propsList[0];
            string headerContent = propsList[1];
            string printout = "";

            if (headerNum == "1")
                printout = "\n# " + headerContent + "\n";

            else if (headerNum =="2")
                printout = "\n## " + headerContent + "\n\n";

            else if (headerNum == "3")
                printout = "\n### " + headerContent + "\n\n";

            else
                throw new NotImplementedException();

            return printout;
        }
    }

    // Concrete Product Markdown Image
    public class MdImage : IElement
    {
        private string props;
        public MdImage(string props)
        {
            this.props = props;
        }
        public string GetStringTemplate()
        {
            string[] propsList = props.Split(';');
            return "![" + propsList[1] + "](" + propsList[0] + " \"" + propsList[2] + "\")\n";
        }
    }

    // Concrete Product Markdown List
    public class MdList : IElement
    {
        private string props;
        public MdList(string props)
        {
            this.props = props;
        }
        public string GetStringTemplate()
        {
            string[] propsList = props.Split(';');
            string listType = propsList[0];
            string printout = "";

            if (listType == "Ordered")
            {
                for(var i = 1; i < propsList.Length; i++)
                    printout += i + ". " + propsList[i] + "\n";
            }
            else if (listType == "Unordered")
            {
                for (var i = 1; i < propsList.Length; i++)
                    printout += "* " + propsList[i] + "\n";
            }

            return printout;
        }
    }

    // Concrete Product Markdown Table
    public class MdTable : IElement
    {
        private string props;
        public MdTable(string props)
        {
            this.props = props;
        }
        public string GetStringTemplate()
        {
            string[] propsList = props.Split(';');
            string printout = "";

            foreach (String tableContent in propsList)
            {

                string[] tableList = tableContent.Split('$');
                if (tableList[0] == "Head")
                {
                    for (var i = 1; i < tableList.Length; i++)
                        printout += " " + tableList[i] + " |";

                    printout += "\n|";

                    for (var j = 1; j < tableList.Length; j++)
                        printout += " --- |";

                    printout += "\n";

                }
                else if (tableList[0] == "Row")
                {
                    printout += "| ";

                    for (var i = 1; i < tableList.Length; i++)
                        printout += tableList[i] + " | ";

                    printout += "\n";
                }
                else
                    throw new NotImplementedException();
            }
            return printout;
        }
    }

    // Concrete Factory 2 : Markdown Factory
    public class MdFactory : IDocumentFactory
    {

        public IDocument CreateDocument(string fileName)
        {
            return new MdDocument(fileName);
        }

        public IElement CreateElement(string elementType, string props)
        {
            if (elementType == "Image")
                return new MdImage(props);

            else if (elementType == "Header")
                return new MdHeader(props);

            else if (elementType == "List")
                return new MdList(props);

            else if (elementType == "Table")
                return new MdTable(props);

            else
                throw new NotImplementedException();
        }

        // Singleton here
        private static MdFactory _instance;

        public static MdFactory Get()
        {
            if (_instance == null)
            {
                _instance = new MdFactory();
            }
            return _instance;
        }
    }
}
