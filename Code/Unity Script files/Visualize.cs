/**************************************************************************************
 
 ** Developed by Team LemonSky **
 ** Hack Your Reality Hackathon 2019 **
 
**************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class Visualize : MonoBehaviour
{
    public GameObject textbox;
    public GameObject input_field;
    public GameObject stopButton;
    public GameObject[] servos;
    public float moveSpeed = 0.01f;
    private int i = 0;
    private int length_com;
    private bool loop = false;
    string[] Commands;
    float speed = 5f; //Speed of rotation at the start
    float minSpeed = 2.5f; //Speed that rotation will gradually lerp to
    float amountRotated = 0f; //keep track of the amount we have rotated
    Quaternion first, second, third;
    int firstangle;
    int secondangle;
    int thirdangle;
    int store_ang = 0;

    private void Start()
    {
        first = servos[0].transform.localRotation;
        second = servos[1].transform.localRotation * Quaternion.Euler(-30, 0, 0);
        third = servos[2].transform.localRotation * Quaternion.Euler(-50, 0, 0);
    }
    public void Manual()
    {

        Commands = null;
        string input = textbox.GetComponent<TextMeshProUGUI>().text;
        i = 0;
        loop = false;
        if(input[0] == 'L')
        {
            loop = true;
            int end = input.Length - 4;
            Debug.Log(input.Substring(2, end));
            Commands = input.Substring(2,end).Split(';');
            stopButton.SetActive(true);
        }
        else
        {
            Commands = input.Split(';');  
        }
        length_com = Commands.Length - 1;
        moveServo(i);
    }

    public void stopLoop()
    {
        loop = false;
        Debug.Log("Loop Stopped!");
        stopButton.SetActive(false);
    }

    public string Send(int boo)
    {
        string input = textbox.GetComponent<TextMeshProUGUI>().text;
        loop = false;
        if (input[0] == 'L')
        {
            loop = true;
            int end = input.Length - 4;
            Debug.Log(input.Substring(2, end));
            Commands = input.Substring(2, end).Split(';');
            stopButton.SetActive(true);
        }
        else
        {
            Commands = input.Split(';');
        }
        length_com = Commands.Length - 1;
        if (loop == true)
        {
            boo = boo % length_com;
            Debug.Log("loop ");
        }
        Debug.Log("boo : " + boo);
        return Commands[boo];
    }

    public void moveServo(int j)
    {
        string command = Commands[j];
        if (command.Length > 2)
        {
            string[] com = command.Split('S');
            int Motor_Number = Mathf.RoundToInt(float.Parse(com[0]));
         //   Debug.Log("n" + com[1] + "n");
            int angle = int.Parse(com[1]);
         //  Debug.Log(Motor_Number + " Command = " + com[1]);
            StartCoroutine(servoSend(Motor_Number, angle));
        }
    }


    IEnumerator servoSend(int ser_no, int angle)
    {
        GameObject active_servo = servos[ser_no];

        var x = active_servo.transform.localEulerAngles.x;
        var y = active_servo.transform.localEulerAngles.y;
        var z = active_servo.transform.localEulerAngles.z;

        if (ser_no == 0)
        {
            if (angle >= 180)
            {
                angle = 179;
            }
            if (angle < 0)
            {
                angle = 0;
            }
            Debug.Log(angle);
            Quaternion target = first * Quaternion.Euler(0, 0, -1 * angle);
            float amountRotated = 0; //keep track of the amount we have rotated
            var diff = Mathf.Abs(store_ang - angle);
            Debug.Log(diff*2);
            while (amountRotated <= diff*2)
            {
                active_servo.transform.localRotation = Quaternion.RotateTowards(active_servo.transform.localRotation, target, speed);
                amountRotated += speed; //the 3rd parameter is the degrees change, so we can keep track of the amount of rotation by adding it each time
                speed = Mathf.Lerp(speed, minSpeed, .3f); //slowly reduce the speed, so the rotation has a more natural feel, slowing it down towards the end
                yield return null;
            }
            store_ang = angle;
        }

        else
        {
            if (angle >= 180)
            {
                angle = 179;
            }
            if (angle < 0)
            {
                angle = 0;
            }
            Quaternion target;

            if (ser_no == 1)
            {
                target = second * Quaternion.Euler(angle, 0, 0); //store the target angle we are trying to reach
                float amountRotated = 0; //keep track of the amount we have rotated
                var diff = Mathf.Abs(active_servo.transform.localEulerAngles.x - angle);
                while (amountRotated <= diff*2)
                {
                    active_servo.transform.localRotation = Quaternion.RotateTowards(active_servo.transform.localRotation, target, speed);
                    amountRotated += speed; //the 3rd parameter is the degrees change, so we can keep track of the amount of rotation by adding it each time
                    speed = Mathf.Lerp(speed, minSpeed, .3f); //slowly reduce the speed, so the rotation has a more natural feel, slowing it down towards the end
                    yield return null;
                }
            }
            else
            {
                target = third * Quaternion.Euler(angle,0,0); //store the target angle we are trying to reach
                float amountRotated = 0; //keep track of the amount we have rotated
                var diff = Mathf.Abs(active_servo.transform.localEulerAngles.x - angle);
                while (amountRotated <= diff*2)
                {
                    active_servo.transform.localRotation = Quaternion.RotateTowards(active_servo.transform.localRotation, target, speed);
                    amountRotated += speed; //the 3rd parameter is the degrees change, so we can keep track of the amount of rotation by adding it each time
                    speed = Mathf.Lerp(speed, minSpeed, .3f); //slowly reduce the speed, so the rotation has a more natural feel, slowing it down towards the end
                    yield return null;
                }
            }


        }

        ExecuteOnMainThread.RunOnMainThread.Enqueue(() => {

            i++;
            if (loop) {
                i = i % length_com;
                Debug.Log("loop " + i);
            }
            Debug.Log("command : " + i);
            moveServo(i);

        });
        yield return null;

    }

    public void Refresh()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}

