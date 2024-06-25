using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBehavior : MonoBehaviour
{
    //ADD BCI2000 REFERENCE HERE
    private UnityBCI2000 bci;

    private TargetControl tc;

    public int MAX_X = 2256;
    public int MAX_Y = 1504;
    private int X_OFFSET = 32768;
    private int Y_OFFSET = 32768;

    private double X_MIN = -4.3;
    private double Y_MIN = 0.5;
    private double X_RANGE = 8.8;
    private double Y_RANGE = 8.5;

    bool t1hit;
    bool t2hit;
    bool t3hit;
    bool t4hit;

    private float Mpx = 0;
    private float Mpy = 0;
    private float Mpxc = 0;
    private float Mpyc = 0;
    
    void Awake()
    {
        // SET BCI2000 REFERENCE
        bci = GameObject.Find("BCI2000").GetComponent<UnityBCI2000>();
        tc  = GameObject.Find("TargetControl").GetComponent<TargetControl>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse position event for unity cursor control
        Mpx = bci.Control.GetEvent("MousePosX"); // eventName
        Mpy = bci.Control.GetEvent("MousePosY");

        Mpxc = (float) (((  Mpx - X_OFFSET) / MAX_X)            * X_RANGE + X_MIN);
        Mpyc = (float) (((((Mpy - Y_OFFSET) / MAX_Y) * -1) + 1) * Y_RANGE + Y_MIN);

        transform.position = new Vector3(Mpxc, Mpyc, 0.63f);

        // BCI2000 set position events here
        bci.Control.SetEvent("PositionX", (uint)((transform.position.x + 10) * 1000)); // eventName, eventValue (must be uint)
        bci.Control.SetEvent("PositionY", (uint)( transform.position.y       * 1000));

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
