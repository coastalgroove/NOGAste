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

namespace NOGAste
{




    class Program
    {



        public static void Main(string[] args)
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connString = config.GetConnectionString("DefaultConnection");

            IDbConnection conn = new MySqlConnection(connString);

            //=================================================

            //Console.WriteLine("Press return to begin to retrieve Events");
            //Console.ReadLine();
            //var eventsRepo = new DapperEventsRepository(conn);
            //var eventsReturned = eventsRepo.GetEvents();

            //foreach (var blah in eventsReturned)
            //{
            //    Console.WriteLine($"Event: {blah.EventID}, MachineName: {blah.MachineName}, UserName: {blah.UserName},  ");
            //    Console.WriteLine("\n");
            //    Console.ReadLine();
            // }


            //=================================================
            //Console.WriteLine("Press return to begin to insert Events");
            //Console.ReadLine();

            //var eventInstance = new Events
            //{
            //    EventID = 999, 
            //    TimeCreated = "13:00:00 08:21",
            //    MachineName = "TimeMachine",
            //    UserID      = "JamesBond"
            //};

            //var eventsRepo = new DapperEventsRepository(conn);
            //eventsRepo.InsertEvents(eventInstance);

            //PrepareCSVForImport.ReadCSVFile();
            //PrepareEVTForImport.ReadEVT();
            PrepareEVTForImport.InsertEVT();
        }//Main



    }//class Program
}//namespace