// A basic example to starting up BCI2000 and demonstrate sending event values

using BCI2000RemoteNET;

BCI2000Connection bciConnection = new BCI2000Connection();
BCI2000Remote     bciRemote     = new BCI2000Remote(bciConnection);

Console.WriteLine("Basic Event Test");

Console.WriteLine("Enter BCI2000 Root Directory:");
string bci2000root = Console.ReadLine()!;

//string bci2000root = "C:\\bci2000.x64_dev";

string OperatorPath = bci2000root + Path.DirectorySeparatorChar +
    "prog" + Path.DirectorySeparatorChar +
    "Operator.exe";

// Start the Operator
// Not required if starting operator separately (telnet)
bciConnection.StartOperator(OperatorPath);

// Connect to BCI2000 Operator
bciConnection.Connect();

// Create New Event
bciRemote.AddEvent("test_event", 32, 5); // event name, bit width, initial value

bciRemote.StartupModules(new Dictionary<string, IEnumerable<string>?>() 
    {
        {"SignalGenerator",       new List<string>(){"LogKeyboard=1", "LogMouse=1"} },
        {"DummySignalProcessing", null },
        {"DummyApplication",      null } 
    }
);

// Modify Parameters
bciRemote.SetParameter("SamplingRate", "1000");
bciRemote.SetParameter("SampleBlockSize", "1000");

// Add Event Watches
bciRemote.Visualize("test_event");
bciRemote.Visualize("KeyDown");
bciRemote.Visualize("MousePosX");
bciRemote.Visualize("MousePosY");

// Start BCI2000
bciRemote.Start();

// Set Event Value
Console.WriteLine("Setting event to 30");
bciRemote.SetEvent("test_event", 30);

Thread.Sleep(200);

// Get Event Value (events can only be retrieved at a resolution of 1 block)
Console.WriteLine(bciRemote.GetEvent("test_event"));

while (true)
{
    /*Console.WriteLine("Setting event to 10");
    bciRemote.SetEvent("test_event", 10);
    Thread.Sleep(1000);

    Console.WriteLine("Setting event to 15");
    bciRemote.SetEvent("test_event", 15);
    Thread.Sleep(1000);*/

    Console.WriteLine(bciRemote.GetEvent("MousePosX"));
}