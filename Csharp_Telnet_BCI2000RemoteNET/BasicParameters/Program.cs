// Basic example to start BCI2000 and modify parameters

using BCI2000RemoteNET;

BCI2000Connection bciConnection = new BCI2000Connection();
BCI2000Remote     bciRemote     = new BCI2000Remote(bciConnection);

Console.WriteLine("Basic Parameters");

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

// Create New Parameter
// Parameter in the ParamArea section within the Application tab, with default value 0 
bciRemote.AddParameter("Application:ParamArea", "Param", "0");

// Load Parameter File
//path is relative to the Operator's working directory, that is, the /prog directory of the BCI2000 installation
bciRemote.LoadParameters("..\\parms\\examples\\StimulusPresentation_SignalGenerator.prm");

// Startup BCI2000 SignalSource, SignalProcessing, and Application modules
bciRemote.StartupModules(new Dictionary<string, IEnumerable<string>?>()
    {
    {"SignalGenerator",       null },
    {"DummySignalProcessing", null },
    {"DummyApplication",      null }
    }
);

// Modify Parameters
bciRemote.SetParameter("SubjectName",     "ExampleSubj");
bciRemote.SetParameter("SamplingRate",    "1000");
bciRemote.SetParameter("SampleBlockSize", "50");

// Start Running BCI2000
bciRemote.Start();

// Get Parameter Value
Console.WriteLine("Subject Session: " + bciRemote.GetParameter("SubjectSession"));