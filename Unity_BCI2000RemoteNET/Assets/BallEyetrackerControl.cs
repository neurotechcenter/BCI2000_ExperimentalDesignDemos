using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEyetrackerControl : MonoBehaviour
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

    void Awake(){
        bci = GameObject.Find("BCI2000").GetComponent<UnityBCI2000>();
        tc = GameObject.Find("TargetControl").GetComponent<TargetControl>();
    }

    void Start()
    {
        bci2000_xmin = 32768;
        bci2000_xmax = bci2000_xmin + Screen.currentResolution.width;
        bci2000_ymin = 32768;
        bci2000_ymax = bci2000_ymin + Screen.currentResolution.height;

        TopTargetY = GameObject.Find("Target1").transform.position.y;
        RightTargetX = GameObject.Find("Target2").transform.position.x;
        BottomTargetY = GameObject.Find("Target3").transform.position.y;
        LeftTargetX = GameObject.Find("Target4").transform.position.x;

        unity_xrange = RightTargetX - LeftTargetX;
        unity_yrange = TopTargetY - BottomTargetY;
        unity_xoffset = LeftTargetX;
        unity_yoffset = BottomTargetY;
    }

    // Update is called once per frame
    void Update()
    {
        Mpx = bci.Control.GetEvent("EyetrackerLeftEyeGazeX");
        Mpy = bci.Control.GetEvent("EyetrackerLeftEyeGazeY");

        Mpxc = (float)((Mpx - bci2000_xmin) / (bci2000_xmax - bci2000_xmin) * unity_xrange + unity_xoffset);
        Mpyc = (float)((-1 * (Mpy - bci2000_ymin) / (bci2000_ymax - bci2000_ymin) + 1) * unity_yrange + unity_yoffset);

        transform.position = new Vector3(Mpxc, Mpyc, 0.63f);


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
