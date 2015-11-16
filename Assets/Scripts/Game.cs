using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
    public Player player;
    private static Game instance = null;
    public static Game Instance
    {
        get { return instance; }
    }

    public GUISetManager setManager;
    private List<NetPlayer> players = new List<NetPlayer>();
    private Dictionary<Card, int> set = new Dictionary<Card, int>();
    private int activePlayer = 0;


    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    public void OnPlayerConnected(NetworkPlayer pl)
    {
        players.Add(new NetPlayer(pl, null));
    }


    public void OnServerInitialized()
    {
        players.Add(new NetPlayer(Network.player,player,true));
    }

    public void ServerStart()
    { 
        //create Deck and send it to other players
        MemoryStream mem = new MemoryStream();
        BinaryFormatter b = new BinaryFormatter();
        b.Serialize(mem, Library.GetHandCards().ToArray());

        networkView.RPC("SetHandCards",RPCMode.AllBuffered, System.Convert.ToBase64String(mem.GetBuffer()));
        mem.Close();

        set = Library.TestDeck();
        MemoryStream mem2 = new MemoryStream();
        b.Serialize(mem2,set);

        networkView.RPC("SetDeck", RPCMode.AllBuffered, mem2.GetBuffer());
        networkView.RPC("StartGame", RPCMode.AllBuffered, null);


        activePlayer = Random.Range(0, players.Count - 1);
        NextPlayer();

    }

    
    public void PlayCard(Card c)
    {
        MemoryStream mem = new MemoryStream();
        BinaryFormatter b = new BinaryFormatter();
        b.Serialize(mem, c);

        if (player.TryPlayCard(c))
        {

            if (Network.isServer)
            {
                PlayedCard(mem.GetBuffer());
            }
            else
            {
                networkView.RPC("PlayedCard", RPCMode.Server, mem.GetBuffer());
            }
        }
    }

    [RPC]
    private void PlayedCard(byte[] data)
    {
        MemoryStream mem = new MemoryStream(data);
        BinaryFormatter b = new BinaryFormatter();
        Card card = (Card)b.Deserialize(mem);


        MemoryStream mem2 = new MemoryStream();       
        for (int i = 0; i < card.action.Count; ++i)
        {
            if (card.action[i].target == PlayerTarget.others)
            {
                b.Serialize(mem2, card.action[i]);
                networkView.RPC("ReactToCard", RPCMode.AllBuffered, data);
            }
        }
      
        if (players[activePlayer].isServer)
        {
            PlayerPlayedCard(Network.player, data);
        }
        else
        {
            networkView.RPC("PlayerPlayedCard", RPCMode.AllBuffered, players[activePlayer].netinfo, data);
        }
        
    }

    [RPC]
    private void ReactToCard(byte[] data)
    {
        MemoryStream mem = new MemoryStream(data);
        BinaryFormatter b = new BinaryFormatter();
        Action action = (Action)b.Deserialize(mem);

        player.DoAction(action);
    }

    [RPC]
    private void PlayerPlayedCard(NetworkPlayer pl, byte[] data)
    {
        MemoryStream mem = new MemoryStream(data);
        BinaryFormatter b = new BinaryFormatter();
        Card card = (Card)b.Deserialize(mem);

        if (Network.player == pl)
        {
            player.PlayHandCard(card);
        }
        else
        { 
            //do stuff
        }
    }


    [RPC]
    void SetDeck(byte[] data)
    {
        MemoryStream mem = new MemoryStream(data);
        BinaryFormatter b = new BinaryFormatter();
        Dictionary<Card,int> cards = new Dictionary<Card,int>();
        cards = (Dictionary<Card,int>)b.Deserialize(mem);
        
        setManager.Init(cards);
    }
    
    [RPC]
    void SetHandCards(string data)
    {
        MemoryStream mem = new MemoryStream(System.Convert.FromBase64String(data));
        BinaryFormatter b = new BinaryFormatter();
        List<Card> cards = new List<Card>();
        cards.AddRange((Card[])b.Deserialize(mem));

        //Debug.Log(cards);

        player.Init(cards);   
    }

    [RPC]
    void StartGame()
    { 
        //switch mode
        InterfaceManager.Instance.ChangeInterface(InterfaceType.Game);
        //Debug.LogError("Yay");
        //StartTurn();
    }

    [RPC]
    void StartTurn()
    {
        player.StartTurn();
    }

    
    void StartTurn(int player)
    {
        activePlayer = player;
        Debug.Log("active: " + activePlayer);

        if (players[activePlayer].isServer)
        {
            StartTurn();
        }
        else
        {
            networkView.RPC("StartTurn", players[activePlayer].netinfo, null);
        }      
    }

    void NextPlayer()
    {
        activePlayer++;
        if (activePlayer >= players.Count)
        { 
            activePlayer = 0;
        }
        Debug.Log("new player: " + activePlayer);
        if (!CheckEnd())
        {
            StartTurn(activePlayer);
        }
        else
        {
            networkView.RPC("EndGame", RPCMode.AllBuffered, null);
        }
    }

    public void EndMyTurn()
    {
        if (Network.isServer)
        {
            EndTurn();
        }
        else
        {
            networkView.RPC("EndTurn", RPCMode.Server);
        }
    }


    [RPC]
    void EndTurn()
    {
        if (Network.isServer)
        {
            NextPlayer();
        }
    }

    bool CheckEnd()
    {
        //return (setManager.CheckEnd());
        return false;
    }

    [RPC]
    void EndGame()
    { 
        //clean up
        //back to lobby
        InterfaceManager.Instance.ChangeInterface(InterfaceType.Menu);
    }


    
    public void BuyCard(Card c)
    {
        if (player.BuyCard(c.costs))
        {
            MemoryStream mem = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(mem, c);
            Debug.Log("buy card");
            if (Network.isServer)
            {
                CheckDeck(mem.GetBuffer());
            }
            else
            {
                networkView.RPC("CheckDeck", RPCMode.Server, mem.GetBuffer());
            }
        }
        else
        {
            Debug.Log("not enough founds");
        }
    
    }

    Card GetCardByName(string name)
    {
        List<Card> cards = new List<Card>(set.Keys);
        for (int i = 0; i < cards.Count; ++i)
        {
            if (cards[i].name == name)
            {
                return cards[i];
            }
        }
        return null;
    }


    [RPC]
    void CheckDeck(byte[] data)
    {
        MemoryStream mem = new MemoryStream(data);
        BinaryFormatter b = new BinaryFormatter();
        Card card = (Card)b.Deserialize(mem);

        Debug.Log("check card");

        Card instance = GetCardByName(card.name);

        if (instance != null && set[instance] > 0)
        {
            --set[instance];
            Debug.Log(instance.name + ": " + set[instance]);
            if (players[activePlayer].isServer)
            {
                ConfirmCardBuy(data);
            }
            else
            {
                networkView.RPC("ConfirmCardBuy", players[activePlayer].netinfo, data);
            }
            networkView.RPC("UpdateSetForAll", RPCMode.AllBuffered, data);
        }
        else
        {
            Debug.Log("Set says nope!");
        }

    }


    [RPC]
    public void ConfirmCardBuy(byte[] data)
    {
        MemoryStream mem = new MemoryStream(data);
        BinaryFormatter b = new BinaryFormatter();
        Card card = (Card)b.Deserialize(mem);

        player.ReduceFounds(card.costs);
        player.RemoveFounds(FoundsType.Buy, 1);

        player.AddCard(card);
    }

    [RPC]
    public void UpdateSetForAll(byte[] data)
    {
        MemoryStream mem = new MemoryStream(data);
        BinaryFormatter b = new BinaryFormatter();
        Card card = (Card)b.Deserialize(mem);

        setManager.BuyCard(card);
    }
}


public class NetPlayer
{
    public NetworkPlayer netinfo;
    public Player playerinfo;
    public bool isServer;

    public NetPlayer(NetworkPlayer net, Player p,bool host = false)
    {
        netinfo = net;
        playerinfo = p;
        isServer = host;
    }
}
