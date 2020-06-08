using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SocketIO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Space(20)]
    public string api_url;
    public string websocket_url;
    [Space(5)]
    public bool local_host;

    [Space(30)]
    public string persistent_data;
    public GameObject loading_screem, loading_obj, loading_camera, loading_alert;
    public Slider loading_slider;

    void Awake()
    {
        instance = this;

#if UNITY_STANDALONE

        Screen.SetResolution(240, 420, false);
        Screen.fullScreen = false;

#endif

        if (local_host)
        {

            api_url = "http://" + env.local_server_url;
            websocket_url = "ws://" + env.local_server_url + "/socket.io/?EIO=4&transport=websocket";

        }
        else
        {

            api_url = "https://" + env.online_server_url;
            websocket_url = "ws://" + env.online_server_url + "/socket.io/?EIO=4&transport=websocket";

        }

    }
    void Start()
    {

        StartCoroutine(FirstLoad());

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FirstLoad()
    {

        loading_camera.SetActive(true);
        loading_screem.SetActive(true);
        loading_obj.SetActive(false);

        loading_slider.value = 0f;

        yield return new WaitForSeconds(2f);

        StartCoroutine(DataManager.instance.VerifyRegistration());

        while (!DataManager.instance.IsLoggedIn())
        {

            yield return null;

        }

        loading_obj.SetActive(true);

        AsyncOperation loading_operation_ = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        loading_slider.value = loading_operation_.progress;

        while (!loading_operation_.isDone)
        {

            loading_slider.value = loading_operation_.progress;

            yield return null;

        }

        loading_obj.SetActive(false);
        loading_screem.SetActive(false);
        loading_camera.SetActive(false);

    }

    public void LoadGame(string player_number_, string player_1_nick_, string player_2_nick_)
    {
        Debug.Log("loading game..");

        StartCoroutine(LoadGame(int.Parse(player_number_), player_1_nick_, player_2_nick_));

    }
    IEnumerator LoadGame(int player_number_, string player_1_nick_, string player_2_nick_)
    {

        Debug.Log("starting courotine...");

        persistent_data = player_number_ + "," + player_1_nick_ + "," + player_2_nick_;

        loading_camera.SetActive(true);
        loading_screem.SetActive(true);
        loading_obj.SetActive(false);

        loading_slider.value = 0f;

        //yield return new WaitForSeconds(2f);

        //StartCoroutine(DataManager.instance.VerifyRegistration());

        //loading_obj.SetActive(true);

        SceneManager.UnloadSceneAsync(1);

        AsyncOperation loading_operation_ = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);

        loading_slider.value = loading_operation_.progress;

        while (!loading_operation_.isDone)
        {

            loading_slider.value = loading_operation_.progress;

            yield return null;

        }

        loading_obj.SetActive(false);
        loading_screem.SetActive(false);
        loading_camera.SetActive(false);

    }

    public void logOut()
    {

        PlayerPrefs.DeleteKey("user_infos");
        DataManager.instance.is_logged_in = false;

        DataManager.instance.text_login.text = "";
        DataManager.instance.text_password.text = "";

        SceneManager.UnloadScene(1);

        StartCoroutine(FirstLoad());

    }

}
