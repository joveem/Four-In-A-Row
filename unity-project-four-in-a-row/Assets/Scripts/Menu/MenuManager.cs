using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static MenuManager instance;

    public GameObject FiarLobby;

    void Awake(){
    
        instance = this;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButton(){

        NetworkManager.instance.Connect(6);

    }

    public void LobbyPlayButton(){

        NetworkManager.instance.FiarEnterPublicQueue();

    }

    public void LobbyCloseButton(){

        NetworkManager.instance.Disconnect();
        FiarLobby.SetActive(false);

    }

    public void ShowFiarLobby(){

        FiarLobby.SetActive(true);

    }
}
