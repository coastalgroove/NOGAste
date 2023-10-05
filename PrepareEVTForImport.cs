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
    public static  class PrepareEVTForImport
    {
        public static void InsertEVT()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connString = config.GetConnectionString("DefaultConnection");

            IDbConnection conn = new MySqlConnection(connString);

            //=================================================


            //Open a runspace:
            Runspace runSpace = RunspaceFactory.CreateRunspace();
            runSpace.Open();

            //Create a pipeline:
            Pipeline pipeline = runSpace.CreatePipeline();

            //Create a command:
            Command cmd = new Command("Get-WinEvent");

            //You can add parameters:
            cmd.Parameters.Add("LogName", "application");
            //cmd.Parameters.Add("StartTime", "08:00");
            //cmd.Parameters.Add("EndTime", "09:00");

            //Add it to the pipeline:
            pipeline.Commands.Add(cmd);

            Collection<PSObject> output = pipeline.Invoke();
            //foreach (PSObject psObject in output)

            string tmpEvtID   = "";
            string tmpTime    = "";
            string tmpUser    = "";
            string tmpMachine = "";

            foreach (PSObject psObject in output)
            {
                Console.WriteLine($"------------------------------------------ \n\n\n\n\n\n");
                Console.ReadLine();
                //Console.WriteLine("Properties and Values:");
                foreach (var property in psObject.Properties)
                {

                    try
                    {
                        //zzz
                        if (property.Name == "ID")
                        {
                           tmpEvtID = (string)property.Value;
                           Console.WriteLine($"EventID:{tmpEvtID}");
                        }

                        if (property.Name == "TimeCreated")
                        {
                           tmpTime = (string)property.Value;
                           Console.WriteLine($"TimeCreated:{tmpTime}");
                        }

                        if (property.Name == "UserID")
                        {
                           tmpUser = (string)property.Value;
                           Console.WriteLine($"UserID:{tmpUser}");
                        }
                        if (property.Name == "MachineName")
                        {
                            tmpMachine = (string)property.Value;
                            Console.WriteLine($"MachineName:{tmpMachine}");
                        }




                    }//End Try
                    catch (Exception ex)
                    {
                        //Console.Write(ex.ToString());
                        Console.Write("Unable to proces this event.......\n");
                        //Console.ReadLine();
                    }



                }//End foreach inner

                if (tmpEvtID == "")
                {
                    tmpEvtID = "4625";
                }

                if (tmpTime == "")
                {
                    tmpTime = "13:00:00 08:21";
                }

                if (tmpUser == "")
                {
                    tmpUser = "JQPublic";
                }

                if (tmpMachine == "")
                {
                    tmpMachine = "SecretSquirrel";
                }

                var eventInstance = new Events  //(EventID, TimeCreated, MachineName, UserID);
                {
                    EventID = tmpEvtID,
                    TimeCreated = tmpTime,
                    UserID = tmpUser,
                    MachineName = tmpMachine
                };

                var eventsRepo = new DapperEventsRepository(conn);
                //Insert into DB
                eventsRepo.InsertEvents(eventInstance);
                Console.Write($"Event Processed, ID:{tmpEvtID}, Time:{tmpTime}, User:{tmpUser}, Host:{tmpMachine}\n");
                Console.ReadLine();

            }//foreach outter

        }//Method InsertEVT




        public static void ReadEVT()
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connString = config.GetConnectionString("DefaultConnection");

            IDbConnection conn = new MySqlConnection(connString);

            //=================================================


            //Open a runspace:
            Runspace runSpace = RunspaceFactory.CreateRunspace();
            runSpace.Open();

            //Create a pipeline:
            Pipeline pipeline = runSpace.CreatePipeline();

            //Create a command:
            Command cmd = new Command("Get-WinEvent");

            //You can add parameters:
            cmd.Parameters.Add("LogName", "application");
            //cmd.Parameters.Add("StartTime", "08:00");
            //cmd.Parameters.Add("EndTime", "09:00");

            //Add it to the pipeline:
            pipeline.Commands.Add(cmd);

            Collection<PSObject> output = pipeline.Invoke();
            //foreach (PSObject psObject in output)



            foreach (PSObject psObject in output)
            {
                Console.WriteLine($"------------------------------------------ \n\n\n\n\n\n");
                Console.ReadLine();
                Console.WriteLine("Properties and Values:");
                foreach (var property in psObject.Properties)
                {

                    try
                    {
                        //zzz
                        if (property.Name == "Id" || property.Name == "MachineName" || property.Name == "TimeCreated")
                        {
                            Console.WriteLine($"{property.Name}: {property.Value} ");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.Write(ex.ToString());
                        Console.Write("Unable to proces this event.......");
                        Console.ReadLine();
                    }
                }//foreach inner
            }//foreach outter

        }//Method ReadEVT






    }//class
}//namespace
