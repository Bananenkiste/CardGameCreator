using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUICard : MonoBehaviour 
{
    public Card card;

    public Text title;
    public Text info;
    public Text costs;
    public Text cardType;

    public void SetCard(Card c)
    {
        card = c;
        if (card != null)
        {
            title.text = card.name;
            info.text = card.Info();
            for (int i = 0; i < card.costs.Count; ++i)
            {
                if (card.costs[i].type == FoundsType.Gold)
                {
                    costs.text = card.costs[i].value.ToString();
                }
            }
        }
    }

    public void PlayCard()
    {
        Game.Instance.PlayCard(card);
    }

    public void BuyCard()
    {
        //Game.Instance.BuyCard(card);
    }

}
