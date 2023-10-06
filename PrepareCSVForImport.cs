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






    public static  class PrepareCSVForImport
    {

        //Method
        public static void ReadCSVFile()
        {
            string filePath = "C:\\Users\\mcguertyj\\Documents\\Repos\\NOGAste\\Raw_Events_Log_v0.5.1_100523.csv";


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

                Console.WriteLine($"Press return to retrieve events logs from CSV and insert into DB");
                Console.ReadLine();
                int csvField = 0;
                int ttlEvents = 0;
                while (!parser.EndOfData)
                {
                    //define a string array that receives extracted each rows
                    //with 27 columns (fields) into an array
                    string[] fields = parser.ReadFields();


                    if (fields.Length >= 27)  //this "if" doesn't really need to be here
                    {   //These are variables that correlate to columns(named
                        //headers) in the .CSV file 
                        string Message = fields[0];
                        string EventID = fields[1];
                        string Version = fields[2];
                        string Qualifiers = fields[3];
                        string Level = fields[4];
                        string Task = fields[5];
                        string Opcode = fields[6];
                        string Keywords = fields[7];
                        string Recordid = fields[8];
                        string ProviderName = fields[9];

                        string ProviderID = fields[10];
                        string LogName = fields[11];
                        string ProcessID = fields[12];
                        string ThreadID = fields[13];
                        string MachineName = fields[14];
                        string UserID = fields[15];
                        string TimeCreated = fields[16];
                        string ActivityID = fields[17];
                        string RelatedActivity = fields[18];
                        string ContainerLog = fields[19];

                        string MatchedQueryIDs = fields[20];
                        string Bookmark = fields[21];
                        string LevelDisplayName = fields[22];
                        string OpcodeDisplayName = fields[23];
                        string TaskDisplayName = fields[24];
                        string KeywordsDisplayNames = fields[25];
                        string Properties = fields[26];


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

                        //NOW, access the extra fields from "msgDict" and UPDATE "logEntry"
                        //before its added to "eventLogEntries"

                       

                        for (int i = 0; i < msgDict.Count; i++)
                        {
                            //Console.WriteLine($"Key: {msgDict.ElementAt(i).Key},  Value: {msgDict.ElementAt(i).Value} ");
                        }
                        //Console.WriteLine($"Finished Processing Message List - a Look at msgDict returned ");
                        //Console.ReadLine();



                        if (msgDict.ContainsKey("UserID"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            logEntry.UserID = msgDict["UserID"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("MachineName"))
                        {
                            //Console.WriteLine($"BEFORE: MachineName:{msgDict["MachineName"]} ");
                            logEntry.MachineName = msgDict["MachineName"];
                            //Console.WriteLine($"AFTER:  MachineName:{msgDict["MachineName"]} ");
                            //Console.ReadLine();
                        }

                        if (msgDict.ContainsKey("FailReason"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            logEntry.UserID = msgDict["FailReason"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }

                        //if (msgDict.ContainsKey("UserID") && msgDict.ContainsKey("MachineName") || msgDict.ContainsKey("Failure_Reason") );
                        //{
                        var eventInstance = new Events  //(EventID, TimeCreated, MachineName, UserID);
                        {
                            EventID = logEntry.EventID,
                            TimeCreated = logEntry.TimeCreated,
                            UserID = logEntry.UserID,
                            MachineName = logEntry.MachineName
                        };
                        //Adding the logEntry into the array "eventLogEntries"
                        //Console.WriteLine($"Writing Event: {csvField} to Array, Total Events: {ttlEvents}");
                        eventLogEntries.Add(logEntry);

                        var eventsRepo = new DapperEventsRepository(conn);
                        //Insert into DB
                        eventsRepo.InsertEvents(eventInstance);
                        //Console.ReadLine() ;
                        csvField++;
                        //}
                    }//if
                    ttlEvents++;
                }//while
                Console.Write($"Total Events Procesed: {ttlEvents}.....\n");
                Console.ReadLine();
            }//using

        }//ReadCSVFile Method




    }//class
}//namespace

