using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.Diagnostics.Eventing.Reader;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Threading;
using System.Diagnostics;

using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using NOGAste;
using System.Security.Policy;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using Markdig.Extensions.Figures;
using System.Drawing;

namespace NOGAste
{




    class Program
    {



        public static void Main(string[] args)
        {

            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("1. Import from CSV");
            Console.WriteLine("2. Select from active Security Log");
            Console.WriteLine("3. Select from active Application Log");
            Console.WriteLine("");
            Console.WriteLine("Select which event source to import ");
            string userInput = Console.ReadLine().Trim();

            if (userInput  == "1")
            {
                PrepareCSVForImport.ReadCSVFile();
            }
            else if (userInput == "2") 
            {
                PrepareEVTForImport.InsertEVT();
            }

            else if (userInput == "2")
            {
                PrepareEVTForImport.InsertEVT();
            }
            Console.WriteLine("Processing Complete");
            Console.WriteLine("\n\n\n\n");


        }//Main



    }//class Program
}//namespace