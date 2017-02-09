# Adafruit Light Sensor TSL2561
This project as an example, how to use Adafruit light sensor TSL2561 in Raspberry PI and Windows 10 IoT Core.

## What is this sensor?
https://www.adafruit.com/products/439

The TSL2561 luminosity sensor is an advanced digital light sensor, ideal for use in a wide range of light situations. 
This sensor is precise, allowing for exact lux calculations and can be configured for 
different gain/timing ranges to detect light ranges from up to 0.1 - 40,000+ Lux on the fly. 

![image](https://cloud.githubusercontent.com/assets/13704023/22796235/531f5dec-ef02-11e6-929a-beb24afb5d74.png)

## How to connect sensor into Raspberry PI?
To connect this sensor to Raspberry PI you need 4 wires. Two of the wires used for voltage (+3V from Raspberry) and ground and remaining two are used for data. 
As this is digital sensor, it uses I2C protocol to communicate with Raspberry. For I2C we need just two wires, Data (SDA) and Clock (SCL).
Please connect sensor SDA and SCL pins accordingly to Raspberry SDA and SCL pins. 

## How do I write code?
I made it very simple for you. You just need to add NuGet package RobootikaCOM.TSL2561 to your project and the you are almost done :)

Right-click in your project name and then "Manage NuGet packages"
![image](https://cloud.githubusercontent.com/assets/13704023/22802711/964f83d6-ef1a-11e6-9e7e-398257c2eda0.png)

Open "Browse" tab, write "robootika" in search-window and then install RobootikaCOM.TSL2561 package into your project.
![image](https://cloud.githubusercontent.com/assets/13704023/22802827/0ba11ed8-ef1b-11e6-8f46-64a8bf8fd432.png)

After adding this NuGet package, you just need to write 2 lines of code.

1. Create an object for sensor: 
````C#
        private TSL2561 TSL2561Sensor = new TSL2561();
````

2. Write a while-loop, to read data from the sensor in every 1 sec.
````C#
            while (true)
            {
                double currentLux = TSL2561Sensor.GetLux();
                Task.Delay(1000).Wait();
            }
````

Final code look like this. 
If you run it, you do not see anything, because it just reads the data, but it doesnt show it anywhere.
You need to integrate this project with my other example, where I am teaching how to send this data into Azure.

````C#
using Windows.ApplicationModel.Background;
using System.Threading.Tasks;

namespace LessonLightSensorTSL2561
{
    public sealed class StartupTask : IBackgroundTask
    {
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
````

## Advanced sensor tuning: timing and gain
This sensor support three different timing and two gain options. 

1. Timing: this mean how many samples sensor will take before calculating light. 
Changing the integration time gives you a longer time over which to sense light. Longer timelines are slower, but are good in very low light situtations!
This sensor has three parameters for timing. 
   1. 13ms shortest measure time, use in bright light
   2. 101ms average measure time, use in medium light
   3. 402ms long measure time, use in dim light
````C#
TSL2561.INTEGRATIONTIME_13MS
TSL2561.INTEGRATIONTIME_101MS
TSL2561.INTEGRATIONTIME_402MS
````
2. Gain: You can change the gain on the fly, to adapt to brighter/dimmer light situations. 
   1. No gain (false): use in bright light
   2. 16x Gain (true): use in dim light

Tu use timing and gain, you need to write one additional ine of code. First parameter for method SetTiming is gain (true/false) and second parameter is timing. 
For example, setting gain:true and timing maximum (402ms), this is the scenario for the most darker situation.
````C#
       public void Run(IBackgroundTaskInstance taskInstance)
        {
            TSL2561Sensor.SetTiming(true, TSL2561.INTEGRATIONTIME_402MS);
            while (true)
            {
                double currentLux = TSL2561Sensor.GetLux();
                Task.Delay(1000).Wait();
            }
        }
````

### Sensor technical Details

* Precisely Measures Illuminance in Diverse Lighting Conditions 
* Temperature range: -30 to 80 *C
* Dynamic range (Lux): 0.1 to 40,000 Lux
* Interface: I2C
* This board/chip uses I2C 7-bit addresses 0x39, 0x29, 0x49, selectable with jumpers
