using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuUiManager : MonoBehaviour
{
    public static MenuUiManager instance;

    public bool is_showing_lobby = false, is_showing_enter_random_room_menu = false, is_showing_searching_room_menu = false;
    public bool is_showing_connection_error_menu = false;
    public bool is_showing_options_menu = false;
    public bool is_showing_customization_menu = false;

    public TextMeshProUGUI nick_text;
    public GameObject fiar_lobby, random_room_menu, searching_room_menu;
    public GameObject connection_error_menu;
    public GameObject options_menu;
    public GameObject customization_menu;
    
    public GameObject player_preview;

    void Awake()
    {

        instance = this;
        nick_text.text = DataManager.instance.player_nick;

        player_preview.GetComponent<Appearance>().applyAppearance(DataManager.instance.appearance);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (is_showing_searching_room_menu)
            {

                // ...

            }
            else if (is_showing_enter_random_room_menu)
            {

                randomRoomMenuCloseButton();

            }
            else if (is_showing_lobby)
            {

                lobbyCloseButton();

            }
            else if (is_showing_connection_error_menu)
            {

                connectionErrorCloseButton();

            }
            else if (is_showing_options_menu)
            {

                optionsCloseButton();

            }
            else if (is_showing_customization_menu)
            {

                customizationCloseButton();

            }

        }

    }

    public void menuPlayButton()
    {

        NetworkManager.instance.Connect(6);

    }

    public void showFiarLobby()
    {

        fiar_lobby.SetActive(true);
        is_showing_lobby = true;

        Debug.Log("ERA PRA TA TRUE, MAS TA: " + is_showing_lobby);

    }

    public void lobbyCloseButton()
    {

        NetworkManager.instance.Disconnect();

        fiar_lobby.SetActive(false);
        is_showing_lobby = false;

    }

    public void lobbyPlayButton()
    {

        NetworkManager.instance.FiarEnterPublicQueue();

        showSeachingRandomMenuButton();

    }

    public void showRandomRoomMenuButton()
    {

        is_showing_enter_random_room_menu = true;
        random_room_menu.SetActive(true);

    }

    public void randomRoomMenuCloseButton()
    {

        is_showing_enter_random_room_menu = false;
        random_room_menu.SetActive(false);

    }

    public void showSeachingRandomMenuButton()
    {

        is_showing_searching_room_menu = true;
        searching_room_menu.SetActive(true);

    }

    public void seachingRandomRoomMenuCloseButton()
    {

        is_showing_searching_room_menu = false;
        searching_room_menu.SetActive(false);

    }

    public void showConnectionError()
    {

        is_showing_connection_error_menu = true;
        connection_error_menu.SetActive(true);

    }
    public void connectionErrorCloseButton()
    {

        is_showing_connection_error_menu = false;
        connection_error_menu.SetActive(false);

    }

    public void showOptions()
    {

        is_showing_options_menu = true;
        options_menu.SetActive(true);

    }
    public void optionsCloseButton()
    {

        is_showing_options_menu = false;
        options_menu.SetActive(false);

    }

    public void showCustomization()
    {

        is_showing_customization_menu = true;
        customization_menu.SetActive(true);

    }
    public void customizationCloseButton()
    {

        is_showing_customization_menu = false;
        customization_menu.SetActive(false);

        StartCoroutine(DataManager.instance.updateAppearanceInServer());

    }

    public void logOutButton()
    {

        GameManager.instance.logOut();

    }



}
