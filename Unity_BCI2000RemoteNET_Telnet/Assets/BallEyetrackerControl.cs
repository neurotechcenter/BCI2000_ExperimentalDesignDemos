using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEyetrackerControl : MonoBehaviour
{
    private UnityBCI2000 bci;

    private TargetControl tc;
    // Start is called before the first frame update

    public int MAX_X = 65535;
    public int MAX_Y = 65535;
    public int X_OFFSET = 0;
    public int Y_OFFSET = 0;

    private double X_MIN = -4.3;
    private double Y_MIN = 0.5;
    private double X_RANGE = 8.8;
    private double Y_RANGE = 8.5;

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
    }

    // Update is called once per frame
    void Update()
    {
        Mpx = bci.Control.GetEvent("EyetrackerLeftEyeGazeX");
        Mpy = bci.Control.GetEvent("EyetrackerLeftEyeGazeY");

        Mpxc = (float) (((Mpx - X_OFFSET) / MAX_X) * X_RANGE + X_MIN);
        Mpyc = (float) (((((Mpy - Y_OFFSET) / MAX_Y) * -1) + 1) * Y_RANGE + Y_MIN);

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
