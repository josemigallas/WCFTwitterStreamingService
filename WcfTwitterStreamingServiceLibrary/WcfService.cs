using System.ServiceModel;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Streaminvi;

namespace WcfTwitterStreamingServiceLibrary
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class WcfService : IWcfService
    {
        private const string ConsumerKey = "...";
        private const string ConsumerSecret = "...";
        private const string AccessToken = "...";
        private const string AccessTokenSecret = "...";

        private readonly ITwitterCredentials _creds;
        private IFilteredStream _filteredStream;
        private IDuplexCallbackContract _oc;

        public WcfService()
        {
            _creds = new TwitterCredentials(ConsumerKey, ConsumerSecret, AccessToken, AccessTokenSecret);
        }

        public void GetStreaming(string[] filters)
        {
            _filteredStream = Stream.CreateFilteredStream(_creds);
            _oc = OperationContext
                .Current
                .GetCallbackChannel<IDuplexCallbackContract>();

            filters
                .ForEach(filter => _filteredStream.AddTrack(filter));

            _filteredStream
                .MatchingTweetAndLocationReceived += (sender, args) => _oc.PrintMessage(args.Tweet.Text);

            _filteredStream
                .StreamStarted += (sender, args) => _oc.StreamStarted();

            _filteredStream
                .StreamPaused += (sender, args) => _oc.StreamPaused();

            _filteredStream
                .StreamResumed += (sender, args) => _oc.StreamResumed();

            _filteredStream
                .StreamStopped += (sender, args) => _oc.StreamStopped();

            _filteredStream.StartStreamMatchingAnyConditionAsync();
        }

        public void PauseStreaming()
        {
            _filteredStream.PauseStream();
        }

        public void ResumeStreaming()
        {
            _filteredStream.ResumeStream();
        }

        public void StopStreaming()
        {
            _filteredStream.StopStream();
        }
    }
}