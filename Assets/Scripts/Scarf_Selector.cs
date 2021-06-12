using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scarf_Selector : MonoBehaviour
{
    private int scarfLength = 0;

    public Save_Edit edit;

    public Slider slider;

    public TextMeshProUGUI scarfText;

    // Start is called before the first frame update
    void Start()
    {
        Startup();
    }

    //Load scarf info and set slider value
    public void Startup()
    {
        scarfLength = edit.GetBytes(16);
        slider.value = scarfLength;
    }

    // Update is called once per frame
    void Update()
    {
        //Update scarfLength and scarf length text
        scarfLength = (int) slider.value;
        scarfText.text = "Length: " + scarfLength;
    }

    public void SaveLength()
    {
        edit.WriteBytes(16, scarfLength);
        edit.PlayClick();
    }
}
