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
using Microsoft.PowerShell.Commands;

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
            Console.WriteLine("4. Print out entire DB Events");
            Console.WriteLine("5. Search for use of malicious program use");
            Console.WriteLine("Select which event source to import ");
            string userInput = Console.ReadLine().Trim();

            if (userInput  == "1")
            {
                PrepareCSVForImport.ReadCSVFile();

            }
            else if (userInput == "2") 
            {
                Console.WriteLine("Must be running as ADMINISTRATOR");
                Console.ReadLine();
                PrepareEVTForImport.InsertEVT("security");


            }

            else if (userInput == "3")
            {
                PrepareEVTForImport.InsertEVT("application");

            }

            else if (userInput == "4")
            {
                var config = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json")
                  .Build();

                string connString = config.GetConnectionString("DefaultConnection");

                IDbConnection conn = new MySqlConnection(connString);

                //=================================================

                var eventsRepo = new DapperEventsRepository(conn);
                var events = eventsRepo.GetAllEvents();
                int i = 0;
                foreach (var field in events)
                {
                    Console.WriteLine($"Nbr:{i}, ID: {field.EventID}, Time: {field.TimeCreated} UserID: {field.UserID}, Machine: {field.MachineName}");
                    i++;
                }
                Console.WriteLine($"{i} Events Retrieved");
            }

            else if (userInput == "5")
            {
                var config = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json")
                  .Build();

                string connString = config.GetConnectionString("DefaultConnection");

                IDbConnection conn = new MySqlConnection(connString);

                //=================================================

                var eventsRepo = new DapperEventsRepository(conn);
                var events = eventsRepo.GetMaliciousProgram();
                int i = 0;
                foreach (var field in events)
                {
                    Console.WriteLine($"Nbr:{i}, ID: {field.EventID}, Time: {field.TimeCreated} UserID: {field.UserID}, Machine: {field.MachineName}, Program: {field.CommandRun}");
                    i++;
                }
                Console.WriteLine($"{i} Events Retrieved");
            }


            Console.WriteLine("Processing Complete");
            Console.WriteLine("\n\n\n\n");


        }//Main



    }//class Program
}//namespace