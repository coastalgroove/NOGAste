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

namespace NOGAste
{







    class Program
    {
        //Method
        public static void ReadCSVFile()
        {
            string filePath = "C:\\Users\\mcguertyj\\Documents\\Repos\\ZZZTest\\Raw_Events_Log_v0.4.9_100123.csv";


            //Defines a boundary for the object outside of which
            //the object is automatically destroyed.  The using statement
            //is exited when exiting the using block
            //The "using" statement allows you to specify multiple resources in a
            //single statement. The object could also be created outside the "using"
            //statement. The objects specified within the using block must implement
            //the IDisposable interface. The framework invokes the Dispose method of
            //objects specified within the "using" statement when the block is exited.
            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // Skip the header row if needed
                parser.ReadLine();

                string acctName = "";




                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connString = config.GetConnectionString("DefaultConnection");

                IDbConnection conn = new MySqlConnection(connString);

                //=================================================




                while (!parser.EndOfData)
                {
                    //define a string array that receives extracted each rows
                    //with 27 columns (fields) into an array
                    string[] fields = parser.ReadFields();


                    if (fields.Length >= 27)  //this "if" doesn't really need to be here
                    {   //These are variables that correlate to columns(named headers) in the .CSV file
                        //indexed 
                        string Message              = fields[0];
                        string EventID              = fields[1];
                        string Version              = fields[2];
                        string Qualifiers           = fields[3];
                        string Level                = fields[4];
                        string Task                 = fields[5];
                        string Opcode               = fields[6];
                        string Keywords             = fields[7];
                        string Recordid             = fields[8];
                        string ProviderName         = fields[9];
                         
                        string ProviderID           = fields[10];
                        string LogName              = fields[11];
                        string ProcessID            = fields[12];
                        string ThreadID             = fields[13];
                        string MachineName          = fields[14];
                        string UserID               = fields[15];
                        string TimeCreated          = fields[16];
                        string ActivityID           = fields[17];
                        string RelatedActivity      = fields[18];
                        string ContainerLog         = fields[19];

                        string MatchedQueryIDs      = fields[20];
                        string Bookmark             = fields[21];
                        string LevelDisplayName     = fields[22];
                        string OpcodeDisplayName    = fields[23];
                        string TaskDisplayName      = fields[24];
                        string KeywordsDisplayNames = fields[25];
                        string Properties           = fields[26];


                        //I tried this but could not get it to work
                        //EventLogEntry logEntry = new EventLogEntry()

                        //So I created my own class EventLogCSVImport
                        EventLogCSVImport logEntry = new EventLogCSVImport(
                                     Message,
                                     EventID,
                                     Version,
                                     Qualifiers,
                                     Level,
                                     Task,
                                     Opcode,
                                     Keywords,
                                     Recordid,
                                     ProviderName,

                                     ProviderID,
                                     LogName,
                                     ProcessID,
                                     ThreadID,
                                     MachineName,
                                     UserID,
                                     TimeCreated,
                                     ActivityID,
                                     RelatedActivity,
                                     ContainerLog,

                                     MatchedQueryIDs,
                                     Bookmark,
                                     LevelDisplayName,
                                     OpcodeDisplayName,
                                     TaskDisplayName,
                                     KeywordsDisplayNames,
                                     Properties
                                     );


                        //List<EventLogEntry> eventLogEntries = new List<EventLogEntry>();
                        List<EventLogCSVImport> eventLogEntries = new List<EventLogCSVImport>();



                        //========================================
                        //NOTE: At this point "eventLogEntries" object is has all the LogEvent Properties
                        //for ONE event.
                        //I need to ADD additional field EXTRACTED using 
                        //"StringExtractionTools which creates, returns a DICTIONARY 
                        //with extra fields I need to add to "eventLogEntries" object

                        //Remember we are in a "while" loop, processing one
                        //"row" i.e. "Event" at a time.
                        //we can OPTIONALLY decide to loop through each extract field from
                        //the event MSG here.  

                        //Uncomment these two lines to see the raw message
                        //Console.WriteLine($"Log Message: {logEntry.Message}");
                        //Console.ReadLine();


                        //This converts a monoloithic msg text to distinct fields and values (best try)
                        Dictionary<string, string> msgDict = new Dictionary<string, string>();
                        msgDict = StringExtractionTools.convertMsgToFields(logEntry.Message);

                        //NOW, access the extra fields from "msgDict" and add to "eventLogEntries"



                        eventLogEntries.Add(logEntry);
                        //Console.WriteLine($"----------------------------------------");
                        //Console.WriteLine($"EventID:    {logEntry.EventID}");
                        //Console.WriteLine($"MachineName:{logEntry.MachineName}");
                        //Console.WriteLine($"Event Time: {logEntry.TimeCreated}");

                        if (msgDict.ContainsKey("UserID"))
                        {
                            logEntry.UserID = msgDict["UserID"];
                        }
                        if (msgDict.ContainsKey("MachineName"))
                        {
                            logEntry.MachineName = msgDict["MachineName"];
                        }

                        var eventsRepo = new DapperEventsRepository(conn);
                        var eventInstance = new Events(); //(EventID, TimeCreated, MachineName, UserID);
                        eventsRepo.InsertEvents(eventInstance);



                    }//if

                }//while

            }//using

        }//ReadCSVFile Method






        public static void ReadEVT()
        {
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











        public static void Main(string[] args)
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connString = config.GetConnectionString("DefaultConnection");

            IDbConnection conn = new MySqlConnection(connString);

            //=================================================

            Console.WriteLine("Press return to begin to retrieve Events");
            Console.ReadLine();
            //var eventsRepo = new DapperEventsRepository(conn);
            //var eventsReturned = eventsRepo.GetEvents();

            //foreach (var blah in eventsReturned)
            //{
            //    Console.WriteLine($"Event: {blah.EventID}, MachineName: {blah.MachineName}, UserName: {blah.UserName},  ");
            //    Console.WriteLine("\n");
            //    Console.ReadLine();
            // }

          
            //=================================================
            Console.WriteLine("Press return to begin to insert Events");
            Console.ReadLine();

            //var eventInstance = new Events
            //{
            //    EventID = 999, // Replace with actual values
            //    TimeCreated = "13:00:00 08:21",
            //    MachineName = "TimeMachine", // Replace with actual values
            //    UserID      = "JamesBond" // Replace with actual values
            //};

            //var eventsRepo = new DapperEventsRepository(conn);
            //eventsRepo.InsertEvents(eventInstance);

            ReadCSVFile();
            //ReadEVT();
        }//Main



    }//class Program
}//namespace