using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiarUiManager : MonoBehaviour
{
    public static FiarUiManager instance;
    public bool is_showing_pause_menu = false, is_showing_left_menu = false, is_showing_entire_chat;


    public GameObject pause_menu, left_menu, entire_chat;
    void Awake()
    {

        instance = this;

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (is_showing_entire_chat)
            {

                hideChatButton();

            }
            else if (!is_showing_left_menu)
            {

                if (is_showing_pause_menu)
                {

                    hidePauseButton();

                }
                else
                {

                    pause_menu.SetActive(true);

                    is_showing_pause_menu = true;

                }

            }


        }

    }

    public void showPlayerLeftMenu()
    {

        hidePauseButton();

        is_showing_left_menu = true;

        left_menu.SetActive(true);

    }

    public void hidePlayerLeftMenu()
    {

        is_showing_left_menu = false;

        left_menu.SetActive(false);

    }

    public void showChatButton()
    {

        is_showing_entire_chat = true;

        entire_chat.SetActive(true);

    }

    public void hideChatButton()
    {

        is_showing_entire_chat = false;

        entire_chat.SetActive(false);

    }

    public void hidePauseButton(){

        is_showing_pause_menu = false;

        pause_menu.SetActive(false);

    }
}
