using System;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Extensions;

namespace TwitterStreamingConsoleApplication
{
    public class Program
    {
        private const string ConsumerKey = "...";
        private const string ConsumerSecret = "...";
        private const string AccessToken = "...";
        private const string AccessTokenSecret = "...";

        public static void Main(string[] filters)
        {
            var creds = new TwitterCredentials(ConsumerKey, ConsumerSecret, AccessToken, AccessTokenSecret);
            var filteredStream = Stream.CreateFilteredStream(creds);

            filters
                .ForEach(filter => filteredStream.AddTrack(filter));

            filteredStream
                .MatchingTweetAndLocationReceived += (sender, args) => { Console.WriteLine(args.Tweet.Text); };

            filteredStream
                .StartStreamMatchingAnyCondition();
        }
    }
}