// BCI2000 Operator must be started in telnet mode, with the IP address specified
// An example batch file TelnetStartup.bat can be used for this
// The IP address in the batch file should be the IP of the machine running BCI2000
// This IP must match the IP provided when running this script

using BCI2000RemoteNET;
using System.Net.NetworkInformation;

BCI2000Connection bciConnection = new BCI2000Connection();
BCI2000Remote     bciRemote     = new BCI2000Remote(bciConnection);

Console.WriteLine("Network Event Sender");

Console.WriteLine("Enter target machine IP:");
string targetIP = Console.ReadLine()!;

Console.WriteLine("Enter target machine port:");
int targetPort  = Convert.ToInt32(Console.ReadLine())!;

//string targetIP = "127.0.0.1";
//int targetPort  = 3999;

// Test if we can ping target computer
Console.WriteLine("Pinging target ip...");
Console.WriteLine(targetIP);
Ping pingSender = new Ping();
PingReply reply = pingSender.Send(targetIP);
Console.WriteLine(reply.Status.ToString());

bciConnection.Connect(address: targetIP, port: targetPort);

// Modules started up on receiving computer
// event created in batch file on receiving computer

// Modify Parameters
bciRemote.SetParameter("SamplingRate", "1000");
bciRemote.SetParameter("SampleBlockSize", "50");

//bciRemote.Visualize("test_event");

Console.WriteLine("Waiting for BCI2000 to start...");
bciRemote.WaitForSystemState(BCI2000Remote.SystemState.Running);
Console.WriteLine("BCI2000 is Running!");

Thread.Sleep(1000);
Console.WriteLine("Setting event to 30");
bciRemote.SetEvent("test_event", 30);

while (true) {
    Console.WriteLine("Setting event to 10");
    bciRemote.SetEvent("test_event", 10);
    Thread.Sleep(1000);

    Console.WriteLine("Setting event to 15");
    bciRemote.SetEvent("test_event", 15);
	Thread.Sleep(1000);
}
