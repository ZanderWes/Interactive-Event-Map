using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tweetinvi.Auth;

namespace WPFEventMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SingletonObject singleton_;
        XmlClassIO xml_data_ = new XmlClassIO();
        float icon_size = 32;
        TwitterInterface twitter = new TwitterInterface();

        public MainWindow()
        {
            InitializeComponent();

            singleton_ = new SingletonObject();
            Init();
        }

        private void Init()
        {
            InitializeMap(); // this is redundant but it does fix a mapload bug that occurs sometimes
            InitializeSingleton();   
        }

        private void InitializeSingleton()
        {
            //get directory path of event xml file 
            string path = System.IO.Directory.GetCurrentDirectory();
            path = path + "//EventsDataFile.xml";

            if (!xml_data_.LoadXmlEventFileSerialized(path, ref singleton_))
            {
                MessageBox.Show($"error loading events xml file {path} check that the file exists");
            }
            else
            {
                GenerateEventMarkers();
            }

            //Get new directory path of carer xml file
            path = System.IO.Directory.GetCurrentDirectory();
            path = path + "//CarerDataFile.xml";

            if(!xml_data_.LoadXmlCarerFileSerialized(path, ref singleton_))
            {
                MessageBox.Show($"error loading carer xml file {path} check that the file exists");
            }
        }

        private void LoadmapView(object sender, RoutedEventArgs e)
        {
            map.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            map.Position = new GMap.NET.PointLatLng(-31.927350217293082, 115.85575468350311);
            map.ShowCenter = false;
            map.MinZoom = 10;
            map.MaxZoom = 15;
            map.Zoom = 10;
        }

        private void InitializeMap()
        {
            map.Position = new GMap.NET.PointLatLng(-31.927350217293082, 115.85575468350311);
            map.ShowCenter = false;
            map.MinZoom = 10;
            map.MaxZoom = 15;
            map.Zoom = 10;
        }

        private void LeftMouseClickEvent(object sender, MouseEventArgs mouse)
        {
            // checks if the mouse is clicked on  an event or not 
            // and then calls the function according to the click input
            Point map_point = Mouse.GetPosition(map);
            if (!map.IsMouseDirectlyOver && map.IsMouseOver)
            {
                GmapOnMarkerClick(FindClosestMapPin(map_point));
            }
            else
            {
                var point = map.FromLocalToLatLng((int)map_point.X, (int)map_point.Y);
                LeftClickMenuResolver(point);
            }
        }


        /******************************************************************************
         * finds the closest map pin location relative to where the mouse click occured
         * This is used because GMaps doesnt have on marker click events for WPF
        *******************************************************************************/
        private GMap.NET.PointLatLng FindClosestMapPin(Point position)
        {
            GMap.NET.PointLatLng clicked_position = map.FromLocalToLatLng((int)position.X, (int)position.Y);
            GMap.NET.PointLatLng closest_marker = clicked_position; //just to keep the compiler happy 

            double distance = 100;
            // distance checking each marker position
            foreach (var pos in map.Markers)
            {
                GMap.NET.PointLatLng marker_point = pos.Position;
                double lat_delta = marker_point.Lat - clicked_position.Lat;
                double lng_delta = marker_point.Lng - clicked_position.Lng;

                double dist_check = Math.Sqrt(lat_delta * lat_delta + lng_delta * lng_delta);

                if (dist_check < distance)
                {
                    closest_marker = marker_point;
                    distance = dist_check;
                }
            }
            return closest_marker;
        }

        private void GmapOnMarkerClick(GMap.NET.PointLatLng point)
        {
            Event temp_copy = singleton_.FindEventInstanceFromPosition(point);

            /************************
             * Event click menu setup
             ************************/
            EventInfoWindow event_view = new EventInfoWindow(ref temp_copy, ref singleton_, ref twitter);
            event_view.ShowDialog();
            GenerateEventMarkers();
            event_view.Close();

        }

        private void LeftClickMenuResolver(GMap.NET.PointLatLng point)//, MouseEventArgs mouse)
        {
            /***************************
            * left click menu setup
            **************************/
            EventMenu menu_click = new EventMenu(ref this.singleton_ , point, ref twitter);
            menu_click.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            menu_click.ShowDialog();

            if(menu_click.DialogResult.HasValue && menu_click.DialogResult.Value)
            {
                GenerateEventMarkers();
            }

            menu_click.Close();
        }

        private void GenerateEventMarkers()
        {
            map.Markers.Clear();
            //Point ofset = new Point(-6, -6);
            foreach (var event_instance in singleton_.GetListOfEvents())
            {
                GMap.NET.WindowsPresentation.GMapMarker marker;
                string name = event_instance.EventInfo.ToLower();

                switch (event_instance.event_type)
                {
                    case Event.EventType.PHOTO_EVENT:
                        try
                        {
                            string path = System.IO.Directory.GetCurrentDirectory();
                            path = path + "//Assets//Images//" + event_instance.EventInfo;//"//Assets//Images//DefaultPin.PNG";

                            marker = new GMap.NET.WindowsPresentation.GMapMarker(event_instance.position);
                            marker.Shape = new Image
                            {
                                Width = icon_size,
                                Height = icon_size,
                                Source = new BitmapImage(new System.Uri(path))
                            };
                            //marker.Offset = ofset;
                            map.Markers.Add(marker);
                        }
                        catch (System.IO.IOException)
                        {
                            //MessageBox.Show($"{event_instance.EventInfo} directory or file not found! using default");
                            try
                            {
                                string path = System.IO.Directory.GetCurrentDirectory();
                                path = path + "//Assets//Images//DefaultPin.PNG";

                                marker = new GMap.NET.WindowsPresentation.GMapMarker(event_instance.position);
                                marker.Shape = new Image
                                {
                                    Width = icon_size,
                                    Height = icon_size,
                                    Source = new BitmapImage(new System.Uri(path))
                                };
                                map.Markers.Add(marker);
                            }
                            catch (System.IO.IOException)
                            {
                                MessageBox.Show("DefaultPin.png directory not found!");
                            }                            
                        }
                        break;
                    case Event.EventType.TEXT_EVENT:
                        try
                        {
                            string path = System.IO.Directory.GetCurrentDirectory();
                            path = path + "//Assets//Images//Text.PNG";

                            marker = new GMap.NET.WindowsPresentation.GMapMarker(event_instance.position);
                            marker.Shape = new Image
                            {
                                Width = icon_size,
                                Height = icon_size,
                                Source = new BitmapImage(new System.Uri(path))
                            };
                            map.Markers.Add(marker);
                        }
                        catch (System.IO.DirectoryNotFoundException)
                        {
                            MessageBox.Show("Text.png directory not found!");
                        }
                        catch (System.IO.FileNotFoundException)
                        {
                            MessageBox.Show("Text.png not found!");
                        }
                        break;
                    case Event.EventType.TWITTER_EVENT:
                        try
                        {
                            string path = System.IO.Directory.GetCurrentDirectory();
                            path = path + "//Assets//Images//Twitter.PNG";

                            marker = new GMap.NET.WindowsPresentation.GMapMarker(event_instance.position);
                            marker.Shape = new Image
                            {
                                Width = icon_size,
                                Height = icon_size,
                                Source = new BitmapImage(new System.Uri(path))
                            };
                            map.Markers.Add(marker);
                        }
                        catch (System.IO.DirectoryNotFoundException)
                        {
                            MessageBox.Show("Twitter.png directory not found!");
                        }
                        catch (System.IO.FileNotFoundException)
                        {
                            MessageBox.Show("Twitter.png not found!");
                        }
                        break;
                    case Event.EventType.VIDEO_EVENT:
                        try
                        {
                            string path = System.IO.Directory.GetCurrentDirectory();
                            path = path + "//Assets//Images//Video.PNG";

                            marker = new GMap.NET.WindowsPresentation.GMapMarker(event_instance.position);
                            marker.Shape = new Image
                            {
                                Width = icon_size,
                                Height = icon_size,
                                Source = new BitmapImage(new System.Uri(path))
                            };
                            map.Markers.Add(marker);
                        }
                        catch (System.IO.DirectoryNotFoundException)
                        {
                            MessageBox.Show("Video.png directory not found!");
                        }
                        catch (System.IO.FileNotFoundException)
                        {
                            MessageBox.Show("Video.png not found!");
                        }
                        break;

                    default:
                        try
                        {
                            string path = System.IO.Directory.GetCurrentDirectory();
                            path = path + "//Assets//Images//DefaultPin.png";

                            marker = new GMap.NET.WindowsPresentation.GMapMarker(event_instance.position);
                            marker.Shape = new Image
                            {
                                Width = icon_size * 1.2,
                                Height = icon_size * 1.2,
                                Source = new BitmapImage(new System.Uri(path))
                            };
                            map.Markers.Add(marker);
                        }
                        catch (System.IO.DirectoryNotFoundException)
                        {
                            MessageBox.Show("DefaultPin.png directory not found!");
                        }
                        catch (System.IO.FileNotFoundException)
                        {
                            MessageBox.Show("DefaultPin.png not found!");
                        }
                        break;
                }
            }
        }

        private void OnAppExit(object sender, EventArgs e)
        {
            string path = System.IO.Directory.GetCurrentDirectory();

            if (!xml_data_.WriteToXmlFileSerialized(path, ref singleton_))
            {
                MessageBox.Show($"error saving data to xml files in folder {path}");
            }
        }

        private void View_Carer_btn_Click(object sender, RoutedEventArgs e)
        {
            CollaboratorView carer_menu = new CollaboratorView(ref this.singleton_);
            carer_menu.ShowDialog();

            carer_menu.Close();
        }
    }
}
