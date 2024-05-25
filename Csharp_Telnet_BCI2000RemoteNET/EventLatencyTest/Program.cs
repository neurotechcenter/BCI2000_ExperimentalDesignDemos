///
/// This script tests latency in sending and receiving events from BCI2000.
/// It sets an event value and attempts to retrieve it after a certain time interval.
/// This time interval starts at 1/3 seconds and steps down in increments of one frame (at 60fps, 16.6ms) for each cycle.
///

using BCI2000RemoteNET;

BCI2000Connection bciConnection = new BCI2000Connection();
BCI2000Remote     bciRemote     = new BCI2000Remote(bciConnection);

Console.WriteLine("Event Latency Test");

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

bciRemote.AddEvent("test_event", 32, 0);

float FRAME_TIME = 1000f / 60f;

bciRemote.StartupModules(new Dictionary<string, IEnumerable<string>?>() 
    {
    {"SignalGenerator",       null },
    {"DummySignalProcessing", null },
    {"DummyApplication",      null } 
    }
);

// Modify Parameters
bciRemote.SetParameter("SamplingRate",    "1000");
bciRemote.SetParameter("SampleBlockSize", "50");

// Add Event Watches
bciRemote.Visualize("test_event");

// Start Running BCI2000
bciRemote.Start();

Thread.Sleep(1000);

for (int i = 20; i > 0; i--)
{
    int waitMs = (int) Math.Round(FRAME_TIME * i);

    bciRemote.SetEvent("test_event", 1);
    Thread.Sleep(waitMs);
    if (bciRemote.GetEvent("test_event") == 1)
    {
        Console.WriteLine("Test succeeded for " + waitMs + "ms");
    } else
    {
        Console.WriteLine("Test failed for " + waitMs + "ms");
    }
    bciRemote.SetEvent("test_event", 0);
    Thread.Sleep(1000);
}

while (true)
{
    int waitMs = (int)Math.Round(FRAME_TIME * 6);

    bciRemote.SetEvent("test_event", 1);
    Thread.Sleep(waitMs);
    if (bciRemote.GetEvent("test_event") == 1)
    {
        Console.WriteLine("Test succeeded for " + waitMs + "ms");
    } else
    {
        Console.WriteLine("Test failed for " + waitMs + "ms");
    }
    bciRemote.SetEvent("test_event", 0);
    Thread.Sleep(1000);
};
