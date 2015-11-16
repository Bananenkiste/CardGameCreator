using UnityEngine;
using System.Collections;

public class InterfaceManager : MonoBehaviour 
{
    private static InterfaceManager instance = null;
    public static InterfaceManager Instance
    {
        get { return instance; }
    }
    
    public Interface[] interfaces;

    void Awake()
    {
        instance = this;
    }

    public void ChangeInterface(InterfaceType type)
    {
        for(int i = 0; i< interfaces.Length;++i)
        {
            interfaces[i].obj.SetActive(interfaces[i].type == type);
        }
    }
	
}

[System.Serializable]
public class Interface
{
    public InterfaceType type;
    public GameObject obj;
}

public enum InterfaceType
{
    Menu,
    Game,
}
