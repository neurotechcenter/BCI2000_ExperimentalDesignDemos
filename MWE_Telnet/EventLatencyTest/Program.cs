///
/// This script tests latency in sending and receiving events from BCI2000.
/// It sets an event value and attempts to retrieve it after a certain time interval.
/// This time interval starts at 1/3 seconds and steps down in increments of one frame (at 60fps, 16.6ms) for each cycle.
///


using BCI2000RemoteNET;

BCI2000Remote bci = new BCI2000Remote();


bci.OperatorPath = "C:\\BCI2000\\prog\\Operator.exe";

bci.Connect();

bci.AddEvent("test_event", 32, 0);

float FRAME_TIME = 1000f / 60f;

bci.StartupModules(new Dictionary<string, List<string>>() {
    {"SignalGenerator", new List<string>() },
    {"DummySignalProcessing", new List<string>() },
    {"DummyApplication", new List<string>() } });


bci.Start();

Thread.Sleep(1000);

for (int i = 20; i > 0; i--)
{
    int waitMs = (int) Math.Round(FRAME_TIME * i);
    bci.SetEvent("test_event", 1);
    Thread.Sleep(waitMs);
    if (bci.GetEvent("test_event") == 1)
    {
        Console.WriteLine("Test succeeded for " + waitMs + "ms");
    } else
    {
        Console.WriteLine("Test failed for " + waitMs + "ms");
    }
    bci.SetEvent("test_event", 0);
    Thread.Sleep(1000);
}

while (true) { 
    int waitMs = (int) Math.Round(FRAME_TIME * 8);
    bci.SetEvent("test_event", 1);
    Thread.Sleep(waitMs);
    if (bci.GetEvent("test_event") == 1)
    {
        Console.WriteLine("Test succeeded for " + waitMs + "ms");
    } else
    {
        Console.WriteLine("Test failed for " + waitMs + "ms");
    }
    bci.SetEvent("test_event", 0);
    Thread.Sleep(1000);
};
