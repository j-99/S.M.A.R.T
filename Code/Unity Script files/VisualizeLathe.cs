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

public class VisualizeLathe : MonoBehaviour
{
    public GameObject textbox;
    public GameObject input_field;
    public GameObject jaw;
    public GameObject stage;
    public GameObject handle;
    private int i = 0;
    private int length_com;
    string[] Commands;
    float speed = 5f; //Speed of rotation at the start
    float minSpeed = 2.5f; //Speed that rotation will gradually lerp to
    Quaternion second;

    public void Manual()
    {
        second = handle.transform.rotation;
        Commands = null;
        string input = textbox.GetComponent<TextMeshProUGUI>().text;
        Commands = input.Split(';');
        length_com = Commands.Length - 1;
        for (i = 0; i <= length_com; i++)
        {
            moveServo(i);
        }
        
    }

    public void moveServo(int j)
    {
        string command = Commands[j];
        if (command.Length > 1)
        {
            if (command[0] == 'M')
            {
                string[] com = command.Split('M');

                //   Debug.Log("n" + com[1] + "n");
                int value = int.Parse(com[1]);
                //  Debug.Log(Motor_Number + " Command = " + com[1]);

                Debug.Log("JAW " + value);
                StartCoroutine(moveStage(value));
                StartCoroutine(moveHandle(value*10));
            }
            else
            {
                string[] com = command.Split('R');

                //   Debug.Log("n" + com[1] + "n");
                int rpm = int.Parse(com[1]);
                //  Debug.Log(Motor_Number + " Command = " + com[1]);
                StartCoroutine(jawRotate(rpm));
            }
        }
    }


    IEnumerator moveStage(int value)
    {
        GameObject active_servo = stage;

        var x = active_servo.transform.localPosition.x;
        var y = active_servo.transform.localPosition.y;
        var z = active_servo.transform.localPosition.z;


        float val = (float)value / 10f;
        //CODE TO MOVE STAGE
        while (Mathf.Abs(val + active_servo.transform.localPosition.z) > 1)
        {
            active_servo.transform.localPosition = Vector3.Lerp(active_servo.transform.localPosition, new Vector3 (x, y, (-1)*val ), Time.deltaTime * 0.5f );
            Debug.Log(active_servo.transform.localPosition.z);
            yield return null;
        }

        yield return null;
    }

    IEnumerator jawRotate(int rpm)
    {
        GameObject active_servo = jaw;

        var x = active_servo.transform.localEulerAngles.x;
        var y = active_servo.transform.localEulerAngles.y;
        var z = active_servo.transform.localEulerAngles.z;

        //RPM CODE
        while (true)
        {
            active_servo.transform.localRotation = Quaternion.Slerp(active_servo.transform.localRotation, Quaternion.Euler(x, y, z), Time.deltaTime * 3000 *rpm);
            x++;
            x = x % 360;
            yield return null;
        }
    }

    IEnumerator moveHandle(int angle)
    {
        GameObject active_servo = handle;
        Quaternion target;
        target = second * Quaternion.Euler(-1*179,0, 0); //store the target angle we are trying to reach
        float amountRotated = 0; //keep track of the amount we have rotated
        speed = angle/25;
        minSpeed = speed / 5;
        var temp = minSpeed / 10;
        while (amountRotated <= 179)
        {
            active_servo.transform.rotation = Quaternion.RotateTowards(active_servo.transform.rotation, target, speed);
            amountRotated += speed; //the 3rd parameter is the degrees change, so we can keep track of the amount of rotation by adding it each time
            speed = Mathf.Lerp(speed, minSpeed, temp); //slowly reduce the speed, so the rotation has a more natural feel, slowing it down towards the end
            yield return null;
            Debug.Log("After = " + active_servo.transform.eulerAngles.y);
        }
    }

    public void Refresh()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

