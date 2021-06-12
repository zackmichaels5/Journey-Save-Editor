using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloak_Selector : MonoBehaviour
{
    private int cloak = 3;
    private bool isWhite = true;

    private int cloakID = 0;

    private bool textureChanged = true;

    public Save_Edit edit;

    public GameObject cloth;

    public Material[] cloakMats;

    // Start is called before the first frame update
    void Start()
    {
        Startup();
    }

    public void Startup()
    {
        //Load save file cloak info into menu
        cloakID = edit.GetBytes(8);
        if (cloakID > 3)
        {
            cloak = cloakID - 3;
            isWhite = true;
        }
        else
        {
            cloak = cloakID;
            isWhite = false;
        }
        textureChanged = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Change white cloak tier 1 input to tier 2
        if (isWhite && cloak == 0)
        {
            cloak = 1;
        }

        //Update cloakID (value used in save) based on if it is a white cloak and the cloak tier (cloak variable)
        if (isWhite) cloakID = cloak + 3;
        else cloakID = cloak;

        //Update cloak display if texture changed
        if (textureChanged)
        {
            textureChanged = false;
            cloth.GetComponent<MeshRenderer>().material = cloakMats[cloakID];
        }
    }

    public void NextCloak()
    {
        cloak++;
        if (cloak > 3) cloak = 0;
        textureChanged = true;
        edit.PlayClick();
    }

    public void PrevCloak()
    {
        cloak--;
        if (cloak < 0 && !isWhite) cloak = 3;
        if (cloak < 1 && isWhite) cloak = 3;
        textureChanged = true;
        edit.PlayClick();
    }

    public void ToggleWhite()
    {
        isWhite = !isWhite;
        textureChanged = true;
        edit.PlayClick();
    }

    public void SaveCloak()
    {
        edit.WriteBytes(8, cloakID);
        edit.PlayClick();
    }
}
