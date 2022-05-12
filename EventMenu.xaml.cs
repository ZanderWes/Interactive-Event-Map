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
using System.Windows.Shapes;

namespace WPFEventMap
{
    /// <summary>
    /// Interaction logic for EventMenu.xaml
    /// </summary>
    public partial class EventMenu : Window
    {
        SingletonObject singleton_;
        GMap.NET.PointLatLng position;
        TwitterInterface twitter_;

        public EventMenu()
        {
            InitializeComponent();
        }

        public EventMenu(ref SingletonObject singleton, GMap.NET.PointLatLng point, ref TwitterInterface twitter)
        {
            InitializeComponent();
            singleton_ = singleton;
            position = point;
            Longitude_lbl.Content = point.Lng;
            Latitude_lbl.Content = point.Lat;
            twitter_ = twitter;
        }


        private void Add_Event_btn_Click(object sender, RoutedEventArgs e)
        {
            if(Video_Event_checkbox.IsChecked is false && Text_Event_checkbox.IsChecked is false &&
                Twitter_event_checkbox.IsChecked is false && Photo_Event_checkbox.IsChecked is false)
            {
                MessageBox.Show("You need to select an event type!");
                return;
            }

            if(Event_content_txtbox.Text == "")
            {
                MessageBox.Show("You need to add some info or a description");
                return;
            }

            Event temp_event = new Event(position);
            temp_event.EventInfo = Event_content_txtbox.Text;

            if (Video_Event_checkbox.IsChecked is true)
                temp_event.event_type = Event.EventType.VIDEO_EVENT;
            else if (Text_Event_checkbox.IsChecked is true)
                temp_event.event_type = Event.EventType.TEXT_EVENT;
            else if (Photo_Event_checkbox.IsChecked is true)
                temp_event.event_type = Event.EventType.PHOTO_EVENT;
            else if (Twitter_event_checkbox.IsChecked is true)
            {
                temp_event.event_type = Event.EventType.TWITTER_EVENT;
                twitter_.PostTextToTwitter(temp_event.EventInfo);
            }

            singleton_.Instanciate(SingletonObject.SingletonType.EVENT_OBJECT, temp_event);
            DialogResult = true;
        }

        private void Video_Event_checkbox_Checked(object sender, RoutedEventArgs e)
        {
            Text_Event_checkbox.IsChecked = false;
            Photo_Event_checkbox.IsChecked = false;
            Twitter_event_checkbox.IsChecked = false;

            Content_label.Content = "";
        }

        private void Text_Event_checkbox_Checked(object sender, RoutedEventArgs e)
        {
            Video_Event_checkbox.IsChecked = false;
            Photo_Event_checkbox.IsChecked = false;
            Twitter_event_checkbox.IsChecked = false;

            Content_label.Content = "Whats on your mind?";
        }

        private void Photo_Event_checkbox_Checked(object sender, RoutedEventArgs e)
        {
            Text_Event_checkbox.IsChecked = false;
            Video_Event_checkbox.IsChecked = false;
            Twitter_event_checkbox.IsChecked = false;

            Content_label.Content = "Enter the photo name e.g. Photo.png";
        }

        private void Twitter_event_checkbox_Checked(object sender, RoutedEventArgs e)
        {
            Text_Event_checkbox.IsChecked = false;
            Photo_Event_checkbox.IsChecked = false;
            Video_Event_checkbox.IsChecked = false;

            Content_label.Content = "What would you like to Tweet?";
        }
    }
}
