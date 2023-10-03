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

        public IEnumerable<Events> GetEvents()
        {
            return _conn.Query<Events>("SELECT * FROM Events");
        }

        public void InsertEvents(Events events)
        {
            _conn.Execute("INSERT INTO Events" +
                          " (EventID, MachineName, UserName)  " +
                          " VALUES " +
                          " (@EventID, @MachineName, @UserName)",
                          new {
                              eventID      = events.EventID,
                              machineName  = events.MachineName,
                              userName     = events.UserName,
                              });
            //Parameterized Statement, Anonymouns Type (place holders) <==> Temporary Object to safely pass in parameters
        }

 

    





    }//Class
}//Namespace
