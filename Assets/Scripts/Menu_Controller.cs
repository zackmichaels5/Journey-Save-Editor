using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Controller : MonoBehaviour
{
    private int lastMenu = 0;

    private int currentMenu = 0;

    private float goalY = 0;

    public float menuYOffset = 900;

    public float moveYSpeed = 900;

    public GameObject prevButtonObj;

    public RectTransform prevTransform;

    public float prevSpeed = 200;
    public float prevBottom = -200;

    public RectTransform menu;

    public GameObject cloth;

    public float clothSpeed = 28;
    public float clothSpeedUp = 60;

    public float clothTopY = 14;

    public GameObject levelUI;
    public GameObject scarfUI;
    public GameObject cloakUI;
    public GameObject symbolUI;
    public GameObject playerUI;

    public Player_Viewer playerUIController;

    public RectTransform playerResetUI;

    public SymbolMatController smc;

    public Save_Edit edit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Change position the menu is moving to based on currentMenu
        if (currentMenu == 0)
        {
            goalY = 0;
        }
        else if (currentMenu == 1 || currentMenu == 3 || currentMenu == 5)
        {
            goalY = menuYOffset;
            if (currentMenu == 1)
            {
                levelUI.SetActive(true);
                scarfUI.SetActive(false);
                playerUI.SetActive(false);
            }
            else if (currentMenu == 3)
            {
                levelUI.SetActive(false);
                scarfUI.SetActive(true);
                playerUI.SetActive(false);
            }
            else
            {
                levelUI.SetActive(false);
                scarfUI.SetActive(false);
                playerUI.SetActive(true);
            }
        }
        else if (currentMenu == 2 || currentMenu == 4)
        {
            goalY = -menuYOffset;
            if (currentMenu == 2)
            {
                cloakUI.SetActive(true);
                symbolUI.SetActive(false);
            }
            else
            {
                cloakUI.SetActive(false);
                symbolUI.SetActive(true);
            }
        }

        //Move companion list reset when on menu 5
        if (currentMenu == 5 && menu.localPosition.y == goalY)
        {
            playerResetUI.localPosition = new Vector3(playerResetUI.localPosition.x, Mathf.Clamp(playerResetUI.localPosition.y + prevSpeed * Time.deltaTime, prevBottom, 0), playerResetUI.localPosition.z);
        }
        else
        {
            playerResetUI.localPosition = new Vector3(playerResetUI.localPosition.x, Mathf.Clamp(playerResetUI.localPosition.y - prevSpeed * Time.deltaTime * 2, prevBottom, 0), playerResetUI.localPosition.z);
        }

        //Move cloth object based on if it is menu 2
        if (currentMenu == 2)
        {
            cloth.transform.position = new Vector3(cloth.transform.position.x, Mathf.Clamp(cloth.transform.position.y - clothSpeed * Time.deltaTime, 0, clothTopY), cloth.transform.position.z);
        }
        else
        {
            cloth.transform.position = new Vector3(cloth.transform.position.x, Mathf.Clamp(cloth.transform.position.y + clothSpeedUp * Time.deltaTime, 0, clothTopY), cloth.transform.position.z);
        }

        //Move menu up or down to it's goal position
        if (menu.localPosition.y > goalY)
        {
            menu.localPosition = new Vector3(menu.localPosition.x, Mathf.Clamp(menu.localPosition.y - moveYSpeed * Time.deltaTime, goalY, menu.localPosition.y), menu.localPosition.z);
        }
        else if (menu.localPosition.y < goalY)
        {
            menu.localPosition = new Vector3(menu.localPosition.x, Mathf.Clamp(menu.localPosition.y + moveYSpeed * Time.deltaTime, menu.localPosition.y, goalY), menu.localPosition.z);
        }

        //Show symbol if it is menu 4
        if (currentMenu == 4 && menu.localPosition.y == goalY)
        {
            smc.SetAlpha(1);
        }
        else
        {
            smc.SetAlpha(0);
        }

        //Show back button when not on main menu
        if (currentMenu != 0 && menu.localPosition.y == goalY)
        {
            prevTransform.localPosition = new Vector3(prevTransform.localPosition.x, Mathf.Clamp(prevTransform.localPosition.y + prevSpeed * Time.deltaTime, prevBottom, 0), prevTransform.localPosition.z);
        }
        else
        {
            prevTransform.localPosition = new Vector3(prevTransform.localPosition.x, Mathf.Clamp(prevTransform.localPosition.y - prevSpeed * Time.deltaTime, prevBottom, 0), prevTransform.localPosition.z);
        }

        //Back to main menu with escape key
        if (Input.GetKey(KeyCode.Escape) && currentMenu != 0 && menu.localPosition.y == goalY && !playerUIController.confirmMenuActive)
        {
            PrevButton();
        }

    }

    public void LevelButton()
    {
        lastMenu = 1;
        currentMenu = 1;
        edit.ButtonPressed();
        edit.PlayClick();
    }

    public void RobeButton()
    {
        lastMenu = 2;
        currentMenu = 2;
        edit.ButtonPressed();
        edit.PlayClick();
    }

    public void ScarfButton()
    {
        lastMenu = 3;
        currentMenu = 3;
        edit.ButtonPressed();
        edit.PlayClick();
    }

    public void SymbolButton()
    {
        lastMenu = 4;
        currentMenu = 4;
        edit.ButtonPressed();
        edit.PlayClick();
    }

    public void PlayerButton()
    {
        lastMenu = 5;
        currentMenu = 5;
        edit.ButtonPressed();
        edit.PlayClick();
    }

    public void PrevButton()
    {
        if (currentMenu == 0) currentMenu = lastMenu;
        else currentMenu = 0;
        edit.ButtonPressed();
        edit.PlayClick();
    }
}

