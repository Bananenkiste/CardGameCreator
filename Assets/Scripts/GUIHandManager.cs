using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIHandManager : MonoBehaviour 
{
    public Transform Panel;
    public Button endTurnButton;
    private List<GUICard> cards = new List<GUICard>();

    public void AddCard(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; ++i)
        {
            AddCard(cards[i]);
        }
        //Reposition();
    }
    
    public void AddCard(Card c)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Card"));
        obj.transform.SetParent(Panel);
        
        //obj.transform.localPosition = new Vector3(60f + (100*cards.Count), 0f, 0f);
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(60f + (100 * cards.Count), 0f);
        
        cards.Add(obj.GetComponent<GUICard>());
        obj.GetComponent<GUICard>().SetCard(c);
        Reposition();
    }

    public void RemoveCard(Card c)
    {
        for (int i = 0; i < cards.Count; ++i)
        {
            if (cards[i].card == c)
            {
                Destroy(cards[i].gameObject);
                cards.RemoveAt(i);
                Reposition();
                return;
            }
        }  
    }

    public void ClearPanel()
    {
        for (int i = 0; i < cards.Count; ++i)
        {
            Destroy(cards[i].gameObject);
        }
        cards.Clear();
    }

    private void Reposition()
    {
        for (int i = 0; i < cards.Count; ++i)
        {
            cards[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(60f + (100 * i), 0f);
        }
    }
}
