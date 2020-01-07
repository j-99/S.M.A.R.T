// By J-99

var socket;

$( document ).ready(function() {
    console.log( "ready!" );
    socket = io();
	socket.on('servo', showData);
});

function showData(data){
	$('#servoA').text(data.x);
	$('#servoB').text(data.y);
	$('#servoC').text(data.z);
}

function controlBot(){
	var data = {'com': $("#command").val()}
	socket.emit('command', data)
}
