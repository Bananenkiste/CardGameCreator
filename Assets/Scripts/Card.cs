using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Card 
{
    [XmlAttribute("Name")]
    public string name = string.Empty;
    [XmlAttribute("Image")]
    public string image = string.Empty;
    [XmlArray("Costs"), XmlArrayItem("Ressource")]
    public List<Found> costs = new List<Found>();
    [XmlArray("Production"), XmlArrayItem("Ressource")]
    public List<Found> produce = new List<Found>();
    [XmlArray("Actions"), XmlArrayItem("Action")]
    public List<Action> action = new List<Action>();
    [XmlArray("DelayedActions"), XmlArrayItem("DelayedAction")]
    public List<Action> delay = new List<Action>();
    [XmlArray("Reactions"), XmlArrayItem("Reaction")]
    public List<Action> reaction = new List<Action>();

    public Card()
    { }

    public Card(string aName, string aimage, List<Found> tCosts, List<Found> tProduce)
    {
        name = aName;
        image = aimage;
        costs = tCosts;
        produce = tProduce;
    }

    public void Play(Player p)
    { 

    }

    public void Drop(Player p)
    {

    }

    public void Dismiss(Player p)
    {

    }

    public void Reaction(Player p)
    {

    }

    public void DelayedPlay(Player p)
    {

    }

    public string Info()
    {
        string info = string.Empty;
        
        for (int i = 0; i < produce.Count; ++i)
        {
            info += string.Format("{0}"+'\n',produce[i].Info());
        }
        if (action != null && action.Count > 0)
        {
            if (produce != null && produce.Count > 0)
            {
                info += string.Format("================" + '\n');
            }
            for (int i = 0; i < action.Count; ++i)
            {
                info += string.Format("{0}" + '\n', action[i].Info());
            }
        }
        return info;
    }

}

[System.Serializable]
public class Action
{
    public PlayerTarget target;
    public ActionType type;
    public int value;
    //public List<Action> choosableActions;

    public string Info()
    {
        string info = string.Empty;

        info += "+ " + value.ToString() + " " + type.ToString();

        return info;
    }

}

[System.Serializable]
public class Found
{
    public FoundsType type;
    public int value;

    public Found()
    { }

    public Found(FoundsType t, int v)
    {
        type = t;
        value = v;
    }

    public string Info()
    {
        return value.ToString() + " " + type.ToString();
    }
}


public enum PlayerTarget
{ 
    self,
    others,
    all,
    left,
    right,
    choose,
}


public enum ActionType
{ 
    DrawCard,
    DropCard,
    DestroyCard,
    ChooseCard,
    AddGold,
    AddBuy,
    AddAction,
}


public enum FoundsType
{ 
    None,
    Gold,
    Action,
    Buy,
    Potion,
    Point,
}

public enum CardType
{ 
    Action=1,
    Money=2,
    Points=4,
    Reaction=8,
}
