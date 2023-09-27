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

            //You can add parameters:
            cmd.Parameters.Add("LogName", "application");
            //cmd.Parameters.Add("StartTime", "08:00");
            //cmd.Parameters.Add("EndTime", "09:00");

            //Add it to the pipeline:
            pipeline.Commands.Add(cmd);

            Collection<PSObject> output = pipeline.Invoke();
            foreach (PSObject psObject in output)
            {
                Console.WriteLine(output);
            }

        }//method
    }//class
}//namespace