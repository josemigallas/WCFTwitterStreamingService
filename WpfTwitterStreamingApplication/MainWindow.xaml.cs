using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfTwitterStreamingApplication.WcfStreamingServiceReference;

namespace WpfTwitterStreamingApplication
{
    public partial class MainWindow
    {
        private static TextBlock _tweetTextBlock;
        private static Button _startStreamingButton;
        private static Button _stopStreamingButton;

        private static StreamState _state;

        private InstanceContext _callbackContext;
        private WcfServiceClient _client;

        public MainWindow()
        {
            InitSession();
            InitializeComponent();

            _tweetTextBlock = TweetTextBlock;
            _startStreamingButton = StartStreamingButton;
            _stopStreamingButton = StopStreamingButton;
            _state = StreamState.Stopped;
        }

        private void InitSession()
        {
            _callbackContext = new InstanceContext(new WcfServiceCallback());
            _client = new WcfServiceClient(_callbackContext, "NetHttpBinding_IWcfService");
        }

        private async void StartStreamingButton_Click(object sender, RoutedEventArgs e)
        {
            switch (_state)
            {
                case StreamState.Stopped:
                    var filters = FiltersTextBox.Text.Split(' ');
                    await _client.GetStreamingAsync(filters);
                    break;

                case StreamState.Resumed:
                    await _client.PauseStreamingAsync();
                    break;

                case StreamState.Paused:
                    await _client.ResumeStreamingAsync();
                    break;
            }
        }

        private async void StopStreamingButton_Click(object sender, RoutedEventArgs e)
        {
            await _client.StopStreamingAsync();
        }

        private enum StreamState
        {
            Resumed,
            Paused,
            Stopped
        }

        public class WcfServiceCallback : IWcfServiceCallback
        {
            public void PrintMessage(string message)
            {
                _tweetTextBlock.Text += message;
            }

            public void StreamStarted()
            {
                _startStreamingButton.Content = "Pause";
                _stopStreamingButton.Visibility = Visibility.Visible;
                _state = StreamState.Resumed;
            }

            public void StreamPaused()
            {
                _startStreamingButton.Content = "Resume";
                _state = StreamState.Paused;
            }

            public void StreamResumed()
            {
                _startStreamingButton.Content = "Pause";
                _state = StreamState.Resumed;
            }

            public void StreamStopped()
            {
                _startStreamingButton.Content = "Start";
                _stopStreamingButton.Visibility = Visibility.Hidden;
                _state = StreamState.Stopped;
            }

            public Task PrintMessageAsync(string message)
            {
                return Task.Factory.StartNew
                    (
                        () => PrintMessage(message)
                    );
            }

            public Task StreamStartedAsynx()
            {
                return Task.Factory.StartNew
                    (
                        StreamStarted
                    );
            }

            public Task StreamPausedAsync()
            {
                return Task.Factory.StartNew
                    (
                        StreamPaused
                    );
            }

            public Task StreamResumedAsync()
            {
                return Task.Factory.StartNew
                    (
                        StreamResumed
                    );
            }

            public Task StreamStoppedAsync()
            {
                return Task.Factory.StartNew
                    (
                        StreamStopped
                    );
            }
        }
    }
}