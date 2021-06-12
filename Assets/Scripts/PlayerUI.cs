using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Windows;

public class PlayerUI : MonoBehaviour
{
    public Save_Edit edit;

    public Player_Viewer viewer;

    public string playerName;
    public long id;
    public int symbol;
    public bool lastCompanion = false;

    [Space]
    public Color lastCompColour;
    public Color notCompColour;

    private TextMeshProUGUI nameText;
    private RawImage symbolImage;
    private GameObject playerButton;

    // Start is called before the first frame update
    void Start()
    {
        //Get Components
        nameText = GetComponentInChildren<TextMeshProUGUI>();
        symbolImage = GetComponentInChildren<RawImage>();
        playerButton = GetComponentInChildren<Button>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (id == 0)
        {
            nameText.enabled = false;
            symbolImage.enabled = false;
            playerButton.SetActive(false);
        }
        else
        {
            nameText.enabled = true;
            symbolImage.enabled = true;
            playerButton.SetActive(true);
        }

    }

    public void Refresh()
    {
        //Get Components if they haven't yet been found
        if (nameText == null) nameText = GetComponentInChildren<TextMeshProUGUI>();
        if (symbolImage == null) symbolImage = GetComponentInChildren<RawImage>();
        if(playerButton == null) playerButton = GetComponentInChildren<Button>().gameObject;

        //Set player name and symbol
        nameText.text = playerName;
        symbolImage.material.SetFloat("Index", symbol);

        //Change colour if player was from last journey
        if (lastCompanion)
        {
            nameText.color = lastCompColour;
            symbolImage.material.SetColor("Color_86FCDA54", lastCompColour);
            symbolImage.material.SetFloat("Alpha", lastCompColour.a);
        }
        else
        {
            nameText.color = notCompColour;
            symbolImage.material.SetColor("Color_86FCDA54", notCompColour);
            symbolImage.material.SetFloat("Alpha", notCompColour.a);
        }
    }

    public void PlayerClicked()
    {
        //Debug.Log(id);
        GUIUtility.systemCopyBuffer = "https://steamcommunity.com/profiles/" + id.ToString();
        edit.PlayClick();

        viewer.copiedEndTime = Time.time + viewer.copiedDelay;
    }
}
