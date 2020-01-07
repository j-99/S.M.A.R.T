/**************************************************************************************
 
 ** Developed by Team LemonSky **
 ** Hack Your Reality Hackathon 2019 **
 
**************************************************************************************/

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Ping1 : MonoBehaviour
{
    Socket socket;
    int angle;
    int servo_no;
    int i = 0;
    public Text infoText;
    public Text dataText;
    public Visualize vis;

    void Start()
    {
        if (socket == null)
        {
            Debug.Log("Attempting connection");
            infoText.text = "Attempting connection";

            socket = IO.Socket("SERVER_NAME");
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Debug.Log("connected");
                var str = "connected";
                lol(str);
            });
        }

        StartCoroutine("socketRead");
    }

    private void OnDestroy()
    {
        if (socket != null)
        {
            socket.Disconnect();
            socket = null;
        }
    }

    struct Servo
    {
        public float x;
        public float y;
        public float z;
    }

    struct SerData
    {
        public string com;
    }

    public void lol(string str){
        ExecuteOnMainThread.RunOnMainThread.Enqueue(() => {

            infoText.text = str;
            //Debug.Log("Works!");
            // Code here will be called in the main thread...

        });

    }

    public void lol1(string str)
    {
        ExecuteOnMainThread.RunOnMainThread.Enqueue(() => {

            dataText.text = str;
            //Debug.Log("Works!");
            // Code here will be called in the main thread...

        });

    }

    public void callSend()
    {
        ExecuteOnMainThread.RunOnMainThread.Enqueue(() => {
            i++;
            sendCom(i);
            // Code here will be called in the main thread...
        });
    }


    public void sendCom(int boo)
    {
        string command = vis.Send(boo);
        i = boo;
        if (command.Length > 2)
        {
            var ctx = new SerData() { com = command };
            socket.Emit("command", JObject.FromObject(ctx));
        }

    }

    IEnumerator socketRead(){
		socket.On("servo", (data) =>
        {
            var servo = (data as JObject).ToObject<Servo>();
           //Debug.Log("Recieved x: " + servo.x + " , y: " + servo.y + " , z: " + servo.z);
            int[] angles = { Mathf.RoundToInt(servo.x), Mathf.RoundToInt(servo.y), Mathf.RoundToInt(servo.z)};
            var str2 = "Recieved Servo 0: " + servo.x.ToString() + " , Servo 1: " + servo.y.ToString() + " , Servo 2: " + servo.z.ToString();
            var str3 = servo.x.ToString() + ";" + servo.y.ToString() + ";" + servo.z.ToString() +";";
            lol(str2);
            lol1(str3);
        });
		yield return null;
	}
    public void stopMon()
    {
        var ctx = new SerData() { com = "kill" };
        socket.Emit("command", JObject.FromObject(ctx));
    }

}
