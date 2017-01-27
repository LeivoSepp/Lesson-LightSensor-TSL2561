using System;
using System.Text;
using Windows.ApplicationModel.Background;
using Microsoft.Devices.Tpm;
using Microsoft.Azure.Devices.Client;
using System.Threading.Tasks;

namespace LessonLightSensorTSL2561
{
    public sealed class StartupTask : IBackgroundTask
    {
        //TSL2561 sensor
        private TSL2561 TSL2561Sensor;
        private Boolean Gain = false;
        private uint MS = 0;
        private double CurrentLux = 0;
        
        private void initializeSensor()
        {
            TSL2561Sensor = new TSL2561();
            MS = (uint)TSL2561Sensor.SetTiming(Gain, 2); //time constant
        }
        private void initDevice()
        {
            TpmDevice device = new TpmDevice(0);
            string hubUri = device.GetHostName();
            string deviceId = device.GetDeviceId();
            string sasToken = device.GetSASToken();
            _sendDeviceClient = DeviceClient.Create(hubUri, AuthenticationMethodFactory.CreateAuthenticationWithToken(deviceId, sasToken), TransportType.Amqp);
        }
        private DeviceClient _sendDeviceClient;
        private async void SendMessages(string strMessage)
        {
            string messageString = strMessage;
            var message = new Message(Encoding.ASCII.GetBytes(messageString));
            await _sendDeviceClient.SendEventAsync(message);
        }
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            initializeSensor();
            initDevice();
            while (true)
            {
                CurrentLux = TSL2561Sensor.GetLux(Gain, MS);
                String strLux = String.Format("{0:0.00}", CurrentLux);
                SendMessages(strLux);
                Task.Delay(1000).Wait();
            }
        }
    }
}
