using UnityEngine;
using System.Collections;

public class GuiServerList : MonoBehaviour 
{
    public Transform serverListPanel;

    void OnEnable()
    {
        CreateServerList();
    }

    public void CreateServerList()
    {
        NetworkManager.Request();
        HostData[] hosts = MasterServer.PollHostList();

        for (int i = 0; i < hosts.Length; ++i)
        {
            ServerButton btn =  ((GameObject)GameObject.Instantiate(Resources.Load("Server"))).GetComponent<ServerButton>();
            btn.gameObject.transform.SetParent(serverListPanel,true);
            btn.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -30f * i);
            btn.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ((RectTransform)serverListPanel).rect.width);
            btn.SetServer(hosts[i]);
        }
        ((RectTransform)serverListPanel).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, hosts.Length * 30f);
    }
}
