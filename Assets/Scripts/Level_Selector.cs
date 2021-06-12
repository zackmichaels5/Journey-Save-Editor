using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level_Selector : MonoBehaviour
{
    private int selectedLevel = 0;
    private int currentLevel = 0;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI currentLevelText;

    public Save_Edit edit;

    public RawImage iconObj;

    public LevelData[] levels;

    // Start is called before the first frame update
    void Start()
    {
        Startup();
    }

    public void Startup()
    {
        //Load and initalize save file level info to menu
        currentLevel = edit.GetBytes(24);
        selectedLevel = currentLevel;
        iconObj.texture = levels[selectedLevel].icon;
    }

    // Update is called once per frame
    void Update()
    {
        //Update level selected text
        levelText.text = selectedLevel.ToString("00") + " - " + levels[selectedLevel].name;
        currentLevelText.text = "Current Level: " + levels[currentLevel].name;
    }

    public void SaveLevel()
    {
        edit.WriteBytes(24, selectedLevel);
        currentLevel = selectedLevel;
        edit.PlayClick();
    }

    public void NextLevel()
    {
        selectedLevel++;
        if (selectedLevel >= levels.Length) selectedLevel = 0;
        iconObj.texture = levels[selectedLevel].icon;
        edit.PlayClick();

    }

    public void PrevLevel()
    {
        selectedLevel--;
        if (selectedLevel < 0) selectedLevel = levels.Length - 1;
        iconObj.texture = levels[selectedLevel].icon;
        edit.PlayClick();

    }
}

//Level Data Class
[System.Serializable]
public class LevelData
{
    public string name;
    public Texture icon;

    LevelData(string v_name, Texture v_icon)
    {
        name = v_name;
        icon = v_icon;
    }
}
