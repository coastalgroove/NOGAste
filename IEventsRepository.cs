using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOGAste
{
 
    
    //An interface is an abstract class which contains only
    //abstract members (stubbed out methods)
    //this interface provides no functionality

    //Interfaces specify behavior
    //Interface is NOT a class, it is different from abstract class or base class
    //A class will IMPLEMENT an interface
    public interface IEventsRepository
    {

        //GetAllEvents and InsertEvents are "stubbed out methods"

        //This is a method declaration with a return type of
        //of IEnumerable<Events>  (Events. IEnumerable<T>)
        //When you want to retrieve a list of events from an
        //object that implements the IEventsRepository
        //interface, you would call this method.
        IEnumerable<Events> GetAllEvents();


        //Stubbed out method 
        //When you want to insert an event into an object
        //that implements the IEventsRepository interface,
        //you would call this method.
        public void InsertEvents(Events events);
    }//Interface

}//namespace



