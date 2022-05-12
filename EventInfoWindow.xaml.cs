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
    /// Interaction logic for EventInfoWindow.xaml
    /// </summary>
    public partial class EventInfoWindow : Window
    {
        Event event_;
        SingletonObject singleton_;
        TwitterInterface twitter_;

        public EventInfoWindow()
        {
            InitializeComponent();
        }

        public EventInfoWindow(ref Event _event, ref SingletonObject singleton, ref TwitterInterface twitter)
        {
            InitializeComponent();
            event_ = _event;
            singleton_ = singleton;
            twitter_ = twitter;
            Init();
        }

        private void Init()
        {
            Longitude_lbl.Content = event_.position.Lng;
            Latitude_lbl.Content = event_.position.Lat;

            string path = System.IO.Directory.GetCurrentDirectory();
            path += "//Assets//Images//";

            if (event_.event_type == Event.EventType.PHOTO_EVENT)
            {
                try
                {
                    path += event_.EventInfo;
                    Image_box.Source = new BitmapImage(new System.Uri(path));
                    Content_textbox.Visibility = Visibility.Hidden;
                    Twitter_text_lbl.Visibility = Visibility.Hidden;
                    Mini_image_box.Visibility = Visibility.Hidden;
                    Photo_event_lbl.Content = $"Photo: {event_.EventInfo}";
                }
                catch (System.IO.IOException)
                {
                    try
                    {
                        path = System.IO.Directory.GetCurrentDirectory();
                        path += "//Assets//Images//DefaultPin.png";
                        Content_textbox.Visibility = Visibility.Hidden;
                        Twitter_text_lbl.Visibility = Visibility.Hidden;
                        Mini_image_box.Visibility = Visibility.Hidden;
                        Image_box.Source = new BitmapImage(new System.Uri(path));
                        Photo_err_event_lbl.Content = $"{event_.EventInfo}";
                    }
                    catch (System.IO.IOException) { }
                } 
            }else if(event_.event_type == Event.EventType.TEXT_EVENT)
            {
                path += "Text.png";
                Mini_image_box.Source = new BitmapImage(new System.Uri(path));
                Content_textbox.Text = event_.EventInfo;
                Twitter_text_lbl.Content = "Text";
                
            }else if(event_.event_type == Event.EventType.TWITTER_EVENT)
            {
                try
                {
                    Twitter_text_lbl.Content = "Tweet";
                    Content_textbox.Text = twitter_.SearchTweetsFor(event_.EventInfo);
                    path += "Twitter.PNG";
                    Mini_image_box.Source = new BitmapImage(new System.Uri(path)); 
                }
                catch (System.IO.IOException) { }  
            }else if(event_.event_type == Event.EventType.VIDEO_EVENT)
            {
                try
                {
                    path += "Video.PNG";
                    Image_box.Source = new BitmapImage(new System.Uri(path));
                    Content_textbox.Visibility = Visibility.Hidden;
                    Twitter_text_lbl.Visibility = Visibility.Hidden;
                    Mini_image_box.Visibility = Visibility.Hidden;
                    Photo_event_lbl.Content = $"{event_.EventInfo}";
                }
                catch (System.IO.IOException) { }
            }
        }

        private void Remove_Event_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this event?", "WARNING", MessageBoxButton.OKCancel);
            switch(result)
            {
                case MessageBoxResult.OK:
                    singleton_.RemoveInstance(event_.object_ID);
                    this.Close();
                    break;
                case MessageBoxResult.Cancel:
                    break;
                case MessageBoxResult.None:
                    break;
            }
            
        }
    }
}
