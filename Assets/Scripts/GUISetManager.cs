using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUISetManager : MonoBehaviour 
{
    Dictionary<Card, int> deck = new Dictionary<Card, int>();
    private int startCount = 0;


    public void Init(Dictionary<Card, int> set)
    {
        deck.Clear();
        deck = set;


        List<Card> cards = new List<Card>(set.Keys);
        for (int i = 0; i < cards.Count; ++i)
        { 
            GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("CardSmall"));
            obj.transform.SetParent(this.transform);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(10 + 110 * (i%7), -10 - 50*Mathf.Floor(i/7));
            obj.GetComponent<GUICard>().SetCard(cards[i]);
        }
        startCount = deck.Count;
    }


    

    public bool BuyCard(Card c)
    {
        if (deck.ContainsKey(c) && deck[c] > 0)
        {
            --deck[c];
            if (deck[c] == 0)
            {
                deck.Remove(c);
            }
            return true;
        }
        return false;
    }

    public bool IsCardAvailable(Card c)
    {
        if (deck.ContainsKey(c) && deck[c] > 0)
        {
            return true;
        }
        return false;
    }
    
    
    public bool CheckEnd()
    {
        if (deck.Count > startCount - 3)
        {
            return false;
        }
        return true;
    }
	
}
