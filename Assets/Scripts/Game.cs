using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    private static Game instance = null;
    public static Game Instance
    {
        get { return instance; }
    }

    private Dictionary<Card, int> set = new Dictionary<Card, int>();

    public GUISetManager setManager;
    private GUIHandManager handManager;

    private List<Player> players = new List<Player>();
    private int activePlayer = 0;



    #region Initialization

    /// <summary>
    /// Makes sure there is only one Game instance in the scene
    /// </summary>
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
        handManager = Object.FindObjectOfType<GUIHandManager>();
        if (handManager == null)
        { Debug.LogError("Handmanager not found!"); }
    }


    void Start()
    {
        Player tester = new Player();
        tester.Init(Library.GetHandCards());
        players.Add(tester);

        if (setManager != null)
        { setManager.Init(Library.TestDeck()); }

        tester.StartTurn();
    }


    #endregion


    void NextPlayer(Player player)
    {

    }

    public void PlayCard(Card cardInstance)
    {
        instance.players[activePlayer].PlayHandCard(cardInstance);

        //handManager.RemoveCard(cardInstance);
    }

}