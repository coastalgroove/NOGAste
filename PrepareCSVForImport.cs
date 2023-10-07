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
using System.Dynamic;
using System.ServiceModel;

namespace NOGAste
{






    public static  class PrepareCSVForImport
    {

        //Method
        public static void ReadCSVFile()
        {
            string filePath = "C:\\Users\\mcguertyj\\Documents\\Repos\\NOGAste\\Raw_Events_Log_v0.5.3_100523.csv";


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

                    //THESE PROPERTIES ARE STRICTLY MATCHING STANDARD EVT LOG
                    //FOR READING IN FROM CSV - WHICH MATCHES AN EVT
                    if (fields.Length >= 27)  //this "if" doesn't really need to be here
                    {   //These are variables that correlate to columns(named
                        //headers) in the .CSV file 
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

                        

                        string EventMsg       = "";
                        string LogonType      = "";
                        string ElevToken      = "";
                        string ImpersonateLvl = "";
                        string LogonFail      = "";
                        string FailInfo       = "";
                        string FailReason     = "";
                        string ProgramRun     = "";
                        string CommandRun     = "";
                        string ProcessInfo    = "";
                        string ObjName        = "";
                        string AppPath        = "";
                        string LogLvl         = "";
                        string Status         = "";
                        string SubStatus      = "";
                        string Reason         = "";

                        //So I created my own class EventLogCSVImport
                        EventLogCSVImport logEntry = new EventLogCSVImport(
                            EventID,
                            TimeCreated,
                            EventMsg,
                            LogonType,
                            ElevToken,
                            ImpersonateLvl,
                            LogonFail,
                            FailInfo,
                            FailReason,
                            //public string   AfterHours,
                            //public string   LogonSuccess,
                            MachineName,
                            UserID,
                            ProgramRun,
                            CommandRun,
                            ProcessInfo,
                            ObjName,
                            AppPath,
                            LogLvl,
                            Status,
                            SubStatus,
                            Reason
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
                        msgDict = StringExtractionTools.convertMsgToFields(Message);

                        //NOW, access the extra fields from "msgDict" and UPDATE "logEntry"
                        //before its added to "eventLogEntries"

                       

                        for (int i = 0; i < msgDict.Count; i++)
                        {
                            //Console.WriteLine($"Key: {msgDict.ElementAt(i).Key},  Value: {msgDict.ElementAt(i).Value} ");
                        }
                        //Console.WriteLine($"Finished Processing Message List - a Look at msgDict returned ");
                        //Console.ReadLine();


                        //EventID Already Defined from LogEntry

                        if (msgDict.ContainsKey("EventMsg"))
                         {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            logEntry.EventMsg = msgDict["EventMsg"]; //.Substring(0,50);
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("LogonType"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            logEntry.LogonType = msgDict["LogonType"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("ElevToken"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            logEntry.ElevToken = msgDict["ElevToken"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("ImpersonateLvl"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            logEntry.ImpersonateLvl = msgDict["ImpersonateLvl"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("LogonFail"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            logEntry.LogonFail = msgDict["LogonFail"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("FailInfo"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            logEntry.FailInfo = msgDict["FailInfo"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }

                        if (msgDict.ContainsKey("FailIReason"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            logEntry.FailReason = msgDict["FailReason"];
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
                        if (msgDict.ContainsKey("UserID"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            logEntry.UserID = msgDict["UserID"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("ProgramRun"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            logEntry.ProgramRun = msgDict["ProgramRun"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("CommandRun"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            logEntry.CommandRun = msgDict["CommandRun"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("ProcessInfo"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            logEntry.ProcessInfo = msgDict["ProcessInfo"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("ObjName"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            logEntry.ObjName = msgDict["ObjName"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("AppPath"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            logEntry.AppPath = msgDict["AppPath"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("LogLvl"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            logEntry.LogLvl = msgDict["LogLvl"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("Status"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            logEntry.Status = msgDict["Status"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("SubStatus"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            logEntry.SubStatus = msgDict["SubStatus"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("ReasonEvnt"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            logEntry.ReasonEvnt = msgDict["ReasonEvnt"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }




                        //Intance of class "Events"
                        var eventInstance = new Events  //(EventID, TimeCreated, MachineName, UserID);
                        {
                            EventID        = logEntry.EventID,
                            TimeCreated    = logEntry.TimeCreated,
                            EventMsg       = logEntry.EventMsg,
                            LogonType      = logEntry.LogonType,
                            ElevToken      = logEntry.ElevToken,
                            ImpersonateLvl = logEntry.ImpersonateLvl,
                            LogonFail      = logEntry.LogonFail,
                            FailInfo       = logEntry.FailInfo,
                            FailReason     = logEntry.FailReason,
                            //AfterHours     = logEntry.AfterHours,
                            //LogonSuccess   = logEntry.LogonSuccess,
                            MachineName    = logEntry.MachineName,
                            UserID         = logEntry.UserID,
                            ProgramRun     = logEntry.ProgramRun,
                            CommandRun     = logEntry.CommandRun,
                            ProcessInfo    = logEntry.ProcessInfo,
                            ObjName        = logEntry.ObjName,
                            AppPath        = logEntry.AppPath,
                            LogLvl         = logEntry.LogLvl,
                            Status         = logEntry.Status,
                            SubStatus      = logEntry.SubStatus,
                            ReasonEvnt     = logEntry.ReasonEvnt
                            //ThreatEval     = logEntry.ThreatEval,
                            //ActionReqd     = logEntry.ActionReqd,
                        };
                        //Adding the logEntry into the array "eventLogEntries"
                        //Console.WriteLine($"Writing Event: {csvField} to Array, Total Events: {ttlEvents}");
                        eventLogEntries.Add(logEntry);

                        var eventsRepo = new DapperEventsRepository(conn);
                        int j = 0;
                        int k = 0;
                       foreach (var item in eventLogEntries)
                        {
                            Console.WriteLine($"Event: {j} -----Fields------");
                            Console.WriteLine($"EventID: {logEntry.EventID}");
                            Console.WriteLine($"TimeCreated: {logEntry.TimeCreated}");
                            Console.WriteLine($"EventMsg: {item.EventMsg}");
                            Console.WriteLine($"LogonType: {item.LogonType}");
                            Console.WriteLine($"ElevToken: {item.ElevToken}");
                            Console.WriteLine($"ImpersonationLvl: {item.ImpersonateLvl}");
                            Console.WriteLine($"LogonFail: {item.LogonFail}");
                            Console.WriteLine($"FailInfo: {item.FailInfo}");
                            Console.WriteLine($"FailReason: {item.FailReason}");
                            Console.WriteLine($"MachineName: {item.MachineName}");
                            Console.WriteLine($"UserID: {item.UserID}");
                            Console.WriteLine($"ProgramRun: {item.ProgramRun}");
                            Console.WriteLine($"CommandRun: {item.CommandRun}");
                            Console.WriteLine($"ProcessInfo: {item.ProcessInfo}");
                            Console.WriteLine($"ObjName: {item.ObjName}");
                            Console.WriteLine($"AppPath: {item.AppPath}");
                            Console.WriteLine($"LogLvl: {item.LogLvl}");
                            Console.WriteLine($"Status: {item.Status}");
                            Console.WriteLine($"SubStatus: {item.SubStatus}");
                            Console.WriteLine($"ReasonEvnt: {item.ReasonEvnt}");
                            Console.ReadLine();
                            j++;
                        }//out foreach
                        //Insert into DB
                        eventsRepo.InsertEvents(eventInstance);
                        Console.ReadLine() ;
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

