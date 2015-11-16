using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour 
{
    private static NetworkManager instance = null;

    public static NetworkManager Instance
    {
        get { return instance; }
    }

    public void Awake()
    {
        MasterServer.ClearHostList();
        NetworkManager.Request();
    }

    public static void Request()
    {
        MasterServer.RequestHostList("Dominion");
    }

    public static void CreateMasterServer(string name, string comment)
    {
        Network.InitializeServer(4, 25132, !Network.HavePublicAddress());
        MasterServer.dedicatedServer = false;
        MasterServer.RegisterHost("Dominion", name, comment);
    }

    public HostData[] FindServer()
    { 
        return MasterServer.PollHostList();
    }


    public static void ConnectToServer(HostData server)
    {
        if (server != null)
        {
            Network.Connect(server);
        }
        else
        {
            Debug.LogError("Missing Host Data");
        }
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        Debug.Log(msEvent.ToString());
    }


    void OnDestroy()
    {
        Unregister();
    }

    public static void Unregister()
    {
        Network.Disconnect();
        MasterServer.UnregisterHost();
    }




}
