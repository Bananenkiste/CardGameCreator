using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public static class Settings 
{
    private static float mVolume = 0.0f;
    private static float sVolume = 0.0f;
    private static Resolution screenResolution = Screen.currentResolution;
    private static bool screenMode = true;
    private static string playername = "Unnamed";

    public static string playerName
    {
        get { return playername; }
        set { playername = value; }
    }

    public static float musicVolume
    {
        get { return mVolume; }
        set { mVolume = value; }
    }

    public static float soundVolume
    {
        get { return sVolume; }
        set { sVolume = value; }
    }

    public static Resolution resolution
    {
        get { return Screen.currentResolution; }
    }

    public static void SetResolution(Resolution res,bool fullscreen = true)
    {
        if (res.height != screenResolution.height || res.width != screenResolution.width || fullscreen != screenMode)
        {
            screenResolution = res;
            screenMode = fullscreen;

            Screen.SetResolution(res.width, res.height, fullscreen);
        }
    }

    public static Resolution[] GetResolutions()
    {
        return Screen.resolutions;
    }

    public static void Save()
    {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.IndentChars = "\t";
        settings.NewLineOnAttributes = true;

        //Debug.Log(Application.persistentDataPath + "/Settings.xml");

        //XmlDocument data = new XmlDocument();
        XmlWriter writer = XmlWriter.Create(Application.persistentDataPath+"/Settings.xml", settings);
        writer.WriteStartDocument();
        /*
        writer.WriteStartElement("Resolution");
        writer.WriteAttributeString("Height", screenResolution.height.ToString());
        writer.WriteAttributeString("Width", screenResolution.width.ToString());
        writer.WriteAttributeString("Fullscreen", screenMode.ToString());
        writer.WriteEndElement();
        */
        writer.WriteStartElement("Audio");
        writer.WriteAttributeString("sound", soundVolume.ToString());
        writer.WriteAttributeString("music", musicVolume.ToString());
        writer.WriteEndElement();
        
        writer.WriteEndDocument();
        writer.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/Settings.xml"))
        {
            MemoryStream mem = new MemoryStream(File.ReadAllBytes(Application.persistentDataPath + "/Settings.xml"));

            XmlReader reader = XmlReader.Create(mem);


            reader.MoveToContent();
            for (int i = 0; i < reader.AttributeCount; ++i)
            {
                reader.MoveToAttribute(i);
                switch (reader.Name)
                {
                    case "music":
                        {
                            musicVolume = reader.ReadContentAsFloat();
                            break;
                        }
                    case "sound":
                        {
                            soundVolume = reader.ReadContentAsFloat();
                            break;
                        }
                    default:
                        {
                            Debug.LogWarning("Unhandled Xml Node"+reader.Name+" - "+reader.ReadContentAsString());
                            break;
                        }
                
                }
            }
        }
		else
		{
			musicVolume = 0.6f;
			soundVolume = 1.0f;
		}
    }
}
