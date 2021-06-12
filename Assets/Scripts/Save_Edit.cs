using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using System.Text;
using TMPro;

public class Save_Edit : MonoBehaviour
{
    [Header("Path Settings")]
    public string path = "";

    public string steamPath = "";

    public GameObject pathInput;

    public GameObject invalidText;
    public float invalidDelay;

    public InputField pathInputField;

    public GameObject pathText;

    public float pathDelay = 5f;

    private float pathDelayTime = -1;

    [Space]
    [Header("Sub Menus")]
    public Scarf_Selector scarf;
    public Cloak_Selector cloak;
    public Level_Selector level;
    public Symbol_Selector symbol;
    public Player_Viewer viewer;

    [Space]
    [Header("Misc")]
    public AudioSource click;

    public GameObject muteText;

    public Toggle backupToggle;

    bool muted = false;

    float mutedTime = 2;

    string[] files;

    string dataPath = "";
    string steamDataPath = "";

    bool manualPath = false;

    byte[] byteArray;

    // Start is called before the first frame update
    void Start()
    {
        //Find the save file and back it up
        path = FindPath();
        if (path != "")
        {
            File.Copy(path, path + ".backup", true);
        }

        //Mute Audio Setting
        muted = PlayerPrefs.GetInt("Muted") == 1;
        if(muted) muteText.GetComponent<TextMeshProUGUI>().text = "Press M to Unmute Audio";
        else muteText.GetComponent<TextMeshProUGUI>().text = "Press M to Mute Audio";
        click.mute = muted;

        pathText.SetActive(false); //Disable path display text

    }

    // Update is called once per frame
    void Update()
    {
        //Change path keyboard shortcut
        if (Input.GetKeyDown(KeyCode.L))
        {
            pathInput.SetActive(true);
            pathInputField.text = path;
        }

        //Show current path
        if (Input.GetKeyDown(KeyCode.P))
        {
            pathText.GetComponent<TextMeshProUGUI>().text = path;
            pathText.SetActive(true);
            pathDelayTime = Time.time + pathDelay;
        }

        if (pathDelayTime < Time.time)
        {
            PathTextDisable();
        }

        //Toggle Mute
        if (Input.GetKeyDown(KeyCode.M))
        {
            muted = !muted;
            click.mute = muted;
            if (muted)
            {
                muteText.GetComponent<TextMeshProUGUI>().text = "Audio Muted";
                PlayerPrefs.SetInt("Muted", 1);
            }
            else
            {
                muteText.GetComponent<TextMeshProUGUI>().text = "Audio Unmuted";
                PlayerPrefs.SetInt("Muted", 0);
            }
            muteText.SetActive(true);
            mutedTime = Time.time + 2;
        }

        //Disable mute text after time
        if (mutedTime < Time.time)
        {
            muteText.SetActive(false);
        }
    }

    //Play button click sound
    public void PlayClick()
    {
        click.Play();
    }

    //Disable path display text
    void PathTextDisable()
    {
        pathText.SetActive(false);
    }

    //Disable path and mute text when switching menu
    public void ButtonPressed()
    {
        pathText.SetActive(false);
        muteText.SetActive(false);
    }

    //Change path button
    public void ChangePath()
    {
        pathInput.SetActive(true);
        pathInputField.text = path;
        PlayClick();
    }

    //Set path input to appdata
    public void SetInputAppdata()
    {
        pathInputField.text = dataPath;
        PlayClick();
    }

