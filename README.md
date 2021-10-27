# Unity Raspberry Pi

I'm attempting training an ai agent in Unity's ml agents that can control a real world robotic car.

This build is based off of an Elegoo arduino kit - https://www.elegoo.com/products/elegoo-smart-robot-car-kit-v-3-0-plus
which is fairly cheap on Amazon.

I'm using a pi zero on the car, which doesn't have enough processing power to run inference, so it has to be controlled remotely. To do this 
I set up a simple web socket server on the pi which can be accessed with websockets plus in Unity c#.

Set the pi ip address to a static address - 
 https://howchoo.com/pi/configure-static-ip-address-raspberry-pi#:~:text=How%20to%20Configure%20a%20Static%20IP%20Address%20on,...%205%20Test%20the%20static%20IP%20address.%20

 Set the camera streaming server and GPIO control server to run on start with a chron job
 https://www.bc-robotics.com/tutorials/setting-cron-job-raspberry-pi/
 
