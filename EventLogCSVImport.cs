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
using System.ServiceModel;

namespace NOGAste
{
    public class EventLogCSVImport
    {
        //Properties
        //THESE PROPERTIES ARE STRICTLY FOR DB WRITING
        //public string Message { get; set; } 
        public string EventID { get; set; }
        public string TimeCreated { get; set; }
        public string EventMsg { get; set; }
        public string LogonType { get; set; }
        public string ElevToken { get; set; }
        public string ImpersonateLvl { get; set; }
        public string LogonFail { get; set; }
        public string FailInfo { get; set; }
        public string FailReason { get; set; }
        //public string   AfterHours     {  get; set; }

        //public string   LogonSuccess   {  get; set; }
        public string MachineName { get; set; }
        public string UserID { get; set; }
        public string ProgramRun { get; set; }
        public string CommandRun { get; set; }
        public string FileAccess { get; set; }
        public string LogLvl { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public string Reason { get; set; }

        //public string   ThreatEval     {  get; set; }
        //public string   ActionReqd     {  get; set; }


        //Constructor with parameters
        public EventLogCSVImport(
               string eventID,
               string timeCreated,
               string eventMsg,
               string logonType,
               string elevToken,
               string impersonateLvl,
               string logonFail,
               string failInfo,
               string failReason,
               string machineName,
               string userID,
               string programRun,
               string commandRun,
               string fileAccess,
               string logLvl,
               string status,
               string subStatus,
               string reason)
        //string   afterHours,
        //string   logonSuccess,
        //string   threatEval,
        //string   actionReqd)


        { //Body of method EventLogCSVImport constructor, Initialize properites with the parameters
            EventID        = eventID;
            TimeCreated    = timeCreated;
            EventMsg       = eventMsg;
            LogonType      = logonType;
            ElevToken      = elevToken;
            ImpersonateLvl = impersonateLvl;
            LogonFail      = logonFail;
            FailInfo       = failInfo;
            FailReason     = failReason;
            //AfterHours                      = afterHours ;   
            //LogonSuccess                    = logonSuccess ;   
            MachineName    = machineName;
            UserID         = userID;
            ProgramRun     = programRun;
            CommandRun     = commandRun;
            FileAccess     = fileAccess;
            LogLvl         = logLvl;
            Status         = status;
            SubStatus      = subStatus;
            Reason         = reason;

        }//End method Body of EventLogCSVImport Constructor

    }//Class EventLogCSVImport


}//namespace