using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOGAste
{
    public interface IEventsRepository
    {
        IEnumerable<Events> GetEvents();

        public void InsertEvents(Events events);
    }



    

}



