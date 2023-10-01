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

namespace NOGAste
{







    class Program
    {

        public static void ReadCSVFile()
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

        }//ReadCSVFile



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
            cmd.Parameters.Add("LogName", "security");
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
                    Console.WriteLine($"{property.Name}: {property.Value} ");

                }//foreach inner
            }//foreach outter

        }//Method ReadEVT











        static void Main(string[] args)
        {

            //ReadCSVFile();
            ReadEVT();
        }//Main



    }//class Program
}//namespace