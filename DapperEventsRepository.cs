using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOGAste
{
    internal class DapperEventsRepository : IEventsRepository
    {
        private readonly IDbConnection _conn;

        public DapperEventsRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        //Implementation of method "GetEvents" from
        //Interface IEventsRepository
        public IEnumerable<Events> GetEvents()
        {
            return _conn.Query<Events>("SELECT * FROM Events");
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
                          " ImpersonateLvl, LogonFail, FailInfo, FailReason, MachineName, " +
                          " UserID, ProgramRun, CommandRun, ProcessInfo, ObjName, AppPath, " +
                          " LogLvl, Status, SubStatus, ReasonEvnt)  " +
                          " VALUES " +
                          " (@EventID, @TimeCreated, @EVentMsg, @LogonType, @ElevToken, " +
                          " @ImpersonateLvl, @LogonFail, @FailInfo, @FailReason, @MachineName, " +
                          " @UserID, @ProgramRun, @CommandRun, @ProcessInfo, @ObjName, @AppPath, " +
                          " @LogLvl, @Status, @SubStatus, @ReasonEvnt)",
                          new {
                              eventID        = events.EventID,
                              timeCreated    = events.TimeCreated,
                              eventMsg       = events.EventMsg,
                              logonType      = events.LogonType,
                              elevToken      = events.ElevToken,
                              impersonateLvl = events.ImpersonateLvl,
                              logonFail      = events.LogonFail,
                              failInfo       = events.FailInfo,
                              failReason     = events.FailReason,
                              machineName    = events.MachineName,
                              userID         = events.UserID,
                              programRun     = events.ProgramRun,
                              commandRun     = events.CommandRun,
                              processInfo    = events.ProcessInfo,
                              objName        = events.ObjName,
                              appPath        = events.AppPath,
                              logLvl         = events.LogLvl,
                              status         = events.Status,
                              subStatus      = events.SubStatus,
                              reasonEvnt     = events.ReasonEvnt,

                          }); ;
             }
        //afterHours     = events.AfterHours,
        //logonSuccess   = events.LogonSuccess,
        //threatEval     = events.ThreatEval,
        //actionReqd     = events.ActionReqd,

    }//Class
}//Namespace
