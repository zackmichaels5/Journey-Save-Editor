using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Symbol_Selector : MonoBehaviour
{
    public Save_Edit edit;

    public Material symbolmat;

    public TextMeshProUGUI symbolText;

    private int id = 0;

    // Start is called before the first frame update
    void Start()
    {
        Startup();
    }

    //Get symbol from save file
    public void Startup()
    {
        id = edit.GetBytes(12);
        UpdateMat();
    }

    // Update is called once per frame
    void Update()
    {
        //Update symbol id display text
        symbolText.text = "Symbol " + id;
    }

    //Update Symbol Material
    void UpdateMat()
    {
        symbolmat.SetFloat("Index", id);
    }

    public void NextSymbol()
    {
        id++;
        if (id > 20) id = 0;
        UpdateMat();
        edit.PlayClick();
    }

    public void PreviousSymbol()
    {
        id--;
        if (id < 0) id = 20;
        UpdateMat();
        edit.PlayClick();
    }

    public void SaveSymbol()
    {
        edit.WriteBytes(12, id);
        edit.PlayClick();
    }
}
