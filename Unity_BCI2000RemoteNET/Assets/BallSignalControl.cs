using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    public class SphereSignalBehaviour : MonoBehaviour
{
    private UnityBCI2000 bci;

    private TargetControl tc;
    // Start is called before the first frame update

    private int bci2000_xmin; // bci2000 cursor offset 2^15
    private int bci2000_xmax; // bci2000 offset + display resolution x
    private int bci2000_ymin; // bci2000 cursor offset 2^15
    private int bci2000_ymax; // bci2000 offset + display resolution y

    private double TopTargetY;    // Target positions for scaling
    private double RightTargetX;
    private double BottomTargetY;
    private double LeftTargetX;

    private double unity_xrange;  // distance between x targets
    private double unity_yrange;  // distance between y targets
    private double unity_xoffset; // left target
    private double unity_yoffset; // bottom target


    private Queue<double> pastValsX = new Queue<double>();
    private Queue<double> pastValsY = new Queue<double>();
    
    private bool t1hit;
    private bool t2hit;
    private bool t3hit;
    private bool t4hit;

    private bool visualizeFlag = false;


    [SerializeField]
    public int AverageFrames = 5;


    [SerializeField]
    double Mpx = 0;
    [SerializeField]
    double Mpy = 0;
    [SerializeField]
    double Mpxc = 0;
    [SerializeField]
    double Mpyc = 0;

    void Awake() 
    {
        bci = GameObject.Find("BCI2000").GetComponent<UnityBCI2000>();
        tc = GameObject.Find("TargetControl").GetComponent<TargetControl>();

        bci.OnIdle(bci =>
        {
            // BCI2000 add events
            bci.AddEvent("PositionX", 32); // eventName, bitWidth
            bci.AddEvent("PositionY", 32);
        });

        
        bci.OnConnected(bci =>
        {
            //To delete
            bci.SetParameter("ModulateAmplitude", "1");    // paramName, paramValue
            bci.SetParameter("SineChannelX", "1");         // paramName, paramValue
            bci.SetParameter("SineChannelY", "2");         // paramName, paramValue
            bci.SetParameter("SineFrequency", "120Hz");    // paramName, paramValue
            bci.SetParameter("SineAmplitude", "100muV");   // paramName, paramValue
            bci.SetParameter("NoiseAmplitude", "0muV");    // paramName, paramValue
            bci.SetParameter("DCOffset", "0muV");          // paramName, paramValue
            //bci.SetParameter("UpdateTrigger", "");          // paramName, paramValue
            //bci.connection.Execute("set parameter % matrix BufferConditions= 2 2 0 0 0 0");
        });

    }

    void Start()
    {
        bci2000_xmin = 0;
        bci2000_xmax = bci2000_xmin + -60;
        bci2000_ymin = 0;
        bci2000_ymax = bci2000_ymin + -60;

        TopTargetY    = GameObject.Find("Target1").transform.position.y;
        RightTargetX  = GameObject.Find("Target2").transform.position.x;
        BottomTargetY = GameObject.Find("Target3").transform.position.y;
        LeftTargetX   = GameObject.Find("Target4").transform.position.x;

        unity_xrange  = RightTargetX - LeftTargetX;
        unity_yrange  = TopTargetY   - BottomTargetY;
        unity_xoffset = LeftTargetX;
        unity_yoffset = BottomTargetY;

        for (int i = 0; i < AverageFrames; i++)
        {
            pastValsX.Enqueue(0);
            pastValsY.Enqueue(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!visualizeFlag)
        {
            // BCI2000 add event watches
            bci.Control.Visualize("Signal(1,1)");
            bci.Control.Visualize("PositionX");
            bci.Control.Visualize("Signal(2,1)");
            bci.Control.Visualize("PositionY");

            visualizeFlag = true;
        }

        pastValsX.Dequeue();
        pastValsX.Enqueue(bci.Control.GetSignal(1, 1));

        pastValsY.Dequeue();
        pastValsY.Enqueue(bci.Control.GetSignal(2, 1));

        Mpx = pastValsX.Max();
        Mpy = pastValsY.Max();

        Mpxc = (float)((Mpx - bci2000_xmin) / (bci2000_xmax - bci2000_xmin) * unity_xrange + unity_xoffset);
        Mpyc = (float)((Mpy - bci2000_ymin) / (bci2000_ymax - bci2000_ymin) * unity_yrange + unity_yoffset);

        transform.position = new Vector3((float) Mpxc, (float) Mpyc, 0.63f);

        // BCI2000 set position events here
        bci.Control.SetEvent("PositionX", (uint)((transform.position.x + 10) * 1000)); // eventName, eventValue (must be uint)
        bci.Control.SetEvent("PositionY", (uint)(transform.position.y * 1000));

        var x = transform.position.x;
        var y = transform.position.y;

        t1hit = false;
        t2hit = false;
        t3hit = false;
        t4hit = false;

        if (x < -3.5)
            t4hit = true;
        if (x > 3.5)
            t2hit = true;
        if (y > 8.6)
            t1hit = true;
        if (y < 1.2)
            t3hit = true;

        tc.SetTargetCol(new bool[] { t1hit, t2hit, t3hit, t4hit });
    }
}
