using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Text;
using System;
using UnityEngine.UI;
using SocketIO;
using UnityEngine.SocialPlatforms;

public class Testdata : MonoBehaviour
{
    static SocketIOComponent socket; //เรียกใช้ความสามารถ socket.io
    
    public InputField nameField; //สร้าง object เป็น InputField ชื่อ nameField
    

    int score = 1; //สร้าง mock data เป็น int ชื่อ score
    
    public Button sendBtn; //ไม่มีอะไร
    // Start is called before the first frame update
    void Start()
    {
        socket = GetComponent<SocketIOComponent>(); //เริ่มต้นเรียกใช้ความสามารถ socket.io   
        socket.On("open", OnConnected); //เปิดท่อ event ชื่อ open เพื่อเตรียมรับ event และ debug ออกมาเป็น "Connected"
        socket.On("userID", idUser);    //เปิดท่อ event ชื่อ userID เพื่อเตรียมรับ event และ debug ออกมาเป็น "User has create" และ ออบเจ๊กที่มีข้อมูลของ player จากเซิฟเวอร์
    }
    IEnumerator PostUnityWebRequest() //คำสั่งยิง Post WebRequest ไปยังเซิฟเวอร์
    {

        var jsonString = "{\"score\":"+score+ "," +             //31-32 สร้างก้อนข้อมูลเป็นรูปแบบ string json
                         "\"name\":\""+nameField.text+"\"}";
       
        byte[] byteData = System.Text.Encoding.ASCII.GetBytes(jsonString.ToCharArray());                        //แปลงรูปแบบข้อมูลจาก jsonString
        UnityWebRequest unityWebRequest = new UnityWebRequest("http://localhost:8080/player/register", "POST"); //ยิง request เป็น post เพื่อสร้างข้อมูลไปยังเซิฟเวอร์    
        unityWebRequest.uploadHandler = new UploadHandlerRaw(byteData);     //ทำการอัพโหลดข้อมูล => เซิฟเวอร์
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");   
        yield return unityWebRequest.Send();        //return ข้อมูลที่ส่งไป

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)      
        {
            Debug.Log(unityWebRequest.error);
        }
        else
        {
            Debug.Log("Form upload complete! Status Code: " + unityWebRequest.responseCode);
        }

        
    }

    IEnumerator PutUnityWebRequest() //คำสั่งยิง Put WebRequest ไปยังเซิฟเวอร์
    {

        var jsonString = "{\"score\":" + score + "," +
                         "\"name\":\"" + nameField.text + "\"}";
        if (nameField.text == nameField.text) //57-61 ทดสอบหากชื่อเดียวกันให้เพิ่มค่า score
        {
            score++;

        }
        byte[] byteData = System.Text.Encoding.ASCII.GetBytes(jsonString.ToCharArray());
        UnityWebRequest unityWebRequest = new UnityWebRequest("http://localhost:8080/player/register/:id", "PUT"); //ยิง request เป็น put เพื่อสร้างข้อมูลไปยังเซิฟเวอร์ (จำเป็นต้องมี _id จาก database ถึงจะ อัพเดทข้อมูลได้)
        unityWebRequest.uploadHandler = new UploadHandlerRaw(byteData);
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");
        yield return unityWebRequest.Send();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
        }
        else
        {
            Debug.Log("Form upload complete! Status Code: " + unityWebRequest.responseCode);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void send()  //ฟังชั่นของปุ่ม ชื่อ send คุณสมบัติ เริ่มการทำงานโค้ดของ PostUnityWebRequest
    {
        StartCoroutine(PostUnityWebRequest());


    }

    void OnConnected(SocketIOEvent obj) //ฟังชั้น OnConnected สำหรับ Debug ออกมาเมื่อได้รับ event จากเซิฟเวอร์
    {
        Debug.Log("Connected");
    }

    public void update()  //ฟังชั่นของปุ่ม ชื่อ send คุณสมบัติ เริ่มการทำงานโค้ดของ  PutUnityWebRequest
    {
        StartCoroutine(PutUnityWebRequest()); 
    }
    void idUser(SocketIOEvent obj) //ฟังชั่น idUser สำหรับ debug ข้อมูลที่ได้จากเซิฟเวอร์
    {
        Debug.Log("User has create");
        Debug.Log(obj);
    }
}
