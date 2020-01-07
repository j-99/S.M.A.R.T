/**************************************************************************************
 
 ** Developed by Team LemonSky **
 ** Hack Your Reality Hackathon 2019 **
 
**************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Opening_Scene : MonoBehaviour
{
    public GameObject[] robot;
    public GameObject monitor_lathe;
    public GameObject help_ui;

    public void Monitor()
    {
        SceneManager.LoadScene("m0");
    }

    public void left()
    {
        robot[0].SetActive(true);
        robot[1].SetActive(false);
        monitor_lathe.SetActive(true);
    }

    public void right()
    {
        robot[1].SetActive(true);
        robot[0].SetActive(false);
        monitor_lathe.SetActive(false);
    }
    public void Program()
    {
        if (robot[0].activeSelf)
        {
            SceneManager.LoadScene("r0");
        }
        else
        {
            SceneManager.LoadScene("r1");
        }
        
    }

    public void Back()
    {
        SceneManager.LoadScene("mm");
    }

    public void Help()
    {
        help_ui.SetActive(true);
    }

    public void closeHelp()
    {
        help_ui.SetActive(false);
    }
    public void Credits()
    {
        //SceneManager.LoadScene(4);
    }
}
