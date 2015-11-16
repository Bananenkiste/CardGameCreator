using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIPlayerInfo : MonoBehaviour 
{
    public Text Playername;
    public Text Founds;


    public void SetInfo(string name,List<Found> founds)
    {
        Playername.text = name;

        Founds.text = string.Empty;
        for (int i = 0; i < founds.Count; ++i)
        {
            Founds.text += founds[i].type + ": " + founds[i].value + "\n";
        }
    }
	
}
