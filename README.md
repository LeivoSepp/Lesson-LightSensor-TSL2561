# Adafruit Light Sensor TSL2561
This project is an example on how to use the Adafruit light sensor TSL2561 in Raspberry PI and Windows 10 IoT Core.

## What is this sensor?
https://www.adafruit.com/products/439

The TSL2561 luminosity sensor is an advanced digital light sensor, ideal for use in a wide range of light situations.
This sensor is precise, allowing for exact lux calculations and can be configured for 
different gain/timing ranges to detect light ranges from up to 0.1 - 40,000+ Lux on the fly. 

![image](https://cloud.githubusercontent.com/assets/13704023/22796235/531f5dec-ef02-11e6-929a-beb24afb5d74.png)

## How to connect this sensor into Raspberry PI?
To connect this sensor to Raspberry PI you need 4 wires. Two of the wires used for voltage Vin (+3V from Raspberry) and ground GND and remaining two are used for data. 
As this is digital sensor, it uses I2C protocol to communicate with the Raspberry. For I2C we need two wires, Data (SDA) and Clock (SCL).
Connect sensor SDA and SCL pins accordingly to Raspberry SDA and SCL pins. 

## How do I write the code?
I made it very simple for you. You just need to add NuGet package RobootikaCOM.TSL2561 to your project and you are almost done :)

Right-click in your project name and then "Manage NuGet packages"
![image](https://cloud.githubusercontent.com/assets/13704023/22802711/964f83d6-ef1a-11e6-9e7e-398257c2eda0.png)

Open "Browse" tab, write "robootika" in search-window and then install RobootikaCOM.TSL2561 package into your project.
![image](https://cloud.githubusercontent.com/assets/13704023/22802827/0ba11ed8-ef1b-11e6-8f46-64a8bf8fd432.png)

After adding this NuGet package, you just need to write 2 lines of code.

1. Create an object for sensor: 
````C#
        private TSL2561 TSL2561Sensor = new TSL2561();
````

2. Write a while-loop, to read data from the sensor every 1 sec.
````C#
            while (true)
            {
                double currentLux = TSL2561Sensor.GetLux();
                Task.Delay(1000).Wait();
            }
````

Final code looks like this. 
If you run it, you do not see anything, because it just reads the data, but it doesnt show it anywhere.
You need to integrate this project with my other example, where I teach how to send this data into Azure.

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
This sensor supports three different timing and two gain options. 

1. Timing: this means how many samples the sensor will take before calculating the light level.
Changing the integration time gives you a longer time over which to sense light. Longer timelines are slower, but are good in very low light situtations!
This sensor has three parameters for timing. 
   1. 13ms the shortest measure time, use in bright light
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

Tu use timing and gain, you need to write one additional line of code. First parameter for method SetTiming is gain (true/false) and second parameter is timing. 
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
## Advanced sensor tuning: change I2C address
I2C address is used to communicate with the sensor. Many sensors have I2C address hardcoded, but this sensor supports three different I2C addresses.
By defult this TSL2561 sensor uses I2C address 0x39. You can change the address by connecting the sensor pin named Addr to +3v or to ground. 

1. How to set I2C address to 0x39
   1. Leave the sensor pin addr open
   2. Create a new sensor object without any parameters
````C#
        private TSL2561 TSL2561Sensor = new TSL2561();
````
2. How to set I2C address to 0x29
   1. Connect the sensor pin addr to ground
   2. Use parameter TSL2561.I2C_ADDR_0_0x29 when creating a new sensor object
````C#
        private TSL2561 TSL2561Sensor = new TSL2561(TSL2561.I2C_ADDR_0_0x29);
````
3. How to set I2C address to 0x49
   1. Connect the sensor pin addr to voltage +3V
   2. Use parameter TSL2561.I2C_ADDR_0_0x49 when creating a new sensor object
````C#
        private TSL2561 TSL2561Sensor = new TSL2561(TSL2561.I2C_ADDR_1_0x49);
````

## Sensor technical Details

* Precisely Measures Illuminance in Diverse Lighting Conditions 
* Temperature range: -30 to 80 *C
* Dynamic range (Lux): 0.1 to 40,000 Lux
* Interface: I2C
* This board/chip uses I2C 7-bit addresses 0x39, 0x29, 0x49, selectable with jumpers

## Indoor and outdoor lighting conditions

Illuminance | Example
--- | --- 
0.002 lux | Moonless clear night sky
0.2 lux | Design minimum for emergency lighting (AS2293).
0.27 - 1 lux | Full moon on a clear night
3.4 lux | Dark limit of civil twilight under a clear sky
50 lux | Family living room
80 lux | Hallway/toilet
100 lux | Very dark overcast day
300 - 500 lux | Sunrise or sunset on a clear day. Well-lit office area.
1,000 lux | Overcast day; typical TV studio lighting
10,000 - 25,000 lux | Full daylight (not direct sun)
32,000 - 130,000 lux | Direct sunlight