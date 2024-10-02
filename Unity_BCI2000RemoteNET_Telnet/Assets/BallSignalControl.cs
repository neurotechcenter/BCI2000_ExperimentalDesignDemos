<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SphereSignalBehaviour : MonoBehaviour
{
    private UnityBCI2000 bci;

    private TargetControl tc;
    // Start is called before the first frame update

    private int MAX_X = 100;
    private int MAX_Y = 100;
    private int X_OFFSET = 0;
    private int Y_OFFSET = 0;

    private double X_MIN = -4.3;
    private double Y_MIN = 0.5;
    private double X_RANGE = 8.8;
    private double Y_RANGE = 8.5;


    private Queue<double> pastValsX = new Queue<double>();
    private Queue<double> pastValsY = new Queue<double>();
    
    private bool t1hit;
    private bool t2hit;
    private bool t3hit;
    private bool t4hit;


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
    }

    void Start()
    {
        for (int i = 0; i < AverageFrames; i++)
        {
            pastValsX.Enqueue(0);
            pastValsY.Enqueue(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        pastValsX.Dequeue();
        pastValsX.Enqueue(bci.Control.GetSignal(1, 1));

        pastValsY.Dequeue();
        pastValsY.Enqueue(bci.Control.GetSignal(2, 1));

        Mpx = pastValsX.Max();
        Mpy = pastValsY.Max();

        Mpxc = (((Mpx - X_OFFSET) / MAX_X) * X_RANGE + X_MIN);
        Mpyc = (((Mpy - Y_OFFSET) / MAX_Y) * Y_RANGE + Y_MIN);

        transform.position = new Vector3((float) Mpxc, (float) Mpyc, 0.63f);


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
=======
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SphereSignalBehaviour : MonoBehaviour
{
    private UnityBCI2000 bci;

    private TargetControl tc;
    // Start is called before the first frame update

    private int MAX_X = 100;
    private int MAX_Y = 100;
    private int X_OFFSET = 0;
    private int Y_OFFSET = 0;

    private double X_MIN = -4.3;
    private double Y_MIN = 0.5;
    private double X_RANGE = 8.8;
    private double Y_RANGE = 8.5;


    private Queue<double> pastValsX = new Queue<double>();
    private Queue<double> pastValsY = new Queue<double>();
    
    private bool t1hit;
    private bool t2hit;
    private bool t3hit;
    private bool t4hit;


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
    }

    void Start()
    {
        for (int i = 0; i < AverageFrames; i++)
        {
            pastValsX.Enqueue(0);
            pastValsY.Enqueue(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        pastValsX.Dequeue();
        pastValsX.Enqueue(bci.Control.GetSignal(1, 1));

        pastValsY.Dequeue();
        pastValsY.Enqueue(bci.Control.GetSignal(2, 1));

        Mpx = pastValsX.Max();
        Mpy = pastValsY.Max();

        Mpxc = (((Mpx - X_OFFSET) / MAX_X) * X_RANGE + X_MIN);
        Mpyc = (((Mpy - Y_OFFSET) / MAX_Y) * Y_RANGE + Y_MIN);

        transform.position = new Vector3((float) Mpxc, (float) Mpyc, 0.63f);


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
>>>>>>> 6cd8fda5fc89e87428191f4287ad6fae25c863ea
