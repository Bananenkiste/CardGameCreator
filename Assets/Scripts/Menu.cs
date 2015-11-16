using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour 
{
    public Window MainMenu;
    public Window ServerList;
    public Window ServerCreator;

    public Window ServerMenu;
    public Window Lobby;
    public Window Settings;

    public Text message;

    public void OpenServerMenu()
    {
        CloseServerList();
        //ServerCreator.gameObject.SetActive(true);
        //ServerCreator.GetComponent<Animator>().Play("Intro");
        ServerCreator.Enable();
    }

    public void OpenServerList()
    {
        CloseServerMenu();
        //ServerList.gameObject.SetActive(true);
        //ServerList.GetComponent<Animator>().Play("Intro");
        ServerList.Enable();
    }

    public void CloseServerMenu()
    {
        //ServerCreator.GetComponent<Animator>().Play("Outro");
        ServerCreator.Disable();
    }

    public void CloseServerList()
    {
        //ServerList.GetComponent<Animator>().Play("Outro");
        ServerList.Disable();
    }

    public void OnConnectedToServer()
    {
        EnableChatMode();
    }

    public void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        DisableGameMode();
    }

    public void EnableChatMode()
    {
        //MainMenu.GetComponent<Animator>().Play("Outro");
        MainMenu.Disable();
        CloseServerList();
        CloseServerMenu();

        Lobby.Enable();
        ServerMenu.Enable();

        /*Lobby.gameObject.SetActive(true);
        Lobby.GetComponent<Animator>().Play("Intro");
        ServerMenu.gameObject.SetActive(true);
        ServerMenu.GetComponent<Animator>().Play("Intro");
         */
    }

    public void DisableGameMode()
    {
        /*Lobby.GetComponent<Animator>().Play("Outro");
        ServerMenu.GetComponent<Animator>().Play("Outro");
        MainMenu.GetComponent<Animator>().Play("Intro");
         */
        Lobby.Disable();
        ServerMenu.Disable();
        MainMenu.Enable();
    }

    public void CreateServer()
    {
        NetworkManager.CreateMasterServer("Server", "comming soon");
        //Game.Instance.AddSelf();
        EnableChatMode();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SendChat()
    {
        Chat.instance.SendChatMessage(message.text);
        message.text = string.Empty;
    }

    public void StartGame()
    {
        //Game.Instance.ServerStart();
    }

    public void OpenSettings()
    {
        CloseServerList();
        CloseServerMenu();
        Settings.Enable();
    }

    public void CloseSettings()
    {
        Settings.Disable();
    }

}
