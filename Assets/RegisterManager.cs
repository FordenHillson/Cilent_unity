using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;


[RequireComponent(typeof(SocketIOComponent))]
public class RegisterManager : MonoBehaviour
{
    static SocketIOComponent socket;
    public InputField nameField;
    public RectTransform namePanel;
    public RectTransform lobbyPanel;
    public Button createRoom;
    public Button joinRoom;

    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("userConnect", HaveUserConnect);
        socket.On("user Disconnect", HaveUserDisconnect);
        socket.On("OnOwnerClientConnect", OnOwnerClientConnect);
        socket.On("goToLobby", HaveGotoLobby);
        namePanel.gameObject.SetActive(true);
        lobbyPanel.gameObject.SetActive(false);
    }

    void Update()
    {
           
    }

    private void HaveUserConnect(SocketIOEvent obj)
    {
        Debug.Log("User Connect to Server");
    }

    private void HaveUserDisconnect(SocketIOEvent obj)
    {
        Debug.Log("User Disconnect in Server");

    }

    private void OnOwnerClientConnect(SocketIOEvent evt)
    {
        Debug.Log("OnOwnerClientConnect : " + evt.data.ToString());
        
    }

    public void sendName()
    {
        JSONObject jSONObject = new JSONObject(JSONObject.Type.OBJECT);
        jSONObject.AddField("nameInput", nameField.text);
        socket.Emit("namePlayer", jSONObject);
        Debug.Log("sendName " + jSONObject);
        namePanel.gameObject.SetActive(false);
        lobbyPanel.gameObject.SetActive(true);
    }

    private void HaveGotoLobby(SocketIOEvent evt)
    {
        Debug.Log("Users: " + nameField + "Go to Looby");
    }

    public void newRoom()
    {
       
    }


}