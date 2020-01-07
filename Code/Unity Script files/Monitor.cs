/**************************************************************************************
 
 ** Developed by Team LemonSky **
 ** Hack Your Reality Hackathon 2019 **
 
**************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Monitor : MonoBehaviour
{
    public GameObject textbox;
    public GameObject[] servos;
    public float moveSpeed = 0.01f;
    private int i = 0;
    private int length_com;
    private bool loop = false;
    string[] Commands;
    string[] Compares;
    private string compare = "0;0;0;";

    void Update()
    {
        string input = textbox.GetComponent<UnityEngine.UI.Text>().text;
        Commands = input.Split(';');
        Compares = compare.Split(';');
        //Debug.Log(Commands[0]+";"+Commands[1]+";"+Commands[2]);
        if(Commands[0] != Compares[0])
        {
            moveServo(0);
        }
        if (Commands[1] != Compares[1])
        {
            moveServo(1);
        }
        if (Commands[2] != Compares[2])
        {
            moveServo(2);
        }
        compare = input;
    }
    public void moveServo(int j)
    {
        int angle = Mathf.RoundToInt(float.Parse(Commands[j]));
        //Debug.Log(j + " = " + Commands[j]);
        servoSend(j, angle);
    }

    private void servoSend(int ser_no, int angle)
    {
        GameObject active_servo = servos[ser_no];
        Debug.Log(angle + " : " + ser_no);
        var x = active_servo.transform.localEulerAngles.x;
        var y = active_servo.transform.localEulerAngles.y;
        var z = active_servo.transform.localEulerAngles.z;

        if (ser_no == 0)
        {
           active_servo.transform.localRotation = Quaternion.Euler(x, (-1) * angle, z);
        }

        else
        {
            active_servo.transform.localRotation = Quaternion.Euler(angle, y, z);
        }

    }


}

