using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class CardCreator : EditorWindow 
{
    static CardCreator editor = null;
    static List<Card> cards = new List<Card>();
    static List<Rect> rects = new List<Rect>();

    [MenuItem("Cards/CardEditor")]
    static void init()
    {
        if (editor)
        {
            editor.Close();
        }
        editor = EditorWindow.GetWindow<CardCreator>();
        cards.Clear();
        Card[] c = editor.Load();
        if (c != null)
        {
            cards.AddRange(c);
        }
        Reposition();
    }


    static void Reposition()
    {
        rects.Clear();
        for (int i = 0; i < cards.Count; ++i)
        {
            rects.Add(new Rect(10 + 110 * (i % 10), 10 + 110 * Mathf.FloorToInt(i / 10), 100, 100));
        }
    }
    
    void doWindow(int id)
    {
        GUI.Label(new Rect(0, 20, 100, 50), cards[id].Info());
        if (GUI.Button(new Rect(0, 70, 80, 20), "Edit"))
        {
            CardEditor.EditCard(cards[id], id);
        }
        if (GUI.Button(new Rect(80, 70, 20, 20), "X"))
        {
            cards.RemoveAt(id);
            Reposition();
        }
    }

    void EndCard(int id)
    {
        if (GUI.Button(new Rect(10, 20, 80, 20), "New Card"))
        {
            cards.Add(new Card("Unnamed", string.Empty, new List<Found>(), new List<Found>()));
            Reposition();
        }
        if (GUI.Button(new Rect(10, 40, 80, 20), "Load"))
        {
            cards.AddRange(Load());
            Reposition();
        }
        if (GUI.Button(new Rect(10, 60, 80, 20), "Save"))
        {
            Save(cards.ToArray());
        }
    }

    void OnGUI()
    {
        BeginWindows();
        for (int i = 0; i < cards.Count; ++i)
        {
            GUI.Window(i, rects[i], doWindow, cards[i].name);
        }
        GUI.Window(cards.Count, new Rect(10 + 110 * (cards.Count % 10), 10 + 110 * Mathf.FloorToInt(cards.Count / 10), 100, 100), EndCard, "Add");
        EndWindows();
    }

    public void Save(Card[] cards)
    {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.IndentChars = "\t";
        settings.NewLineOnAttributes = true;

        XmlSerializer serial = new XmlSerializer(typeof(Card[]));
        XmlWriter writer = XmlWriter.Create(Application.persistentDataPath + "/Library.xml", settings);
        writer.WriteStartDocument();

        serial.Serialize(writer, cards);

        writer.WriteEndDocument();
        writer.Close();
    }

    Card[] Load()
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

}
