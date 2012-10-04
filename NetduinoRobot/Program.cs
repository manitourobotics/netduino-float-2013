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
            uint watchdogTimer = 1000;

            PWM umbrella = new PWM(Pins.GPIO_PIN_D10); //Right controller

            Socket receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            receiveSocket.Bind(new IPEndPoint(IPAddress.Any, 4444));
            byte[] rxData = new byte[10]; // Incoming data buffer
            double raw_speed = 0;

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
                     
                    raw_speed += (rxData[0] - 127.5) * .001; // Add the value of the stick to the current speed 
                    // Mediate added speed to negative if it's below center line(on ipgamepad). Make the added speed very little because the mount of UDP packets is large.
                    // map function only accept input between 0-255
                    if (raw_speed < 0)
                    {
                        raw_speed = 0;
                    }
                    else if (raw_speed > 255)
                    {
                        raw_speed = 255;
                    }
                    // Stick maintains speed unless calibrate changes.
                    umbrella.SetPulse(20000, map((uint)raw_speed, 0, 255, 1500, 2100)); // Right controller 1500-2100 -- only positive

                    watchdogTimer++;
                }
                else
                {
                    // Disable the robot
                    umbrella.SetDutyCycle(0);
                }
            }
        }

        public static uint map(uint x, uint in_min, uint in_max, uint out_min, uint out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
    }
}