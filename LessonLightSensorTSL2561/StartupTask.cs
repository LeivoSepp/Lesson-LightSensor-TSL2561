using Windows.ApplicationModel.Background;
using System.Threading.Tasks;

namespace LessonLightSensorTSL2561
{
    public sealed class StartupTask : IBackgroundTask
    {
        private TSL2561 TSL2561Sensor = new TSL2561();

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            TSL2561Sensor.SetTiming(true, TSL2561.INTEGRATIONTIME_402MS);
            while (true)
            {
                double currentLux = TSL2561Sensor.GetLux();
                Task.Delay(1000).Wait();
            }
        }
    }
}
