using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Space(30)]
    public string api_url, socket_url;

    [Space(30)]
    public string persistent_data;
    public GameObject loading_screem, loading_obj, loading_camera, loading_alert;
    public Slider loading_slider;

    void Awake()
    {

        #if UNITY_STANDALONE

                Screen.SetResolution(240, 420, false);
                Screen.fullScreen = false;
                
        #endif

        instance = this;

        //loading_obj.SetActive(false);

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

    public void LoadGame(string player_number_)
    {

        StartCoroutine(LoadGame(int.Parse(player_number_)));

    }
    IEnumerator LoadGame(int player_number_)
    {

        persistent_data = player_number_ + ",0";

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
}
