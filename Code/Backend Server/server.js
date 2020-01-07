// Developed by J-99

var express = require('express');

const PORT = process.env.PORT || 3000;

var app = express();
var server = app.listen(PORT);

var socket = require('socket.io');
var io = socket(server);

app.use(express.static('public'));

console.log('Socket server running on port %d :)', PORT);

io.sockets.on('connection', newConnection);

function newConnection(socket){
	console.log('new connection : ' + socket.id);

	socket.on('servo', servoData);

	function servoData(data){
		socket.broadcast.emit('servo', data);
		console.log(data);
	}

	socket.on('command', sendCommand);

	function sendCommand(data){
		socket.broadcast.emit('command', data);
		console.log(data);
	}
}
