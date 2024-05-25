// Basic example to start BCI2000 and acquire the value from the first channel

using BCI2000RemoteNET;
using System.Collections.Generic;

BCI2000Connection bciConnection = new BCI2000Connection();
BCI2000Remote     bciRemote     = new BCI2000Remote(bciConnection);

Console.WriteLine("Basic Signal Test");

Console.WriteLine("Enter BCI2000 Root Directory:");
string bci2000root = Console.ReadLine()!;

//string bci2000root  = "C:\\bci2000.x64_dev";

string OperatorPath = bci2000root + Path.DirectorySeparatorChar +
    "prog" + Path.DirectorySeparatorChar +
    "Operator.exe";

// Start the Operator
// Not required if starting operator separately (telnet)
bciConnection.StartOperator(OperatorPath);

// Connect to BCI2000 Operator
bciConnection.Connect();

// Startup BCI2000 SignalSource, SignalProcessing, and Application modules
bciRemote.StartupModules(new Dictionary<string, IEnumerable<string>?>() 
    {
    {"SignalGenerator",       null },
    {"DummySignalProcessing", null },
    {"DummyApplication",      null } 
    }
);

// Modify Parameters
bciRemote.SetParameter("SamplingRate",    "1000");
bciRemote.SetParameter("SampleBlockSize", "1000");

// Start Running BCI2000
bciRemote.Start();

// Print value of first channel of the control signal to console
while (true) {
    Console.WriteLine(bciRemote.GetSignal(1, 1)); //Channel, Sample within block
}
