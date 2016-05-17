using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

public static class Library
{
    public static Card[] cards;


    public static Card GetCardByName(string name)
    {
        for (int i = 0; i < cards.Length; ++i)
        {
            if (cards[i].name == name)
            {
                return cards[i];
            }        
        }
        return null;
    }

    public static List<Card> GetHandCards()
    {
        if (cards == null)
        {
            cards = Load();
        }

        List<Card> hand = new List<Card>();
        Card c = GetCardByName("Dorf");
        if (c != null)
        {
            for (int i = 0; i < 3; ++i)
            {
                hand.Add(c);
            }
        }
        c = GetCardByName("Kupfer");
        if (c != null)
        {
            for (int i = 0; i < 7; ++i)
            {
                hand.Add(c);
            }
        }
        return hand;
    }


    public static Card[] Load()
    {
        if (File.Exists(Application.persistentDataPath + "/Library.xml"))
        {
            MemoryStream mem = new MemoryStream(File.ReadAllBytes(Application.persistentDataPath + "/Library.xml"));
            XmlSerializer serial = new XmlSerializer(typeof(Card[]));
            XmlReader reader = XmlReader.Create(mem);

            return ((Card[])serial.Deserialize(reader));
        }
        return null;
    }

    public static Dictionary<Card,int> TestDeck()
    {
        if (cards == null)
        {
            cards = Load();
        }
        Dictionary<Card, int> deck = new Dictionary<Card,int>();

        deck.Add(GetCardByName("Anwesen"), 12);
        deck.Add(GetCardByName("Herzogtum"), 12);
        deck.Add(GetCardByName("Provinz"), 12);
        deck.Add(GetCardByName("Kupfer"), 60);
        deck.Add(GetCardByName("Silber"), 60);
        deck.Add(GetCardByName("Gold"), 60);
        deck.Add(GetCardByName("Dorf"),10);
        deck.Add(GetCardByName("Burggraben"),10);
        deck.Add(GetCardByName("Laboratorium"),10);
        deck.Add(GetCardByName("Markt"),10);
        deck.Add(GetCardByName("Kapelle"), 10);

        return deck;
    }

    

	
}
