using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace WPFEventMap
{
    public class XmlClassIO
    {
        

        public XmlClassIO()
        {
        }

        public bool LoadXmlEventFileSerialized(string path, ref SingletonObject singleton)
        {
            try
            {
                XmlSerializer xml_serializer = new XmlSerializer(typeof(List<Event>));
                Stream stream = File.OpenRead(path);
                List<Event> deserialized_list = (List<Event>)xml_serializer.Deserialize(stream);

                for (int i = 0; i < deserialized_list.Count; i++)
                {
                    singleton.Instanciate(SingletonObject.SingletonType.EVENT_OBJECT, deserialized_list[i]);
                }
                stream.Close();

                return true;
            }
            catch (System.IO.IOException) // refactoring exception catch
            {
                System.Console.WriteLine("error accessing xml data file! " +
                    "\nEnsure the file exists and is in the correct folder." +
                    $"Expected directory: {path}");
                return false;
            }
        }

        public bool LoadXmlCarerFileSerialized(string path, ref SingletonObject singleton)
        {
            try
            {
                XmlSerializer xml_serializer = new XmlSerializer(typeof(List<Collaborator>));
                Stream stream = File.OpenRead(path);
                List<Collaborator> deserialized_list = (List<Collaborator>)xml_serializer.Deserialize(stream);

                for (int i = 0; i < deserialized_list.Count; i++)
                {
                    singleton.Instanciate(SingletonObject.SingletonType.COLLABORATOR, deserialized_list[i]);
                }
                stream.Close();

                return true;
            }
            catch (System.IO.IOException) 
            {
                System.Console.WriteLine("error reading xml data file! " +
                    "\nEnsure the file exists and is in the correct folder." +
                    $"Expected directory: {path}");
                return false;
            }
        }

        /********************************************
         * Abstracting the write to XML into one call 
         * that handles all the xml output internally
         ********************************************/
        public bool WriteToXmlFileSerialized(string path, ref SingletonObject singleton)
        {
            List<Event> event_list = singleton.GetListOfEvents();
            List<Collaborator> carer_list = singleton.GetListOfCarers();

            bool events_data_saved_ = WriteToXMLEventFileSerialized(path, event_list);
            bool carer_data_saved_ = WriteToXmlCarerFileSerialized(path, carer_list);

            if (events_data_saved_ && carer_data_saved_)
                return true;
            else { return false; }
        }

        /************************************************************
         * Writing Event object data to xml file in serialized format
         ************************************************************/
        public bool WriteToXMLEventFileSerialized(string path, List<Event> event_list)
        {
            try
            {
                path = path + "//EventsDataFile.xml";
                Stream stream = File.OpenWrite(path);

                /************************************
                 * Flushing the previous file content
                 ************************************/
                stream.SetLength(0);
                stream.Flush();

                /**************************
                 * Writing new file content
                 **************************/
                XmlSerializer xml_serializer = new XmlSerializer(typeof(List<Event>));
                xml_serializer.Serialize(stream, event_list);
                stream.Close();

                return true;
            }
            catch (System.IO.IOException) // refactoring exception catch
            {
                System.Console.WriteLine("error writing data to file! " +
                    "\nEnsure the file exists and is in the correct folder." +
                    $"Expected directory: {path}");
                return false;
            }
        }
        public bool WriteToXmlCarerFileSerialized(string path, List<Collaborator> carer_list)
        {
            try
            {
                System.Console.WriteLine(path);

                path = path + "//CarerDataFile.xml";
                Stream stream = File.OpenWrite(path);

                /************************************
                 * Flushing the previous file content
                 ************************************/
                stream.SetLength(0);
                stream.Flush();

                /**************************
                 * Writing new file content
                 **************************/
                XmlSerializer xml_serializer = new XmlSerializer(typeof(List<Collaborator>));
                xml_serializer.Serialize(stream, carer_list);
                stream.Close();

                return true;
            }
            catch(System.IO.IOException) // refactoring exception catch
            {
                System.Console.WriteLine("error writing data to file! " +
                    "\nEnsure the file exists and is in the correct folder." +
                    $"Expected directory: {path}");
                return false;
            }
        }





/************************************************************************************************************************************************/
        // NOTE: these following functions are not currently used

        /********************************************
         * This only works for specific style of xml
         * UNUSED
         ********************************************/
        public bool LoadXmlEventFileUnSerialized(string path, ref SingletonObject singleton)
        {
            try
            {
                XmlDocument xml_document = new XmlDocument();
                xml_document.Load(path);
                //xml_document.w

                foreach (XmlNode node in xml_document.DocumentElement)
                {
                    //string nm = node.Attributes[0].Value;
                    string name = node["name"].InnerText;
                    double longitude = double.Parse(node["long"].InnerText);
                    double latitude = double.Parse(node["lat"].InnerText);
                    GMap.NET.PointLatLng point = new GMap.NET.PointLatLng(latitude, longitude);

                    Event temp_event = new Event(point);
                    temp_event.EventInfo = name;
                    singleton.Instanciate(SingletonObject.SingletonType.EVENT_OBJECT, temp_event);
                }

                return true;
            }
            catch (System.IO.FileNotFoundException) //System.IO.DirectoryNotFoundException)
            {

                System.Console.WriteLine("couldnt find file error");
                return false;
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                System.Console.WriteLine("couldnt find file path error");
                return false;
            }


        }

        /**********************
         * This doesnt work :(
         * UNUSED
         **********************/
        public bool WriteToXmlEventFileUnSerialized(string path, ref SingletonObject singleton)
        {
            try
            {
                XmlDocument xml_document = new XmlDocument();
                xml_document.Load(path);
                xml_document.PreserveWhitespace = true;

                List<Event> temp_list = singleton.GetListOfEvents();

                for (int i = 0; i < temp_list.Count; i++)
                {
                    XmlElement xml_element = xml_document.CreateElement("Event");//, temp_list[i].EventName);
                    XmlElement xml_elm_nm = xml_document.CreateElement("name");//, temp_list[i].EventName);
                    GMap.NET.PointLatLng point = temp_list[i].position;
                    XmlElement xml_elm_lat = xml_document.CreateElement("lat");//point.Lat.ToString());
                    XmlElement xml_elm_lng = xml_document.CreateElement("long");// point.Lng.ToString());

                    XmlNode xml_node = xml_element;// + xml_elm2 + xml_elm3;
                                                   // xml_node.AppendChild();
                    xml_node.InsertAfter(xml_elm_lng, xml_element);
                    xml_node.InsertAfter(xml_elm_lat, xml_element);
                    //xml_node.LastChild.AppendChild(xml_elm_lng);
                    // xml_node.LastChild.AppendChild(xml_elm_lat);

                    xml_document.DocumentElement.AppendChild(xml_node);
                }
                xml_document.Save(path);
                //foreach (XmlNode node in xml_document.DocumentElement)
                //{
                //    //string nm = node.Attributes[0].Value;
                //    string name = node["name"].InnerText;
                //    double longitude = double.Parse(node["long"].InnerText);
                //    double latitude = double.Parse(node["lat"].InnerText);
                //    GMap.NET.PointLatLng point = new GMap.NET.PointLatLng(latitude, longitude);

                //    Event temp_event = new Event(point);
                //    temp_event.EventName = name;
                //    singleton.Instanciate(SingletonObject.SingletonType.EVENT_OBJECT, temp_event);
                //}

                return true;
            }
            catch (System.IO.FileNotFoundException) //System.IO.DirectoryNotFoundException)
            {

                System.Console.WriteLine("couldnt find file error");
                return false;
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                System.Console.WriteLine("couldnt find file path error");
                return false;
            }
        }
    }
}
