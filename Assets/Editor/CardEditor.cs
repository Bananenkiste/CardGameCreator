using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CardEditor : EditorWindow
{
    static CardEditor window = null;
    static Card cardToEdit = null;

    static void init()
    {
        if (window)
        {
            window.Close();
        }
        window = EditorWindow.GetWindow<CardEditor>();
    }


    public static void EditCard(Card card, int cardid)
    {
        init();
        if (card != null)
        {
            cardToEdit = card;
        }
    }

    void doWindow(int id)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Cardname: ", GUILayout.Width(200));
        cardToEdit.name = GUILayout.TextField(cardToEdit.name);
        GUILayout.EndHorizontal();
        
        GUILayout.Label("Costs: ");
        CreateFoundsList(cardToEdit.costs);      

        GUILayout.Label("Produce: ");
        CreateFoundsList(cardToEdit.produce);

        GUILayout.Label("Actions:");
        CreateActionList(cardToEdit.action);
        GUILayout.Label("Reaction:");
        CreateActionList(cardToEdit.reaction);
        GUILayout.Label("Delayed:");
        CreateActionList(cardToEdit.delay);

    }

    void CreateFoundsList(List<Found> founds)
    {
        for (int i = 0; i < founds.Count; ++i)
        {
            GUILayout.BeginHorizontal();
            founds[i].type = (FoundsType)EditorGUILayout.EnumPopup(founds[i].type);
            founds[i].value = EditorGUILayout.IntField(founds[i].value);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                founds.RemoveAt(i);
            }
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Add", GUILayout.Width(50)))
        {
            founds.Add(new Found());
        }
    }

    void CreateActionList(List<Action> actions)
    {
        for (int i = 0; i < actions.Count; ++i)
        {
            GUILayout.BeginHorizontal();
            actions[i].type = (ActionType)EditorGUILayout.EnumPopup(actions[i].type);
            actions[i].target = (PlayerTarget)EditorGUILayout.EnumPopup(actions[i].target);
            actions[i].value = EditorGUILayout.IntField(actions[i].value);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                actions.RemoveAt(i);
            }
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Add Action"))
        {
            actions.Add(new Action());
        }
    }


    void OnGUI()
    {
        if (cardToEdit != null)
        {
            BeginWindows();
            GUI.Window(0, new Rect(10, 10, window.position.width-20, window.position.height-20), doWindow, cardToEdit.name);
            EndWindows();
        }
    }
}
