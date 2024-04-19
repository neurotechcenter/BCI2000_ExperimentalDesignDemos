var c = document.getElementById("myCanvas");
var ctx = c.getContext('2d');
var colorArray = [
  '#CEF19E',
  '#A7DDA7',
  '#78BE97',
  '#398689',
  '#0B476D'
];
var color_idx = 0;

//create a webSocket client object which connect to the BCI2000 server
var BCIwebSocket = new WebSocket("ws://localhost:3998");
//try to connect to bci2000, if successed, do the BCI2000 initialization work
BCIwebSocket.onopen = function(evt) {
    console.log("*****WebSocket ONOPEN");
    BCIwebSocket.send("Hide window watches");
    BCIwebSocket.send("Reset system");
    BCIwebSocket.send("Startup system localhost");
    BCIwebSocket.send("Start executable SignalGenerator --local --LogMouse=1");
    BCIwebSocket.send("Start executable DummySignalProcessing --local");
    BCIwebSocket.send("Start executable DummyApplication --local");
    BCIwebSocket.send("Wait for Connected");
    //add state
    BCIwebSocket.send("add state Color 16 0");
    BCIwebSocket.send("setconfig");
    //add watch
    BCIwebSocket.send("Show window watches");
    BCIwebSocket.send("visualize watch Color");     
    BCIwebSocket.send("start");

};

function changeColor() {
  // var color_idx = Math.floor(Math.random() * colorArray.length);
  var randomColor = colorArray[color_idx];
  ctx.beginPath();  
  ctx.rect(10, 10, 150, 80);
  ctx.fillStyle = randomColor;
  ctx.fill();
  //send color idx to BCI2000
  BCIwebSocket.send("set state Color " + color_idx);
  color_idx++;
  if(color_idx > 4){
    color_idx = 0;
  }
}