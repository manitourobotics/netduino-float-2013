netduino-float-2013
===================

C# code for our netduino which controlled the Robotics Homecoming parade float for 2012-2013

####About

This code only uses the left stick on the IPGamePad to control a 12V motor. The motor is controlled from PWM port 6 on the netduino. This signal returns to the netduino through a GND(ground) port. This application uses [IPGamePad](https://github.com/ericbarch/ipgamepad/tree/78ee50c86390f447d4f0220fd9ca6049ed3b1804) as its controller on an android phone. The left stick on IPGamePad is moved to add or decrease speed on the motor controller with moving above the center line to increase speed and moving below to decrease. Instead of its intendend way, the stick does not decrease the speed to zero if there is no input, but instead maintains speed. This function is not to be confused with the motor shutting off after 1 second if the there is no communication between IPGamePad and Netduino(I.E. diconnecting the android device from the network).

