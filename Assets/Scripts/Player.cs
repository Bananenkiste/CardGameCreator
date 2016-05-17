using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player
{
    private string name = "Unnamed";
    public string Name 
    { 
        get { return name; }
        set { name = value; }
    }


    private GUIHandManager handManager;
    private List<Found> resources = new List<Found>();
    private List<Card> deck = new List<Card>();
    private List<Card> graveyard = new List<Card>();
    private List<Card> active = new List<Card>();
    private List<Card> hand = new List<Card>();
    private bool turnActive = false;
    private NetworkPlayer netInfo;

    public NetworkPlayer NetInfo { get { return netInfo; } }

    public Player()
    {
        handManager = Object.FindObjectOfType<GUIHandManager>();
        if (handManager == null)
        { Debug.LogError("No HandManager Found!"); }
    }

    public Player(NetworkPlayer data)
    {
        netInfo = data;
        handManager = Object.FindObjectOfType<GUIHandManager>();
        if (handManager == null)
        { Debug.LogError("No HandManager Found!"); }
    }


    public void StartTurn()
    {
        turnActive = true;
        for (int i = 0; i < active.Count; ++i)
        {
            PlayCard(active[i]);
        }
        graveyard.AddRange(active);
        active.Clear();
        AddFounds(FoundsType.Action, 1);
        AddFounds(FoundsType.Buy, 1);
    }

    public void Init(List<Card> startDeck)
    {
        int i;
        while (startDeck.Count > 0)
        {
            i = Random.Range(0, startDeck.Count - 1);
            deck.Add(startDeck[i]);
            startDeck.RemoveAt(i);
        }
        DrawCard(5);
    }
    
    public bool BuyCard(List<Found> cardcosts)
    { 
        if(FindResource(FoundsType.Buy,1))
        {
            for (int i = 0; i < cardcosts.Count; ++i)
            { 
                if(!FindResource(cardcosts[i].type,cardcosts[i].value))
                {
                    Debug.Log("Missing: " + cardcosts[i].type);
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    public void ReduceFounds(List<Found> cardcosts)
    {
        for (int i = 0; i < cardcosts.Count; ++i)
        {
            RemoveFounds(cardcosts[i].type, cardcosts[i].value);
        }
    }


    public bool FindResource(FoundsType t, int value)
    {
        Debug.Log("Checking founds");
        for (int i = 0; i < resources.Count; ++i)
        {
            if (resources[i].type == t)
            {
                if (resources[i].value >= value)
                {
                    Debug.Log("enough: " + resources[i].type + "  -- " + resources[i].value + " / " + value);
                    return true;
                }
                else
                {
                    Debug.Log("not enough: " + resources[i].type + "  -- " + resources[i].value + " / " + value);
                    return false;
                }
            }
        }
        return false;
    }


    public bool TryPlayCard(Card c)
    {
        if (turnActive && FindResource(FoundsType.Action,1) && (c.action.Count>0 || c.delay.Count>0))
        {
            RemoveFounds(FoundsType.Action, 1);
            hand.Remove(c);
            if (c.delay.Count > 0)
            {
                active.Add(c);
            }
            else
            {
                graveyard.Add(c);
            }
            return true;
            //Game.Instance.PlayCard(c);
        }
        return false;
    }

    public void PlayHandCard(Card c)
    {
        PlayCard(c);
        RemoveCard(c);
        graveyard.Add(c);
    }

    public void PlayCard(Card c)
    {
        for (int i = 0; i < c.action.Count; ++i)
        {
            if (c.action[i].target == PlayerTarget.self)
            {
                DoAction(c.action[i]);
            }
        }
        
    }

    public void DoAction(Action action)
    {
        switch (action.type)
        {
            case ActionType.AddBuy:
            {
                AddFounds(FoundsType.Buy, action.value);
                break;
            }
            case ActionType.AddGold:
            {
                AddFounds(FoundsType.Gold, action.value);
                break;
            }
            case ActionType.DrawCard:
            {
                DrawCard(action.value);
                break;
            }
            case ActionType.AddAction:
            {
                AddFounds(FoundsType.Action, action.value);
                break;
            }
        }
    }


    public void AddCard(Card c)
    {
        graveyard.Add(c);
    }

    public void DropCard(Card c)
    {
        for (int i = 0; i < c.produce.Count; ++i)
        {
            RemoveFounds(c.produce[i].type, c.produce[i].value);
        }
        hand.Remove(c);
        handManager.RemoveCard(c);
        graveyard.Add(c);
    }

    public void RemoveCard(Card c)
    {
        for (int i = 0; i < c.produce.Count; ++i)
        {
            RemoveFounds(c.produce[i].type, c.produce[i].value);
        }
        hand.Remove(c);
        handManager.RemoveCard(c);
    }


    public void AddFounds(FoundsType type,int value)
    {
        for (int i = 0; i < resources.Count; ++i)
        {
            if (resources[i].type == type)
            {
                resources[i].value += value;
                return;
            }
        }
        resources.Add(new Found(type, value));
    }

    public void RemoveFounds(FoundsType type, int value)
    {
        for (int i = 0; i < resources.Count; ++i)
        {
            if (resources[i].type == type)
            {
                resources[i].value -= value;
                return;
            }
        }
    }

    public void EndTurn()
    {
        if (turnActive)
        {
            turnActive = false;
            graveyard.AddRange(hand);
            hand.Clear();
            resources.Clear();
            //Game.Instance.EndMyTurn();
            DrawCard(5);
        }
        
    }

    public void DrawCard(int cards)
    {
        if(cards <= 0)
        {
            return;
        }
        for(int i=0;i < cards;++i)
        {
            DrawCard();
        }
    }


    public void DrawCard()
    {
        if (deck.Count < 1)
        {
            if (graveyard.Count >= 1)
            {
                Shuffle();
            }
            else
            {
                return;
            }
        }
        hand.Add(deck[0]);
        handManager.AddCard(deck[0]);
        for (int i = 0; i < deck[0].produce.Count; ++i)
        {
            //if (deck[0].produce[i].type != FoundsType.Point && deck[0].produce[i].type != FoundsType.None)
            //{
                AddFounds(deck[0].produce[i].type, deck[0].produce[i].value);
            //}
        }
        deck.RemoveAt(0);
    }

    public void Shuffle()
    {
        int a = 0;
        while (graveyard.Count > 0)
        {
            a = Random.Range(0, graveyard.Count-1);
            deck.Add(graveyard[a]);
            graveyard.RemoveAt(a);
        }
        graveyard.Clear();
    }

}