    //Set path input to steam
    public void SetInputSteam()
    {
        pathInputField.text = steamDataPath.Replace(@"\", "/");
        PlayClick();
    }

    //Set path input to saved
    public void SetInputSave()
    {
        pathInputField.text = PlayerPrefs.GetString("SavePath");
        PlayClick();
    }

    //Check if inputed path is valid
    public void InputPath()
    {
        PlayClick();
        
        if (File.Exists(pathInputField.text) && pathInputField.text != "")
        {
            path = pathInputField.text;

            if (backupToggle.isOn)
            {
                File.Copy(path, path + ".backup", true);
            }

            if (path != PlayerPrefs.GetString("SavePath") && path != dataPath && path != steamDataPath)
            {
                PlayerPrefs.SetString("SavePath", path);
            }
            pathInput.SetActive(false);
            if(manualPath) backupToggle.isOn = false;
            scarf.Startup();
            cloak.Startup();
            level.Startup();
            symbol.Startup();
            viewer.Startup();
        }
        else
        {
            invalidText.SetActive(true);
            Invoke("RemoveInvalidText", invalidDelay);
        }
    }

    //Remove invalid path text from screen
    void RemoveInvalidText()
    {
        invalidText.SetActive(false);
    }

    //Find a valid save path
    string FindPath()
    {
        //Appdata path
        dataPath = "";
        string persistentData = Application.persistentDataPath;
        int slashCount = 0;
        for (int i = 0; i < persistentData.Length; i++)
        {
            dataPath = dataPath + persistentData[i];
            if (persistentData[i] == '/') slashCount++;
            if (slashCount > 3) break;
        }

        dataPath = dataPath + "Local/Annapurna Interactive/Journey/Steam/SAVE.BIN";

        //Steam path
        steamDataPath = "";
        files = Directory.GetDirectories(steamPath);
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Length > 40) steamDataPath = files[i] + @"\638230\remote\SAVE.BIN";
        }

        //Saved path
        string savedPath = PlayerPrefs.GetString("SavePath");

        //Check which paths exist and when they were last written to
        DateTime appdataTime = new DateTime(2, 1, 1, 1, 1, 1);
        DateTime steamTime = new DateTime(2, 1, 1, 1, 1, 1);
        DateTime saveTime = new DateTime(2, 1, 1, 1, 1, 1);

        bool appdataExists = false;
        bool steamExists = false;
        bool saveExists = false;

        if (File.Exists(dataPath))
        {
            appdataTime = File.GetLastWriteTime(dataPath);
            appdataExists = true;
        }
        if (File.Exists(steamDataPath))
        {
            steamTime = File.GetLastWriteTime(steamDataPath);
            steamExists = true;
        }
        if (File.Exists(savedPath))
        {
            saveTime = File.GetLastWriteTime(savedPath);
            saveExists = true;
        }

        //Use the most recently modified path. If there is no path open the path selection menu
        if (!appdataExists && !steamExists && !saveExists)
        {
            pathInput.SetActive(true);
            pathInputField.text = PlayerPrefs.GetString("SavePath");
            backupToggle.isOn = true;
            manualPath = true;
            return "";
        }

        if (appdataTime > steamTime)
        {
            if (appdataTime > saveTime)
            {
                return dataPath;
            }
            else
            {
                return savedPath;
            }
        }
        else
        {
            if (steamTime > saveTime)
            {
                return steamDataPath;
            }
            else
            {
                return savedPath;
            }
        }

    }

    //Write bytes to save file
    public void WriteBytes(int position, int value)
    {
        using (var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
        {
            stream.Position = position;
            Debug.Log("Editing Byte " + stream.ReadByte() + " at position " + position);

            //Write int value to byte position
            stream.Position = position;
            stream.WriteByte(Convert.ToByte(value + ""));

            stream.Position = position;
            Debug.Log("Edited to " + stream.ReadByte());
        }
    }

    //Get bytes from save file
    public int GetBytes(int position)
    {
        if (File.Exists(path))
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                stream.Position = position;
                return stream.ReadByte();
            }
        }
        else
        {
            Debug.LogWarning("File doesn't exist at path " + path);
            return 0;
        }
        
    }

    public byte[] GetAllBytes()
    {
        if (File.Exists(path))
        {
            return File.ReadAllBytes(path);
        }
        else
        {
            Debug.LogWarning("File doesn't exist at path " + path);
            byte[] tempByteArray = new Byte[7000];
            Array.Clear(tempByteArray, 0, tempByteArray.Length);
            return tempByteArray;
        }
    }

    public byte[] GetAllBytes(string overridePath)
    {
        if (File.Exists(overridePath))
        {
            return File.ReadAllBytes(overridePath);
        }
        else
        {
            Debug.LogWarning("File doesn't exist at path " + overridePath);
            byte[] tempByteArray = new Byte[7000];
            Array.Clear(tempByteArray, 0, tempByteArray.Length);
            return tempByteArray;
        }
    }
}
