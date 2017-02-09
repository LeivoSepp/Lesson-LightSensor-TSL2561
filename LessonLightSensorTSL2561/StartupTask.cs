using Windows.ApplicationModel.Background;
using System.Threading.Tasks;

namespace LessonLightSensorTSL2561
{
    public sealed class StartupTask : IBackgroundTask
    {
        //TSL2561 sensor
        private TSL2561 TSL2561Sensor = new TSL2561();

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            while (true)
            {
                double currentLux = TSL2561Sensor.GetLux();
                Task.Delay(1000).Wait();
            }
        }
    }
}
