using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public static ChatManager instance;

    public TMP_InputField chat_input_field;
    public GameObject temporary_chat_pivot, entire_chat_pivot;
    public GameObject[] message_prefabs;

    void Awake()
    {

        instance = this;

    }

    // Update is called once per frame
    void Update()
    {

    }

    

    public void SendMessageButton(){

        if(chat_input_field.text != "" && chat_input_field.text != " "){

            NetworkManager.instance.SendMessageToServer(FourInARow.instance.player_number, chat_input_field.text);

            chat_input_field.text = "";

        }

    }

    public void AddMessage(int player_number_, string message_content_)
    {

        foreach (ChatMessage message_ in temporary_chat_pivot.transform.GetComponentsInChildren<ChatMessage>())
        {

            if (message_.player_number == player_number_)
            {

                Destroy(message_.gameObject);

            }

        }

        GameObject inst_ = Instantiate(message_prefabs[player_number_ == FourInARow.instance.player_number ? 0 : 1], new Vector3(), new Quaternion(), temporary_chat_pivot.transform);

        inst_.GetComponent<ChatMessage>().SetContent(player_number_, message_content_, true);

        GameObject inst_2_ = Instantiate(message_prefabs[player_number_ == FourInARow.instance.player_number ? 0 : 1], new Vector3(), new Quaternion(), entire_chat_pivot.transform);

        inst_.GetComponent<ChatMessage>().SetContent(player_number_, message_content_, false);
    }
}
