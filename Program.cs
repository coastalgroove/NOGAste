using System.Management.Automation;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;
using System.Drawing;
using System;
using System.Collections.ObjectModel;
//using Microsoft.VisualBasic;

namespace NOGAste
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Open a runspace:
            Runspace runSpace = RunspaceFactory.CreateRunspace();
            runSpace.Open();

            //Create a pipeline:
            Pipeline pipeline = runSpace.CreatePipeline();

            //Create a command:
            Command cmd = new Command("Get-WinEvent");

            var filterHashTable = new System.Collections.Hashtable();
            //You can add parameters:
            cmd.Parameters.Add("LogName", "security");
            //filterHashTable.Add("LogName", "Security");
            //filterHashTable.Add("StartTime", DateTime.Now.Date); // Earlier time
            //filterHashTable.Add("EndTime", DateTime.Now.Date.AddDays(0).AddHours(0).AddMinutes(-30).AddSeconds(0)); // Later time

            // Add the filter hash table as a parameter:
            //cmd.Parameters.Add("FilterHashtable", filterHashTable); // Correct parameter name "FilterHashtable"





            //Add it to the pipeline:
            pipeline.Commands.Add(cmd);

            Collection<PSObject> output = pipeline.Invoke();

            List<string> msgList = new List<string>();
            int eventID = 0;
            foreach (PSObject psObject in output)
            {
                eventID = (int)psObject.Properties["Id"].Value;
                if (eventID == 4688)
                {
                    Console.WriteLine($"------------------------------------------");
                    DateTime timeStamp = (DateTime)psObject.Properties["TimeCreated"].Value;
                    string message = (string)psObject.Properties["Message"].Value;
                    Console.WriteLine($"Event ID: {eventID}, Time: {timeStamp}");
                    msgList.Add(message);
                }
            }//foreach outter

            Console.WriteLine($"Message: {msgList[0]}");
            Console.ReadLine();


        }//method
    }//class
}//namespace




