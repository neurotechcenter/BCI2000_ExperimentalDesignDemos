using BCI2000RemoteNET;
using System.Net.NetworkInformation;

BCI2000Remote bci = new BCI2000Remote();


Console.WriteLine("Enter target machine IP:");
string targetIP = Console.ReadLine();

Console.WriteLine("Enter target machine port:");
int targetPort = Convert.ToInt32(Console.ReadLine());

//string targetIP = "10.0.0.100";
//int targetPort = 3999;

bci.TelnetIp = targetIP; //ip of receiver pc
bci.TelnetPort = targetPort;

Console.WriteLine("Network Test");

Console.WriteLine("Pinging target ip...");
Console.WriteLine(targetIP);
Ping pingSender = new Ping();
PingReply reply = pingSender.Send(targetIP);
Console.WriteLine(reply.Status.ToString());

bci.Connect();


bci.AddEvent("test_event", 32, 5);
//Console.WriteLine("First get event");
//bci.GetEvent("test_event");

bci.StartupModules(new Dictionary<string, List<string>>() {
    {"SignalGenerator", new List<string>(){"LogKeyboard=1", "LogMouse=1"} },
    {"DummySignalProcessing", new List<string>() },
    {"DummyApplication", new List<string>() } });


Console.WriteLine("wait state");
bci.WaitForSystemState("Connected");
Console.WriteLine("end wait");

bci.Execute("visualize watch test_event");

bci.Start();
bci.SetEvent("test_event", 30);
Thread.Sleep(1000);
//Console.WriteLine("Second get event");
//Console.WriteLine(bci.GetEvent("test_event"));


while (true) { 
	bci.SetEvent("test_event", 10);
    Console.WriteLine("event 10");
    Thread.Sleep(1000);
	bci.SetEvent("test_event", 15);
	Console.WriteLine("event 15");
	Thread.Sleep(1000);
}
