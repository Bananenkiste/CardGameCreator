using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiSettings : MonoBehaviour 
{
    public InputField nickname;

    public void OnEnable()
    {
        nickname.text = Settings.playerName;
    }

    public void Apply()
    {
        if (nickname.text.Length > 0)
        {
            Settings.playerName = nickname.text;
        }
    }
	
}
