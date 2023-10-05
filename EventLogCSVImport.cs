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

namespace NOGAste
{
    public class EventLogCSVImport
    {
        //Properties
        public string Message              { get; set; }
        public string EventID              { get; set; }
        public string Version              { get; set; }
        public string Qualifiers           { get; set; }
        public string Level                { get; set; }
        public string Task                 { get; set; }
        public string Opcode               { get; set; }
        public string Keywords             { get; set; }
        public string RecordID             { get; set; }
        public string ProviderName         { get; set; }


        public string ProviderID           { get; set; }
        public string LogName              { get; set; }
        public string ProcessID            { get; set; }
        public string ThreadID             { get; set; }
        public string MachineName          { get; set; }
        public string UserID               { get; set; }
        public string TimeCreated        { get; set; }
        public string ActivityID           { get; set; }
        public string RelatedActivity      { get; set; }
        public string ContainerLog         { get; set; }


        public string MatchedQueryIDs      { get; set; }
        public string Bookmark             { get; set; }
        public string LevelDisplayName     { get; set; }
        public string OpcodeDisplayName    { get; set; }
        public string TaskDisplayName      { get; set; }
        public string KeywordsDisplayNames { get; set; }
        public string Properties           { get; set; }



        //Constructor with parameters
        public EventLogCSVImport(
                               string message,
                               string eventID,
                               string version,
                               string qualifiers,
                               string level,
                               string task,
                               string opcode,
                               string keywords,
                               string recordID,
                               string providerName,

                               string providerID,
                               string logName,
                               string processID,
                               string threadID,
                               string machineName,
                               string userID,
                               string timeCreated,
                               string activityID,
                               string relatedActivity,
                               string containerLog,

                               string matchedQueryIDs,
                               string bookmark,
                               string levelDisplayName,
                               string opcodeDisplayName,
                               string taskDisplayName,
                               string keywordsDisplayNames,
                               string properties)

        { //Body of method EventLogCSVImport constructor, Initialize properites with the parameters

            Message               = message;
            EventID               = eventID;
            Version               = version;
            Qualifiers            = qualifiers;
            Level                 = level;
            Task                  = task;
            Opcode                = opcode;
            Keywords              = keywords;
            RecordID              = recordID;
            ProviderName          = providerName;
            ProviderID            = providerID;
            LogName               = logName;
            ProcessID             = processID;
            ThreadID              = threadID;
            MachineName           = machineName;
            UserID                = userID;
            TimeCreated           = timeCreated;
            ActivityID            = activityID;
            RelatedActivity       = relatedActivity;
            ContainerLog          = containerLog;
            MatchedQueryIDs       = matchedQueryIDs;
            Bookmark              = bookmark;
            LevelDisplayName      = levelDisplayName;
            OpcodeDisplayName     = opcodeDisplayName;
            TaskDisplayName       = taskDisplayName;
            KeywordsDisplayNames  = keywordsDisplayNames;
            Properties            = properties;
        }//End method Body of EventLogCSVImport Constructor

    }//Class EventLogCSVImport


}//namespace