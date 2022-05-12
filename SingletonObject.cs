using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//SingletonObject my_singleton = new SingletonObject();

//if (my_singleton.Instanciate(SingletonObject.SingletonType.EVENT))
//{
//    my_singleton.TraverseSingletonOutputID();
//    System.Console.WriteLine("no errors instanciating");
//}
//else { System.Console.WriteLine("error instanciating"); }

namespace WPFEventMap
{
    public class SingletonObject
    {
        public int object_ID { get; set; }
        private int event_id_iterator;
        private int carer_id_iterator;

        Dictionary<int, Event> children_event_instances;
        Dictionary<int, Collaborator> children_collaborator_instances;

        public SingletonObject() 
        { 
            
        }

        public enum SingletonType
        {
            EVENT,
            EVENT_OBJECT,
            COLLABORATOR
        }

        public bool Instanciate(SingletonType type, GMap.NET.PointLatLng point)
        {

            switch (type) 
            {
                /*********************************************
                * instancing a new event object without object
                **********************************************/
                case SingletonType.EVENT:

                    if (children_event_instances == null)
                    {
                        children_event_instances = new Dictionary<int, Event>();
                        event_id_iterator = 1;
                    }

                    Event instance = new Event(point);
                    instance.object_ID = event_id_iterator;

                    children_event_instances.Add(event_id_iterator, instance);
                    event_id_iterator++;

                    return true;

                case SingletonType.EVENT_OBJECT:

                    System.Console.WriteLine("invalid function called you need to parse an event object");
                    return false;

                default:
                    return false;
            }
        }

        public bool Instanciate(SingletonType type, Event event_obj)
        {
            switch (type)
            {
                case SingletonType.EVENT:

                    System.Console.WriteLine("invalid function called you need to parse a GMap.NET.PointLatLng point");
                    return false;

                /*********************************************
                * instancing a new event object via an object. 
                **********************************************/
                case SingletonType.EVENT_OBJECT:

                    if (children_event_instances == null)
                    {
                        children_event_instances = new Dictionary<int, Event>();
                        event_id_iterator = 1;
                    }

                    event_obj.object_ID = event_id_iterator;
                    children_event_instances.Add(event_id_iterator, event_obj);
                    event_id_iterator++;
                    System.Console.WriteLine("instanciated");
                    return true;

                default:
                    return false;
            }
        }

        public bool Instanciate(SingletonType type, Collaborator carer)
        {
            switch (type)
            {
                case SingletonType.COLLABORATOR:

                    if(children_collaborator_instances == null)
                    {
                        children_collaborator_instances = new Dictionary<int, Collaborator>();
                        carer_id_iterator = 1;
                    }

                    carer.object_ID = carer_id_iterator;
                    children_collaborator_instances.Add(carer_id_iterator, carer);
                    carer_id_iterator++;
                    return true;
                default:
                    return false;
            }
        }

        // checks if instance exists
        public bool InstanceExists(int instanceID)
        {
            return children_event_instances.ContainsKey(instanceID);
        }

        public Event FindEventInstanceFromPosition(GMap.NET.PointLatLng point)
        {
            foreach(var events in children_event_instances)
            {
                if (events.Value.position == point)
                    return events.Value;
            }
            return new Event();
        }

        public GMap.NET.PointLatLng FindInstancePosition(int instanceID)
        {
             foreach(var event_ in children_event_instances)
            {
                if (event_.Value.object_ID == instanceID)
                    return event_.Value.position;
               // else { return null; }
            } 
             return  new GMap.NET.PointLatLng();
        }

        public void RemoveInstance(int instanceID)
        {
            if( children_event_instances.Remove(instanceID));
            {
                return;
            }
            System.Console.WriteLine("instanceID  not found");
        }

        public int getInstanceID()
        {
            return object_ID;
        }

        public void TraverseSingletonOutputID()
        {
            foreach (var singleton_instance in children_event_instances)
            {
                System.Console.WriteLine("instance ID: " + singleton_instance.Key);
            }
        }

        public List<int> getInstanceIDList()
        {
            List<int> temp_id_list = new List<int>();

            foreach (var singleton_instance in children_event_instances)
            {
                temp_id_list.Add(singleton_instance.Key);
            }
            return temp_id_list;
        }
        
        public List<Event> GetListOfEvents()
        {
            List<Event> temp_list = new List<Event>();

            foreach (var singleton_instance in children_event_instances)
            {
                temp_list.Add(singleton_instance.Value);
            }
            return temp_list;
        }

        public List<Collaborator> GetListOfCarers()
        {
            List<Collaborator> temp_list = new List<Collaborator>();

            foreach(var singleton_instance in children_collaborator_instances)
            {
                temp_list.Add(singleton_instance.Value);
            }
            return temp_list;
        }

        public Event GetSingletonEventReference(int id)
        {
            return children_event_instances[id];
        }

        public Collaborator GetSingletonCarerReference(int id)
        {
            return children_collaborator_instances[id];
        }


        //// this is not a good idea ever but just to check if singleton works properly
        //public void RemoveRandomInstance()
        //{
        //    List<int> temp_id = getInstanceIDList();

        //    Random rand = new Random();
        //    int rndm = rand.Next(0, temp_id.Count());

        //    RemoveInstance(temp_id[rndm]);
        //}




    }
}
