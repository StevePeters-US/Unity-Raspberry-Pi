import websockets
import asyncio
import RPi.GPIO as GPIO
import json
import atexit

# Using custom Elegoo motor controller. Defer to forward_back.ino for more details
#    Left motor truth table
#  ENA         IN1               IN2         Description  
#  LOW   Not Applicable    Not Applicable    Motor is off
#  HIGH        LOW               LOW         Motor is stopped (brakes)
#  HIGH        HIGH              LOW         Motor is on and turning forwards
#  HIGH        LOW               HIGH        Motor is on and turning backwards
#  HIGH        HIGH              HIGH        Motor is stopped (brakes)

#    Right motor truth table
#  ENB         IN3             IN4         Description  
#  LOW   Not Applicable   Not Applicable   Motor is off
#  HIGH        LOW             LOW         Motor is stopped (brakes)
#  HIGH        LOW             HIGH        Motor is on and turning forwards
#  HIGH        HIGH            LOW         Motor is on and turning backwards
#  HIGH        HIGH            HIGH        Motor is stopped (brakes)  

#    The direction of the car's movement
#  Left motor    Right motor     Description  
#  stop(off)     stop(off)       Car is stopped
#  forward       forward         Car is running forwards
#  forward       backward        Car is turning right
#  backward      forward         Car is turning left
#  backward      backward        Car is running backwards

#reference to - define the L298n IO pin
#define ENA 5
#define ENB 6
#define IN1 7
#define IN2 8
#define IN3 9
#define IN4 11

LEDPIN = 26

PORT = 7890

def init():
    GPIO.setmode(GPIO.BCM)

    # LED
    GPIO.setup(LEDPIN, GPIO.OUT)

    # Left Motor
    GPIO.setup(17, GPIO.OUT) #ENA
    GPIO.setup(27, GPIO.OUT) #IN1
    GPIO.setup(22, GPIO.OUT) #IN2

    #Right Motor
    GPIO.setup(23, GPIO.OUT) #ENB
    GPIO.setup(24, GPIO.OUT) #IN3
    GPIO.setup(25, GPIO.OUT) #IN4

    atexit.register(OnExit)

def OnExit():
    print("Exiting Python server")
    GPIO.cleanup()

def toggleLight(lightOn):
    if lightOn:
        GPIO.output(LEDPIN, GPIO.HIGH)
    else:
        GPIO.output(LEDPIN, GPIO.LOW)
#time.sleep(5)

# speed in range -1 to 1
def moveLeftMotor(Speed):
    if Speed > 0.1:
        GPIO.output(17, GPIO.HIGH)
        GPIO.output(27, GPIO.HIGH)
        GPIO.output(22, GPIO.LOW)

    elif Speed < -0.1:
        GPIO.output(17, GPIO.HIGH)
        GPIO.output(27, GPIO.LOW)
        GPIO.output(22, GPIO.HIGH)

    else:
        GPIO.output(17, GPIO.LOW)
        GPIO.output(27, GPIO.LOW)
        GPIO.output(22, GPIO.LOW)

def moveRightMotor(Speed):
    if Speed > 0.1:
        GPIO.output(23, GPIO.HIGH)
        GPIO.output(24, GPIO.LOW)
        GPIO.output(25, GPIO.HIGH)

    elif Speed < -0.1:
        GPIO.output(23, GPIO.HIGH)
        GPIO.output(24, GPIO.HIGH)
        GPIO.output(25, GPIO.LOW)

    else:
        GPIO.output(23, GPIO.LOW)
        GPIO.output(24, GPIO.LOW)
        GPIO.output(25, GPIO.LOW)


async def echo(websocket, path):
    print("A client just connected")
    try:
        async for message in websocket:

            inJson = json.loads(message)
            print(inJson["LED"])
            
            print("Received message from client: " + message)

            if inJson["LED"] == "on":
                toggleLight(True)
            else:
                toggleLight(False)

            if inJson["moveUp"] == True:
                moveLeftMotor(1)
                moveRightMotor(1)

            elif inJson["moveDown"] == True:
                moveLeftMotor(-1)
                moveRightMotor(-1)

            elif inJson["moveLeft"] == True:
                moveLeftMotor(-1)
                moveRightMotor(1)

            elif inJson["moveRight"] == True:
                moveLeftMotor(1)
                moveRightMotor(-1)

            else:
                moveLeftMotor(0)
                moveRightMotor(0)

            await websocket.send("Pong: " + message)
    except websockets.exceptions.ConnectionClosed as e:
        print("A client just disconnected")


if __name__ == '__main__':
    init()

    start_server = websockets.serve(echo, "0.0.0.0", PORT)
    print("Server listening on Port " + str(PORT))

    asyncio.get_event_loop().run_until_complete(start_server)
    asyncio.get_event_loop().run_forever()
