netduino-float-2013
===================

C# code for our netduino which controlled the Robotics Homecoming parade float for 2012-2013

####About

This code only uses the right stick on the IPGamePad to control a 12V motor. The motor is controlled from PWM port 6 on the netduino. This signal returns to the netduino through a GND(ground) port. The stick is moved to add or decrease speed on the motor controller with moving above the center line to increase speed and moving below to decrease. Instead of its intendend way, the stick does not decrease the speed to zero if there is no input, but instead maintains speed. This function is not to be confused with the motor shutting off if the there is no communication between IPGamePad and Netduino.

