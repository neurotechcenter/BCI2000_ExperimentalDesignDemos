var c = document.getElementById("myCanvas");
var ctx = c.getContext('2d');
var colorArray = [
  '#ff0000',
  '#ffff00',
  '#0000ff',
  '#ff00ff'
];
var color_idx = 1;

//create a webSocket client object which connect to the BCI2000 server
var BCIwebSocket = new WebSocket("ws://localhost:3998");
//try to connect to bci2000, if successed, do the BCI2000 initialization work
BCIwebSocket.onopen = function(evt) {
    console.log("*****WebSocket ONOPEN");
    BCIwebSocket.send("Hide window watches");
    BCIwebSocket.send("Reset system");
	
	//add state
    BCIwebSocket.send("ADD EVENT Square 4 0");
	
    BCIwebSocket.send("Startup system localhost");
	
    BCIwebSocket.send("Start executable SignalGenerator --local --LogMouse=1");
    BCIwebSocket.send("Start executable DummySignalProcessing --local");
    BCIwebSocket.send("Start executable DummyApplication --local");
	
    BCIwebSocket.send("Wait for Connected");
	
    BCIwebSocket.send("setconfig");
	
    //add watch
    BCIwebSocket.send("Show window watches");
    BCIwebSocket.send("visualize watch Square");  

	//start
    BCIwebSocket.send("start");

};

function changeColor() {
  var Color = colorArray[color_idx-1];
  ctx.beginPath();  
  ctx.rect(10, 10, 150, 80);
  ctx.fillStyle = Color;
  ctx.fill();
  //send color idx to BCI2000
  BCIwebSocket.send("Set event Square " + color_idx);
  color_idx++;
  if(color_idx > 4){
    color_idx = 1;
  }
}