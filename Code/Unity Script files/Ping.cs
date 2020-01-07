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

public class Ping : MonoBehaviour
{
    Socket socket;
    int angle;
    int servo_no;
    int i = 0;
    public Text infoText;
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

    public void callSend()
    {
        Debug.Log("called send");
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
            string[] com = command.Split('S');
            servo_no = Mathf.RoundToInt(float.Parse(com[0]));
            angle = int.Parse(com[1]);
            Debug.Log(servo_no);
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
            var str2 = "0S" + servo.x.ToString() + " , 1S" + servo.y.ToString() + " , 2S" + servo.z.ToString();
            lol(str2);

            Debug.Log(angles[servo_no]);
            if (angles[servo_no] == angle)
            {
                callSend();
            }
        });
		yield return null;
	}

}
