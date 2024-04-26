using BCI2000RemoteNET;

BCI2000Remote bci = new BCI2000Remote();


bci.OperatorPath = "C:\\bci2000.x64\\prog\\Operator.exe";

Console.WriteLine("Remote Test");

bci.Connect();


bci.AddEvent("test_event", 32, 5);
Console.WriteLine("First event test");
Console.WriteLine(bci.GetEvent("test_event"));

bci.StartupModules(new Dictionary<string, List<string>>() {
    //{"NihonKohdenSource", new List<string>(){"LogEyetrackerTobiiPro=1", "LogKeyboard=1", "LogMouse=1", "LogWebcam=1", "SecondaryBCI2000=0"} },
    {"SignalGenerator", new List<string>(){"LogKeyboard=1", "LogMouse=1", "LogWebcam=1", "SecondaryBCI2000=1"} },
    {"DummySignalProcessing", new List<string>() },
    {"DummyApplication", new List<string>() } });


Console.WriteLine("wait state");
bci.WaitForSystemState("Connected");
Console.WriteLine("end wait");

bci.Start();
bci.SetEvent("test_event", 30);
Thread.Sleep(1000);
Console.WriteLine("Second test event");
Console.WriteLine(bci.GetEvent("test_event"));

while (true) { }