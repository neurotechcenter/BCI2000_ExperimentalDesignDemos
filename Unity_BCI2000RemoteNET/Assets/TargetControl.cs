using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class TargetControl : MonoBehaviour
{
    //ADD BCI2000 REFERENCE HERE
    private UnityBCI2000 bci;
    
    GameObject t1;
    GameObject t2;
    GameObject t3;
    GameObject t4;

    const int TIME = 200;
    int framecount = 0;

    System.Random rng = new System.Random();

    bool targetActive = false;

    bool[] targetCol = new bool[4];

    string BCI2000SamplingRate;

    public void SetTargetCol(bool[] targetCols)
    {
        targetCol = targetCols;
    }

    void Awake()
    {
        //SET BCI2000 REFERENCE
        bci = GameObject.Find("BCI2000").GetComponent<UnityBCI2000>();

        bci.OnIdle(bci =>
        {
            bci.AddEvent("t1hit", 1); // eventName, bitWidth
            bci.AddEvent("t2hit", 1);
            bci.AddEvent("t3hit", 1);
            bci.AddEvent("t4hit", 1);

          // BCI2000 create new parameter
          // Tab:Field, ParamName, ParamValue 
          bci.AddParameter("Visualize:Timing", "ExampleParam", "4");            
        });

        bci.OnConnected(bci =>
        {
            // BCI2000 add event watches
            bci.Visualize("t1hit"); // eventName
            bci.Visualize("t2hit");
            bci.Visualize("t3hit");
            bci.Visualize("t4hit");

            // BCI2000 set parameter
            // paramName, paramValue
            bci.SetParameter("SubjectName",     "UnitySubject"); 
            bci.SetParameter("SamplingRate",    "200");         
            bci.SetParameter("SampleBlockSize", "10");

            // BCI2000 get parameter
            // paramName
            BCI2000SamplingRate = bci.GetParameter("SamplingRate"); 
            Debug.Log("BCI2000 Sampling Rate: " + BCI2000SamplingRate);
        });

    }

    // Start is called before the first frame update
    void Start()
    {
        t1 = GameObject.Find("Target1");
        t2 = GameObject.Find("Target2");
        t3 = GameObject.Find("Target3");
        t4 = GameObject.Find("Target4");

        t1.SetActive(false);
        t2.SetActive(false);
        t3.SetActive(false);
        t4.SetActive(false);
<<<<<<< HEAD

=======
>>>>>>> 6cd8fda5fc89e87428191f4287ad6fae25c863ea
    }

    // Update is called once per frame
    void Update()
    {
        if (framecount < TIME)
        {
            framecount++;
        }
        else
        {
            framecount = 0;
            if (!targetActive)
            {
                targetActive = true;
                int rn = rng.Next(100);
                if (rn < 25)
                {
                    t1.SetActive(true);
                }
                else if (rn < 50)
                {
                    t2.SetActive(true);
                }
                else if (rn < 75)
                {
                    t3.SetActive(true);

                }
                else if (rn < 100)
                {
                    t4.SetActive(true);
                }
            }
        }
        if (targetActive)
        {
            if (t1.activeSelf && targetCol[0])
            {
                // BCI2000 Set t1 event
<<<<<<< HEAD
                // eventName, eventValue (must be uint)
                bci.Control.SetEvent("t1hit", 1); 
=======
                bci.Control.SetEvent("t1hit", 1); // eventName, eventValue (must be uint)

                // Can be used to write strings to the Notes event
                //bci.Control.connection.Execute("PUT NOTE t1hit");

>>>>>>> 6cd8fda5fc89e87428191f4287ad6fae25c863ea
                t1.SetActive(false);
                targetActive = false;
            } else if (t2.activeSelf && targetCol[1])
            {
                // BCI2000 Set t2 event
                bci.Control.SetEvent("t2hit", 1);

                t2.SetActive(false);
                targetActive = false;
            } else if (t3.activeSelf && targetCol[2])
            {
                // BCI2000 Set t3 event
                bci.Control.SetEvent("t3hit", 1);

                t3.SetActive(false);
                targetActive = false;
            } else if (t4.activeSelf && targetCol[3])
            {
                // BCI2000 Set t4 event
                bci.Control.SetEvent("t4hit", 1);

                t4.SetActive(false);
                targetActive = false;
            }
        }
        else
        {
            // BCI2000 set target events to 0 here
            bci.Control.SetEvent("t1hit", 0);
            bci.Control.SetEvent("t2hit", 0);
            bci.Control.SetEvent("t3hit", 0);
            bci.Control.SetEvent("t4hit", 0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            UnityEngine.Application.Quit();
    }
}
