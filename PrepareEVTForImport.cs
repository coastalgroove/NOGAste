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
        public static void InsertEVT(string logName)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connString = config.GetConnectionString("DefaultConnection");

            IDbConnection conn = new MySqlConnection(connString);

            //=================================================


            //Open a runspace to enable making a powershell call 
            Runspace runSpace = RunspaceFactory.CreateRunspace();
            runSpace.Open();

            //Create a pipeline:
            Pipeline pipeline = runSpace.CreatePipeline();

            //Create the actual Powershell command:
            Command cmd = new Command("Get-WinEvent");  //Get-WinEvent is the powerhsell command

            //You can add parameters:
            cmd.Parameters.Add("LogName", logName);
            //cmd.Parameters.Add("StartTime", "08:00");
            //cmd.Parameters.Add("EndTime", "09:00");

            //Add it to the pipeline:
            pipeline.Commands.Add(cmd);

            Collection<PSObject> output = pipeline.Invoke();
            //foreach (PSObject psObject in output)
            Console.WriteLine($"Press return to retrieve all active logs in Application Log and insert into DB");
            Console.ReadLine();

            string tmpEvtID   = "";
            string tmpTime    = "";
            string tmpUser    = "";
            string tmpMachine = "";

            int innerCnt  = 0;
            int outterCnt = 0;
            int exceptCnt = 0;
            foreach (PSObject psObject in output)
            {
                Console.WriteLine($"------------------------------------------ \n\n\n\n\n\n");
                //Console.ReadLine();
                Console.WriteLine("Properties and Values:");
                foreach (var property in psObject.Properties)
                {
                    try
                    {
                        //Console.WriteLine($"-----------------------------------------\n");
                        Console.WriteLine($"----------------INNER LOOP TOP---------------\n");
                        Console.WriteLine($"Property:>{property.Name}< Value:>{property.Value}<");
                        //Console.ReadLine();

                        //zzz

                        if ((property.Name).Trim() == "Id")
                        {
                           tmpEvtID = (property.Value).ToString();
                           Console.WriteLine($"EventID:{tmpEvtID}");
                           Console.WriteLine($"-----------------WE GOT A   >>Id<<   HIT!!!-------------****************--");
                           //Console.ReadLine();
                        }

                        if ((property.Name).Trim() == "TimeCreated")
                        {
                           tmpTime = (property.Value).ToString();
                            Console.WriteLine($"TimeCreated:{tmpTime}");
                            Console.WriteLine($"-----------------WE GOT A   >>TIME<<<  HIT!!!-----------****************--");
                            //Console.ReadLine();
                        }

                        if ((property.Name).Trim() == "UserID")
                        {
                           tmpUser = (property.Value).ToString();
                            Console.WriteLine($"UserID:{tmpUser}");
                            Console.WriteLine($"-----------------WE GOT A UserID HIT!!!--------****************--");

                            //Console.ReadLine();
                        }
                        if ((property.Name).Trim() == "MachineName")
                        {
                           tmpMachine = (property.Value).ToString();
                            Console.WriteLine($"MachineName:{tmpMachine}");
                            Console.WriteLine($"-----------------WE GOT A Machine HIT!!!--------****************--");
                            //Console.ReadLine();
                        }

                        Console.WriteLine($"Property:>{property.Name}< Value:>{property.Value}<\n");
                        Console.WriteLine($"Ttl Properties:{innerCnt}, Fail:{exceptCnt}, Events:{outterCnt}\n");
                        //Console.WriteLine("--------INNER LOOP BOTTOM------\n");
                        //Console.ReadLine();
                        innerCnt++;
                    }//End Try
                    catch (Exception ex)
                    {
                        //Console.Write(ex.ToString());
                        exceptCnt++;
                        Console.Write($"TryCatch Ttl Properties:{innerCnt}, Fail:{exceptCnt}, Events:{outterCnt}, Unable to proces this event.......\n");
                        //Console.ReadLine();
                    }


                    
                }//End foreach inner

                if (tmpEvtID == "")
                {
                    tmpEvtID = "9999";
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
                    EventID     = tmpEvtID,
                    TimeCreated = tmpTime,
                    UserID      = tmpUser,
                    MachineName = tmpMachine
                };

                var eventsRepo = new DapperEventsRepository(conn);
                //Insert into DB
                eventsRepo.InsertEvents(eventInstance);
                Console.Write($"OUTTER Event Processed, TtlRecords Processed:{outterCnt} ID:{tmpEvtID}, Time:{tmpTime}, User:{tmpUser}, Host:{tmpMachine}\n");

 
                //Console.ReadLine();
                innerCnt = 0;
                exceptCnt = 0;
                outterCnt++;
            }//foreach outter
             Console.Write($"Total Events Procesed,  Events:{outterCnt}, Total Fail:{exceptCnt}......\n");
             Console.ReadLine();
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
