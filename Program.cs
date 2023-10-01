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

namespace NOGAste
{
    //    public class EventLogEntry
    //    {
    //        //Properties
    //        public string Message { get; set; }
    //        public string EventID { get; set; }
    //        public string Version { get; set; }
    //        public string Qualifiers { get; set; }
    //       public string Level { get; set; }
    //      public string Task { get; set; }
    //      public string Opcode { get; set; }
    //      public string Keywords { get; set; }
    //      public string RecordID { get; set; }
    //      public string ProviderName { get; set; }


    //       public string ProviderID { get; set; }
    //       public string LogName { get; set; }
    //       public string ProcessID { get; set; }
    //       public string ThreadID { get; set; }
    //       public string MachineName { get; set; }
    //       public string UserID { get; set; }
    //       public string TimeCreated { get; set; }
    //       public string ActivityID { get; set; }
    //       public string RelatedActivity { get; set; }
    //       public string ContainerLog { get; set; }


    //        public string MatchedQueryIDs { get; set; }
    //        public string Bookmark { get; set; }
    //        public string LevelDisplayName { get; set; }
    //        public string OpcodeDisplayName { get; set; }
    //        public string TaskDisplayName { get; set; }
    //        public string KeywordsDisplayNames { get; set; }
    //        public string Properties { get; set; }

    //Constructor with parameters
    //       public EventLogEntry(
    //                              string message,
    //                              string eventID,
    //                              string version,
    //                              string qualifiers,
    //                              string level,
    //                              string task,
    //                              string opcode,
    //                              string keywords,
    //                              string recordID,
    //                              string providerName,
    //
    //                               string providerID,
    //                               string logName,
    //                               string processID,
    //                               string threadID,
    //                               string machineName,
    //                               string userID,
    //                               string timeCreated,
    //                               string activityID,
    //                               string relatedActivity,
    //                               string containerLog,

    //                               string matchedQueryIDs,
    //                               string bookmark,
    //                               string levelDisplayName,
    //                               string opcodeDisplayName,
    //                               string taskDisplayName,
    //                               string keywordsDisplayNames,
    //                               string properties)

    //        { //Body of method constructor, Initialize properites with the parameters

    //                                 Message              = message;
    //                                 EventID              = eventID;
    //                                 Version              = version;
    //                                 Qualifiers           = qualifiers;
    //                                 Level                = level;
    //                                 Task                 = task;
    //                                 Opcode               = opcode;
    //                                 Keywords             = keywords;
    //                                 RecordID             = recordID;
    //                                 ProviderName         = providerName ;
    //                                 ProviderID           = providerID;
    //                                 LogName              = logName;
    //                                 ProcessID            = processID;
    //                                 ThreadID             = threadID;
    //                                 MachineName          = machineName;
    //                                 UserID               = userID;
    //                                 TimeCreated          = timeCreated;
    //                                 ActivityID           = activityID;
    //                                 RelatedActivity      = relatedActivity;
    //                                 ContainerLog         = containerLog;
    //                                 MatchedQueryIDs      = matchedQueryIDs;
    //                                 Bookmark             = bookmark;
    //                                 LevelDisplayName     = levelDisplayName;
    //                                 OpcodeDisplayName    = opcodeDisplayName;
    //                                 TaskDisplayName      = taskDisplayName;
    //                                 KeywordsDisplayNames = keywordsDisplayNames;
    //                                 Properties           = properties;
    //       }//End method Body of EventLogentry Constructor



    //}//Class EventLogEntry









    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "C:\\Users\\mcguertyj\\Documents\\Repos\\ZZZTest\\Raw_Events_Log_v0.4.9_100123.csv"; // Provide the correct path if the file is in a different location


            //List<EventLogEntry> eventLogEntries = new List<EventLogEntry>();
            List<EventLogCSVImport> eventLogEntries = new List<EventLogCSVImport>();


            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // Skip the header row if needed
                parser.ReadLine();

                string acctName = "";

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    if (fields.Length >= 5)
                    {
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

                        //EventLogEntry logEntry = new EventLogEntry(

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

                        eventLogEntries.Add(logEntry);
                        Console.WriteLine($"----------------------------------------");
                        Console.WriteLine($"EventID:    {logEntry.EventID}");
                        Console.WriteLine($"Event Time: {logEntry.TimeCreated}");
                        Console.WriteLine($"MachineName:{logEntry.MachineName}");

                        //This converts monoloithic msg text to distinct fields and values (best try)
                        List<string> msgFieldList = new List<string>();
                        msgFieldList = StringExtractionTools.convertMsgToFields(logEntry.Message);
                        Console.WriteLine("------Completed List--------");
                        foreach (string field in msgFieldList)
                        {
                            Console.WriteLine(field);
                        }
                        //Console.ReadLine();

                        //Console.WriteLine($"UserID:     {logEntry.UserID}");

                        //-----Account Name------------
                        //acctName = StringExtractionTools.ExtractFieldDelimBoundary(logEntry.Message, "Account Name", "Account Domain");
                        //
                        //acctName = StringExtractionTools.ExtractFromListReturnSubStr(msgFieldList, "Account Name");

                        //Console.WriteLine($"UserName:   {acctName}");

                        //-----Process Name------------
                        //-----Process Cmd Line------------
                        //-----Object Name------------
                        //index = logEntry.Message.IndexOf("Account Name");
                        //index2 = logEntry.Message.IndexOf("Account Domain");


                        //Console.WriteLine($"EventMsg:   {logEntry.Message}");

                        //Console.ReadLine();

                    }//if
                }//while

            }//using

            // Now, you have a list of EventLogEntry objects created from the CSV file data in eventLogEntries.
            // You can work with this list as needed.
        }//Main



    }//class Program
}//namespace