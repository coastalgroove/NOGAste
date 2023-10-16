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
            //the object is automatically destroyed - to ENSURE
            //the object is disposed of properly

            //The using statement
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
                    //with 27 columns (stdEventFields) into an array
                    string[] stdEventFields = parser.ReadFields();

                    //THESE PROPERTIES ARE STRICTLY MATCHING STANDARD EVT LOG
                    //FOR READING IN FROM CSV - WHICH MATCHES AN EVT
                    if (stdEventFields.Length >= 27)  //this "if" doesn't really need to be here
                    {   //These are variables that correlate to columns(named
                        //headers) in the .CSV file 
                        string Message              = stdEventFields[0];
                        string EventID              = stdEventFields[1];  //Used in final CSV eventInstance
                        string Version              = stdEventFields[2];
                        string Qualifiers           = stdEventFields[3];
                        string Level                = stdEventFields[4];
                        string Task                 = stdEventFields[5];
                        string Opcode               = stdEventFields[6];
                        string Keywords             = stdEventFields[7];
                        string Recordid             = stdEventFields[8];
                        string ProviderName         = stdEventFields[9];

                        string ProviderID           = stdEventFields[10];
                        string LogName              = stdEventFields[11];
                        string ProcessID            = stdEventFields[12];
                        string ThreadID             = stdEventFields[13];
                        string MachineName          = stdEventFields[14];
                        string UserID               = stdEventFields[15];
                        string TimeCreated          = stdEventFields[16];  //Used in final CSV eventInstance
                        string ActivityID           = stdEventFields[17];
                        string RelatedActivity      = stdEventFields[18];
                        string ContainerLog         = stdEventFields[19];

                        string MatchedQueryIDs      = stdEventFields[20];
                        string Bookmark             = stdEventFields[21];
                        string LevelDisplayName     = stdEventFields[22];
                        string OpcodeDisplayName    = stdEventFields[23];
                        string TaskDisplayName      = stdEventFields[24];
                        string KeywordsDisplayNames = stdEventFields[25];
                        string Properties           = stdEventFields[26];



                        //BEGIN Custom Message Field Extraction
                        //THESE ARE CUSTOM PROPERTIES MESSAGE FIELDS FOR DB INSERTION
                        string EventMsg       = "";
                        string LogonType      = "";
                        string ElevToken      = "";
                        string ImpersonateLvl = "";
                        string LogonFail      = "";
                        string KnownUser      = "";     
                        string FailReason     = "";
                        string ProgramRun     = "";
                        string CommandRun     = "";
                        string ProcessInfo    = "";
                        string ObjName        = "";
                        string AppPath        = "";
                        string LogLvl         = "";
                        string Status         = "";
                        string SubStatus      = "";
                        string ReasonEvnt     = "";
                        string ThreatEval     = "";
                        string ActionReqd     = "";

                        //****BEGIN SPLICE****
                        //I want to extact this to its own method but to do so requires that
                        //we pass the Message, EventID, TimeCreated fields and complete the
                        //construction of the ENTIRE final Event instance externally and
                        //pass that back.    Do NOT have time for that now.


                        //Custom class for just for internal "Message" stdEventFields except for
                        //EventID, TimeCreated which we get from the "Official" PSObject stdEventFields
                        customMsgExtractDef customEventFields = new customMsgExtractDef(
                            EventID,            //Already defined standard Log Values above
                            TimeCreated,        //Already defined standard Log Values above
                            EventMsg,           //All remaining fields get defined from Message internals
                            LogonType,
                            ElevToken,
                            ImpersonateLvl,
                            LogonFail,
                            KnownUser,        
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
                            ReasonEvnt,
                            ThreatEval,
                            ActionReqd
                                     );


                        //List<EventLogEntry> customEventLogEntries = new List<EventLogEntry>();
                        List<customMsgExtractDef> customEventLogEntries = new List<customMsgExtractDef>();



                        //========================================
                        //NOTE: At this point "customEventLogEntries" object has all the LogEvent Properties
                        //for ONE event.
                        //I need to ADD additional fields EXTRACTED using 
                        //"StringExtractionTools which creates, returns a DICTIONARY 
                        //with extra custom EventFields

                        //Remember we are in a "while" loop, processing one
                        //"row" i.e. "Event" at a time.
                        //we can OPTIONALLY decide to loop through each extract field from
                        //the event MSG here.  

                        //Uncomment these two lines to see the raw message
                        //Console.WriteLine($"Log Message: {customEventFields.Message}");
                        //Console.ReadLine();


                        //This returns a dictionary of CUSTOM fields extracted from the standard
                        //Event "message" field (a large text blob of varying size and content
                        //with some internal "markers" REPEATED increasing the difficulty of
                        //field identifcation and extraction
                        Dictionary<string, string> msgDict = new Dictionary<string, string>();
                        msgDict = StringExtractionTools.convertMsgToFields(Message);

                        //uncomment to see the "Message"
                        //Console.WriteLine($"Message:{Message}");
                        //Console.ReadLine();

                        //All of the CSV/EVT "Message" fields have been loaded into the msgDict
                        //Now we process each one and extract the information we need that
                        //is NOT available in the "formal" object.fields, we synthesize
                        //extra stdEventFields from "msgDict" and UPDATE "customEventFields"
                        //before its added to "customEventLogEntries"
                        for (int i = 0; i < msgDict.Count; i++)
                        {
                            //Console.WriteLine($"Key: {msgDict.ElementAt(i).Key},  Value: {msgDict.ElementAt(i).Value} ");
                        }
                        //Console.WriteLine($"Finished Processing Message List - a Look at msgDict returned ");
                        //Console.ReadLine();


                        //"EventID" and "TimeCreated" Already Defined from LogEntry

                        if (msgDict.ContainsKey("EventMsg"))
                         {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            customEventFields.EventMsg = msgDict["EventMsg"]; //.Substring(0,50);
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("LogonType"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            customEventFields.LogonType = msgDict["LogonType"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("ElevToken"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            customEventFields.ElevToken = msgDict["ElevToken"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("ImpersonateLvl"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            customEventFields.ImpersonateLvl = msgDict["ImpersonateLvl"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("LogonFail"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            customEventFields.LogonFail = msgDict["LogonFail"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("KnownUser"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            customEventFields.KnownUser = msgDict["KnownUser"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }

                        if (msgDict.ContainsKey("FailReason"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            customEventFields.FailReason = msgDict["FailReason"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("MachineName"))
                        {
                            //Console.WriteLine($"BEFORE: MachineName:{msgDict["MachineName"]} ");
                            customEventFields.MachineName = msgDict["MachineName"];
                            //Console.WriteLine($"AFTER:  MachineName:{msgDict["MachineName"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("UserID"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            customEventFields.UserID = msgDict["UserID"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("ProgramRun"))
                        {
                            //Console.WriteLine($"BEFORE: UserID:{msgDict["UserID"]} ");
                            customEventFields.ProgramRun = msgDict["ProgramRun"];
                            //Console.WriteLine($"AFTER:  UserID:{msgDict["UserID"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("CommandRun"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            customEventFields.CommandRun = msgDict["CommandRun"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("ProcessInfo"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            customEventFields.ProcessInfo = msgDict["ProcessInfo"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("ObjName"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            customEventFields.ObjName = msgDict["ObjName"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("AppPath"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            customEventFields.AppPath = msgDict["AppPath"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("LogLvl"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            customEventFields.LogLvl = msgDict["LogLvl"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("Status"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            customEventFields.Status = msgDict["Status"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("SubStatus"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            customEventFields.SubStatus = msgDict["SubStatus"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }
                        if (msgDict.ContainsKey("ReasonEvnt"))
                        {
                            //Console.WriteLine($"BEFORE: FailReason:{msgDict["FailReason"]} ");
                            customEventFields.ReasonEvnt = msgDict["ReasonEvnt"];
                            //Console.WriteLine($"AFTER:  FailReason:{msgDict["FailReason"]} ");
                            //Console.ReadLine();
                        }
                        //****END SPLICE****



                        //Intance of class "Events"
                        var eventInstance = new Events  //(EventID, TimeCreated, MachineName, UserID);
                        {
                            EventID        = customEventFields.EventID,
                            TimeCreated    = customEventFields.TimeCreated,
                            EventMsg       = customEventFields.EventMsg,
                            LogonType      = customEventFields.LogonType,
                            ElevToken      = customEventFields.ElevToken,
                            ImpersonateLvl = customEventFields.ImpersonateLvl,
                            LogonFail      = customEventFields.LogonFail,
                            KnownUser      = customEventFields.KnownUser,      //not used
                            FailReason     = customEventFields.FailReason,
                            //AfterHours   = customEventFields.AfterHours,
                            //LogonSuccess = customEventFields.LogonSuccess,
                            MachineName    = customEventFields.MachineName,
                            UserID         = customEventFields.UserID,
                            ProgramRun     = customEventFields.ProgramRun,
                            CommandRun     = customEventFields.CommandRun,
                            ProcessInfo    = customEventFields.ProcessInfo,
                            ObjName        = customEventFields.ObjName,
                            AppPath        = customEventFields.AppPath,
                            LogLvl         = customEventFields.LogLvl,
                            Status         = customEventFields.Status,
                            SubStatus      = customEventFields.SubStatus,
                            ReasonEvnt     = customEventFields.ReasonEvnt,
                            ThreatEval     = "TBD", //customEventFields.ThreatEval,
                            ActionReqd     = "TBD", //customEventFields.ActionReqd,
                        };
                        //Adding the customEventFields into the array "customEventLogEntries"
                        //Console.WriteLine($"Writing Event: {csvField} to Array, Total Events: {ttlEvents}");
                        customEventLogEntries.Add(customEventFields);



                        var eventsRepo = new DapperEventsRepository(conn);
                        int j = 0;
                        int k = 0;
                       foreach (var item in customEventLogEntries)
                        {
                            Console.WriteLine($"Event: {j} -----Fields------");
                            Console.WriteLine($"EventID: {customEventFields.EventID}");
                            Console.WriteLine($"TimeCreated: {customEventFields.TimeCreated}");
                            Console.WriteLine($"EventMsg: {item.EventMsg}");
                            Console.WriteLine($"LogonType: {item.LogonType}");
                            Console.WriteLine($"ElevToken: {item.ElevToken}");
                            Console.WriteLine($"ImpersonationLvl: {item.ImpersonateLvl}");
                            Console.WriteLine($"LogonFail: {item.LogonFail}");
                            Console.WriteLine($"KnownUser: {item.KnownUser}");        //not used
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
                            //Console.ReadLine();
                            j++;
                        }//out foreach
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

