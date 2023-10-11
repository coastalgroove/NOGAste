using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOGAste
{
    public class DapperEventsRepository : IEventsRepository
    {
        private readonly IDbConnection _conn;

        public DapperEventsRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        //Implementation of method "GetAllEvents" from
        //Interface IEventsRepository
        public IEnumerable<Events> GetAllEvents()
        {
            return _conn.Query<Events>("SELECT * FROM events");
        }


        public IEnumerable<Events> GetMaliciousProgram()
        {
            return _conn.Query<Events>("SELECT * FROM securityLogs.events     " +
                                       " WHERE CommandRun LIKE '%powershell%' " +
                                       " OR ProcessInfo LIKE '%powershell%'   " +
                                       " OR ObjName LIKE '%powershell%'       " +
                                       " OR AppPath LIKE '%powershell%';      "
                                       );
        }


        public IEnumerable<Events> GetEvent()
        {
            return _conn.Query<Events>("SELECT KeyID,EventID,UserID,ThreatEval,ActionReqd " +
                                       " FROM securityLogs.events  WHERE KeyID = 3 " 
                                       );
        }








        //Implementation of method "InsertEvents" from
        //Interface IEventsRepository
        public void InsertEvents(Events events)
        {
            //Parameterized Statement, Anonymouns Type
            //(place holders) <==> Temporary Object to
            //safely pass in parameters
            _conn.Execute("INSERT INTO Events" +
                          " (EventID, TimeCreated,  EventMsg, LogonType, ElevToken, " +
                          " ImpersonateLvl, LogonFail, KnownUser, FailReason, MachineName, " +
                          " UserID, ProgramRun, CommandRun, ProcessInfo, ObjName, AppPath, " +
                          " LogLvl, Status, SubStatus, ReasonEvnt, ThreatEval, ActionReqd)  " +
                          " VALUES " +
                          " (@EventID, @TimeCreated, @EVentMsg, @LogonType, @ElevToken, " +
                          " @ImpersonateLvl, @LogonFail, @KnownUser, @FailReason, @MachineName, " +
                          " @UserID, @ProgramRun, @CommandRun, @ProcessInfo, @ObjName, @AppPath, " +
                          " @LogLvl, @Status, @SubStatus, @ReasonEvnt, @ThreatEval, @ActionReqd)",
                          new {
                              eventID        = events.EventID,
                              timeCreated    = events.TimeCreated,
                              eventMsg       = events.EventMsg,
                              logonType      = events.LogonType,
                              elevToken      = events.ElevToken,
                              impersonateLvl = events.ImpersonateLvl,
                              logonFail      = events.LogonFail,
                              knownUser      = events.KnownUser,
                              failReason     = events.FailReason,
                              machineName    = events.MachineName,
                              //afterHours     = events.AfterHours,
                              //logonSuccess   = events.LogonSuccess,                              machineName    = events.MachineName,
                              userID = events.UserID,
                              programRun     = events.ProgramRun,
                              commandRun     = events.CommandRun,
                              processInfo    = events.ProcessInfo,
                              objName        = events.ObjName,
                              appPath        = events.AppPath,
                              logLvl         = events.LogLvl,
                              status         = events.Status,
                              subStatus      = events.SubStatus,
                              reasonEvnt     = events.ReasonEvnt,
                              threatEval     = events.ThreatEval,
                              actionReqd     = events.ActionReqd,
                          }); ;
             }



    }//Class
}//Namespace
