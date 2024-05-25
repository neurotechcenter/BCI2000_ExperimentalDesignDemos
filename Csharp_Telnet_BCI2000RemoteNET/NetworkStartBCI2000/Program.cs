// BCI2000 Operator must be started in telnet mode, with the IP address specified
// An example batch file TelnetStartup_OperatorOnly.bat can be used for this
// The IP address in the batch file should be the IP of the machine running BCI2000
// This IP must match the IP provided when running this script

using BCI2000RemoteNET;
using System.Net.NetworkInformation;

BCI2000Connection bciConnection = new BCI2000Connection();
BCI2000Remote     bciRemote     = new BCI2000Remote(bciConnection);

Console.WriteLine("Network Start BCI2000");

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

// Connect to BCI2000 Operator
bciConnection.Connect();

bciRemote.AddEvent("test_event", 32, 5); // event name, bit width, initial value

bciRemote.StartupModules(new Dictionary<string, IEnumerable<string>?>() 
    {
        {"SignalGenerator",       null },
        {"DummySignalProcessing", null },
        {"DummyApplication",      null }
    }
);

// Modify Parameters
bciRemote.SetParameter("SamplingRate", "1000");
bciRemote.SetParameter("SampleBlockSize", "50");

// Add Event Watches
bciRemote.Visualize("test_event");

bciRemote.Start();

bciRemote.SetEvent("test_event", 30);

//Thread.Sleep(1000);
//Console.WriteLine("event return value: " + bci.GetEvent("test_event"));


while (true) {
    Console.WriteLine("Setting event to 10");
    bciRemote.SetEvent("test_event", 10);
    Thread.Sleep(1000);

    Console.WriteLine("Setting event to 15");
    bciRemote.SetEvent("test_event", 15);
    Thread.Sleep(1000);
}
