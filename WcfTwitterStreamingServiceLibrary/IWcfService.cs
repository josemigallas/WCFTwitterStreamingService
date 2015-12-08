using System.ServiceModel;

namespace WcfTwitterStreamingServiceLibrary
{
    [ServiceContract]
    public interface IDuplexCallbackContract
    {
        [OperationContract]
        void PrintMessage(string message);

        [OperationContract]
        void StreamStarted();

        [OperationContract]
        void StreamPaused();

        [OperationContract]
        void StreamResumed();

        [OperationContract]
        void StreamStopped();
    }

    [ServiceContract(CallbackContract = typeof (IDuplexCallbackContract))]
    public interface IWcfService
    {
        [OperationContract]
        void GetStreaming(string[] filters);

        [OperationContract]
        void PauseStreaming();

        [OperationContract]
        void ResumeStreaming();

        [OperationContract]
        void StopStreaming();
    }
}