using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace WPFEventMap
{
    interface ITwitterlink
    {
        void PostTextToTwitter(string text_content);
        Task<Tweetinvi.Models.ITweet[]> GetTweetsFromTwitter();
        string SearchTweetsFor(string text);
    }

    public class TwitterInterface : ITwitterlink
    {
        // establishing the connection to twitter API 
        private static string CONSUMER_KEY = "";
        private static string CONSUMER_SECRET = "";
        private static string ACCESS_TOKEN = "";
        private static string ACCESS_TOKEN_SECRET = "";

        TwitterClient userClient = new TwitterClient(CONSUMER_KEY, CONSUMER_SECRET, ACCESS_TOKEN, ACCESS_TOKEN_SECRET);

        public TwitterInterface()
        {

        }

        public void PostTextToTwitter(string text_content)
        {
            try
            {
                var tweet = userClient.Tweets.PublishTweetAsync(text_content);

            }
            catch
            {
                System.Windows.MessageBox.Show("error connecting to twitter");
            }
        }

        public string SearchTweetsFor(string text)
        {
            try
            {
                var tweet_stream = GetTweetsFromTwitter();

                for (int i = 0; i < tweet_stream.Result.Length; i++)
                {
                    if (tweet_stream.Result[i].ToString().ToLower() == text.ToLower())
                    {
                        return tweet_stream.Result[i].ToString();
                    }
                }
            }
            catch 
            {
                System.Windows.MessageBox.Show("error connecting to twitter");
            }
            

            return "couldnt find Tweet";
        }

        public Task<Tweetinvi.Models.ITweet[]> GetTweetsFromTwitter()
        {
            try
            {
                return userClient.Timelines.GetHomeTimelineAsync();
            }
            catch
            {
                System.Windows.MessageBox.Show("error connecting to twitter");
                return null;
            }
        }
    }
}
