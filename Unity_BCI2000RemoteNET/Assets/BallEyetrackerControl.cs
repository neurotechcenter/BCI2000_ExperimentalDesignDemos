using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEyetrackerControl : MonoBehaviour
{
    // BCI2000
    UnityBCI2000 bci;

    private TargetControl tc;
    // Start is called before the first frame update

    public int MAX_X    = 65535; //65535
    public int MAX_Y    = 65535; //65535
    public int X_OFFSET = 60000;
    public int Y_OFFSET = 60000;

    private double X_MIN   = -20; //-4.3
    private double Y_MIN   = -20; //0.5
    private double X_RANGE = 1310; //8.8
    private double Y_RANGE = 1310; //8.5

    bool t1hit;
    bool t2hit;
    bool t3hit;
    bool t4hit;


    [SerializeField]
    float Mpx = 0;
    [SerializeField]
    float Mpy = 0;
    [SerializeField]
    float Mpxc = 0;
    [SerializeField]
    float Mpyc = 0;

    void Awake()
    {
        //SET BCI2000 REFERENCE HERE
        bci = GameObject.Find("BCI2000").GetComponent<UnityBCI2000>();

        // BCI2000 Add Events
        bci.AddEvent("PositionX", 32);
        bci.AddEvent("PositionY", 32);

        // Add BCI2000 event watches
        bci.ExecuteCommand("visualize watch PositionX");
        bci.ExecuteCommand("visualize watch PositionY");
        bci.ExecuteCommand("visualize watch EyetrackerLeftEyeGazeX");
        bci.ExecuteCommand("visualize watch EyetrackerRightEyeGazeX");
        bci.ExecuteCommand("visualize watch EyetrackerLeftEyeGazeY");
        bci.ExecuteCommand("visualize watch EyetrackerRightEyeGazeY");
    }

    void Start()
    {
        
        
        tc = GameObject.Find("TargetControl").GetComponent<TargetControl>();
    }

    // Update is called once per frame
    void Update()
    {
        // BCI2000 Get eye tracker events
        Mpx = bci.GetEvent("EyetrackerLeftEyeGazeX");
        Mpy = bci.GetEvent("EyetrackerLeftEyeGazeY");

        Mpxc = (float) (((  Mpx - X_OFFSET) / MAX_X)            * X_RANGE + X_MIN);
        Mpyc = (float) (((  Mpy - Y_OFFSET) / MAX_Y)            * Y_RANGE + Y_MIN);

        transform.position = new Vector3(Mpxc, Mpyc, 0.63f);

        // BCI2000 SET POSITION EVENTS HERE
        bci.SetEvent("PositionX", (int)(transform.position.x + 10 * 1000));
        bci.SetEvent("PositionY", (int)(transform.position.y      * 1000));

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
