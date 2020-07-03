using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public string player_id, player_nick;
    public int appearance;
    public string[] response_text;
    public bool has_login = false, is_logged_in = false;
    public TMP_InputField text_login, text_password;
    public GameObject menu_login;

    void Awake()
    {

        instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("user_infos"))
        {

        }
    }

    // Update is called once per frame

    public void LoginButton()
    {

        PlayerPrefs.SetString("user_infos", text_login.text + " " + text_password.text);

        StopCoroutine(VerifyRegistration());
        StartCoroutine(VerifyRegistration());

    }

    public IEnumerator VerifyRegistration()
    {

        Debug.Log("-- Checking login");

        GameManager.instance.loading_alert.SetActive(true);

        if (PlayerPrefs.HasKey("user_infos"))
        {
            Debug.Log("- tem login");

            string json_text_ = "{ \"login\": \"" + PlayerPrefs.GetString("user_infos").Split(' ')[0] + "\", \"password\": \"" + PlayerPrefs.GetString("user_infos").Split(' ')[1] + "\" }";
            byte[] json_ = new UTF8Encoding().GetBytes(json_text_);

            UnityWebRequest req_ = UnityWebRequest.Post(GameManager.instance.api_url + "/auth/login", new WWWForm());

            req_.uploadHandler = (UploadHandler)new UploadHandlerRaw(json_);
            req_.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            req_.SetRequestHeader("Content-Type", "application/json");

            yield return req_.SendWebRequest();

            if (req_.isNetworkError || req_.isHttpError)
            {

                Debug.Log("Connection erro: " + req_.error);

            }
            else
            {

                response_text = req_.downloadHandler.text.Split(',');

                if (response_text[0] == "1")
                {

                    // Successful login

                    menu_login.SetActive(false);

                    player_id = response_text[1];

                    player_nick = response_text[2];

                    appearance = int.Parse(response_text[3]);

                    is_logged_in = true;

                }
                else
                {

                    // Invalid username or password
                    Debug.Log("--- Invalid username or password, deleting user informations");
                    PlayerPrefs.DeleteKey("user_infos");
                    menu_login.SetActive(true);

                }

            }

        }
        else
        {

            // no saved login information
            Debug.Log("- sem login");
            menu_login.SetActive(true);

        }

        GameManager.instance.loading_alert.SetActive(false);
    }

    public IEnumerator updateAppearanceInServer()
    {

        Debug.Log("- tem login");

        string json_text_ = "{ \"user_id\": \"" + player_id + "\", \"appearance\": \"" + appearance + "\" }";
        byte[] json_ = new UTF8Encoding().GetBytes(json_text_);

        UnityWebRequest req_ = UnityWebRequest.Post(GameManager.instance.api_url + "/user/setappearance", new WWWForm());

        req_.uploadHandler = (UploadHandler)new UploadHandlerRaw(json_);
        req_.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req_.SetRequestHeader("Content-Type", "application/json");

        yield return req_.SendWebRequest();

        if (req_.isNetworkError || req_.isHttpError)
        {

            Debug.Log("Connection erro: " + req_.error);

        }else{

            response_text = req_.downloadHandler.text.Split(',');

            if(response_text[0] == "1"){

                Debug.Log("- Appearance has been updatede in sever!");

            }else{

                Debug.Log("--- ERROR in server appearance update!");

            }
        }

    }

    public bool IsLoggedIn()
    {

        return is_logged_in;

    }


    public bool hasItemInAppearance(int id_, int appearance_)
    {

        return (appearance_ & (1 << id_ - 1)) != 0;

    }

    public int putItemInAppearance(int id_, ref int appearance_)
    {

        ItemType item_type_ = ItemsDataBase.instance.getItemById(id_).type;

        foreach (Item item_ in ItemsDataBase.instance.itens)
        {

            if (item_ != null)
            {

                if (item_.type == item_type_)
                {

                    removeItemInAppearance(item_.id, ref appearance_);

                }

            }

        }

        appearance_ |= (1 << id_ - 1);

        return appearance_;

    }

    public void removeItemInAppearance(int id_, ref int appearance_)
    {
        appearance_ &= ~(1 << id_ - 1);
    }
}
