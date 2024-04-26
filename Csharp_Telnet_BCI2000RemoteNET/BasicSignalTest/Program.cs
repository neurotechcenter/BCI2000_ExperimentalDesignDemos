using BCI2000RemoteNET;

BCI2000Remote bci = new BCI2000Remote();


bci.OperatorPath = "C:\\BCI2000\\prog\\Operator.exe";

bci.Connect();



bci.StartupModules(new Dictionary<string, List<string>>() {
    {"SignalGenerator", new List<string>() },
    {"DummySignalProcessing", new List<string>() },
    {"DummyApplication", new List<string>() } 
    }
);

bci.WaitForSystemState("Connected");

bci.Start();

while (true) {
    Console.WriteLine(bci.GetSignal(1, 1));
}
