using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerButton : MonoBehaviour 
{
    public Text label;
    public Text comment;
    private HostData info;

    public void SetServer(HostData data)
    {
        info = data;

        label.text = data.gameName;
        comment.text = data.comment;
        comment.text += data.connectedPlayers + "/" + data.playerLimit;
    }

    public void Click()
    {
        NetworkManager.ConnectToServer(info);
    }
	
}
