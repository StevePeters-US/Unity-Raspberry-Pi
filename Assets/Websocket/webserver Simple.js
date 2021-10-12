var Gpio = require('onoff').Gpio; //include onoff to interact with the GPIO
var LED26 = new Gpio(26, 'out'); //use GPIO pin 26 as output
var GPIO26value = 0;  // Turn on the LED by default


const WebSocket = require('ws')
const wss = new WebSocket.Server({ port: 8080 }, () => {
    console.log('server started')
})

wss.on('connection', function connection(ws) {
    ws.on('message', (data) => {
        console.log('data received \n %o', data)

        if (data == 'Hello') {
            LED26.writeSync(1); // Turn LED on
        }

        if (data == 'GoodBye') {
            LED26.writeSync(0); // Turn LED off
        }

        ws.send(data);
    })
})

wss.on('listening', () => {
    console.log('listening on 8080')
})