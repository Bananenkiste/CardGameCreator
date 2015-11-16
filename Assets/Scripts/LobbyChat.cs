using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyChat : MonoBehaviour 
{
    public Text chatwindow;


    public void OnEnable()
    {
        Chat.delegates += RecieveMessage;
    }

    public void OnDisable()
    {
        Chat.delegates -= RecieveMessage;
    }

    public void RecieveMessage(string message)
    {
        chatwindow.text += "\n" + message;
    }

}
