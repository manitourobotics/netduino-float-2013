using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO.Ports;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;


namespace NetduinoRobot
{
    public class Program
    {
        public static void Main()
        {
            uint watchdogTimer = 200;

            PWM umbrella = new PWM(Pins.GPIO_PIN_D10); //Right controller
            PWM dispenser = new PWM(Pins.GPIO_PIN_D5); // Left Controller


            Socket receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
	        //OutputPort headlights = new OutputPort(Pins.GPIO_PIN_D4, false);
            receiveSocket.Bind(new IPEndPoint(IPAddress.Any, 4444));
            byte[] rxData = new byte[10]; // Incoming data buffer
            double calibrate = 0;

            while (true) /* Main program loop */
            {
                /* Try to receive new data - spend 100uS waiting */
                if (receiveSocket.Poll(100, SelectMode.SelectRead))
                {
                    int rxCount = receiveSocket.Receive(rxData);
                    watchdogTimer = 0;
                }

                if (watchdogTimer < 200)   // Only enable the robot if data was received recently
                {
                    // 900 (full rev) to 2100 (full fwd), 1500 is neutral
                    umbrella.SetPulse(20000, map((uint)rxData[2], 0, 255, 900, 2100)); // Right controller
                     
                    calibrate += (rxData[0] - 127.5) * .001; // Add the value of the stick to the current speed 
                    // Mediate added speed to negative if it's below center line(on ipgamepad). Make the added speed very little because the mount of UDP packets is large.
                    if (calibrate < 0)
                    {
                        calibrate = 0;
                    }
                    else if (calibrate > 255)
                    {
                        calibrate = 255;
                    }
                    dispenser.SetPulse(20000, map((uint)calibrate, 0, 255, 1500, 2100)); // Right controller 1500-2100 -- only positive

                    

            
                    //umbrella.SetPulse(
                    //if ((uint)rxData[4] == 255)
                    //    headlights.Write(true);
                    //else
                    //    headlights.Write(false);
                    watchdogTimer++;
                }
                else
                {
                    // Disable the robot
                    //leftDrive.SetDutyCycle(0);
                    //rightDrive.SetDutyCycle(0);
		            //headlights.Write(false);
                }
            }
        }

        public static uint map(uint x, uint in_min, uint in_max, uint out_min, uint out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
    }
}