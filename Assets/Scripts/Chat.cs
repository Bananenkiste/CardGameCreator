using UnityEngine;
using System.Collections;

public class Chat : MonoBehaviour 
{
    public static Chat instance = null;

    public delegate void PrintMessage(string message);
    public static PrintMessage delegates;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(this.gameObject);    
    }


    [RPC]
    public void RecieveMessage(string message)
    {
        Debug.Log("Recieved Message: "+message);
        if (delegates != null)
        {
            delegates(message);
        }
    }

    [RPC]
    public void SendChatMessage(string message)
    {
        Debug.Log("Outgoing Message");
        if (Network.isServer)
        {
            networkView.RPC("RecieveMessage", RPCMode.All, message);
        }
        else
        {
            networkView.RPC("SendChatMessage", RPCMode.Server, message);
        }
    
    }
	
}
