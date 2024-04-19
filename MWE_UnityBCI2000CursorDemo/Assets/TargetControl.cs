using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TargetControl : MonoBehaviour
{
    GameObject t1;
    GameObject t2;
    GameObject t3;
    GameObject t4;

    string SubjName;

    const int TIME = 200;
    int framecount = 0;

    System.Random rng = new System.Random();

    bool targetActive = false;

    bool[] targetCol = new bool[4];

    public void SetTargetCol(bool[] targetCols)
    {
        targetCol = targetCols;
    }

    // BCI2000
    UnityBCI2000 bci;

    void Awake()
    {
        // SET BCI2000 REFERENCE HERE
        bci = GameObject.Find("BCI2000").GetComponent<UnityBCI2000>();

        // BCI2000 Add Parameters
        bci.AddParam("Application:SubField", "NewParam", "DefaultValue", "minValue", "maxValue");

        // BCI2000 Set Parameters
        bci.SetParam("SubjectName", "TestSubject");
        bci.SetParam("SamplingRate",    "1000");
        bci.SetParam("SampleBlockSize", "50");

        // BCI2000 Add Events
        bci.AddEvent("t1hit", 2);
        bci.AddEvent("t2hit", 2);
        bci.AddEvent("t3hit", 2);
        bci.AddEvent("t4hit", 2);

        // Add BCI2000 event watches
        bci.ExecuteCommand("visualize watch t1hit");
        bci.ExecuteCommand("visualize watch t2hit");
        bci.ExecuteCommand("visualize watch t3hit");
        bci.ExecuteCommand("visualize watch t4hit");
        
        // Change log level to avoid continuously printing event changes to
        // operator log
        bci.ExecuteCommand("Set variable LogLevel 0");
    }

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

        // BCI2000 Get Parameters
        //SubjName = bci.GetParam("SubjectName");
        //Debug.Log(SubjName);
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
                // BCI2000 SET T1HIT HERE
                bci.SetEvent("t1hit", 1);

                t1.SetActive(false);
                targetActive = false;
            }
            else if (t2.activeSelf && targetCol[1])
            {
                // BCI2000 SET T2HIT HERE
                bci.SetEvent("t2hit", 1);

                t2.SetActive(false);
                targetActive = false;
            }
            else if (t3.activeSelf && targetCol[2])
            {
                // BCI2000 SET T3HIT HERE
                bci.SetEvent("t3hit", 1);

                t3.SetActive(false);
                targetActive = false;
            }
            else if (t4.activeSelf && targetCol[3])
            {
                // BCI2000 SET T4HIT HERE
                bci.SetEvent("t4hit", 1);

                t4.SetActive(false);
                targetActive = false;
            }
        }
        else
        {
            // BCI2000 Target VALUES TO 0 HERE
            bci.SetEvent("t1hit", 0);
            bci.SetEvent("t2hit", 0);
            bci.SetEvent("t3hit", 0);
            bci.SetEvent("t4hit", 0);
        }
    }
}
