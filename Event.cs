using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFEventMap
{
    /********************************************
    * Event object inherits from Singleton oject
    *********************************************/
    public class Event : SingletonObject
    {
        //setup for assignment 2
        public enum EventType
        {
            PHOTO_EVENT,
            TWITTER_EVENT,
            VIDEO_EVENT,
            TEXT_EVENT
        }

        public EventType event_type { get; set; }
        public GMap.NET.PointLatLng position { get; set; }
        public string EventInfo { get; set; }
        //public string EventItemDirectory { get; set; }

        public DateTime Date_and_Time { get; set; }

        public Person person { get; set; }

        public Event() : base()
        {
            //EventTypeVisual = "Twitter";
            //event_type = EventType.TWITTER_EVENT;
        }

        public Event(GMap.NET.PointLatLng point) : base()
        {
            position = point;
            Date_and_Time = DateTime.Now;
            
        }

        
        //setup for assignment 2
        public Event(EventType type_e, GMap.NET.PointLatLng point) :base()
        {
            event_type = type_e;
            position = point;
            Date_and_Time = DateTime.Now;
        }

        public void AddPersonToEvent(string name, string relationship)
        {
            person = new Person(name, relationship);
        }
    }
}
