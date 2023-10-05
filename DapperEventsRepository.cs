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
                          " (EventID, TimeCreated, MachineName, UserID)  " +
                          " VALUES " +
                          " (@EventID, @TimeCreated, @MachineName, @UserID)",
                          new {
                              eventID      = events.EventID,
                              timeCreated  = events.TimeCreated,
                              machineName  = events.MachineName,
                              userID       = events.UserID,
                              });
             }


    }//Class
}//Namespace
