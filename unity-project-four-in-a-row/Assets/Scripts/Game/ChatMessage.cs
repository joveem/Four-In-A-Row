using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatMessage : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI message_text;
    public string text_content;
    public int player_number;
    public bool is_temporary;

    void Start()
    {

        Debug.Log(" a aaaaaaaaaaaaaaaaaaaaaaaa ");

        message_text.text = text_content;

        StartCoroutine(ApplyContent());

    }

    public void SetContent(int player_number_, string text_content_, bool is_temporary_)
    {

        text_content = text_content_;
        player_number = player_number_;
        is_temporary = is_temporary_;

    }

    IEnumerator ApplyContent()
    {
        message_text.text = "";
        message_text.text = text_content;

        yield return new WaitForSeconds(0.1f);

        message_text.text = "";
        message_text.text = text_content;

        yield return new WaitForSeconds(0.1f);

        message_text.text = "";
        message_text.text = text_content;

        yield return new WaitForSeconds(ChatManager.instance.temporary_messages_duration - 0.2f);

        if (is_temporary)
        {

            Destroy(gameObject);
            Debug.Log("- - - T R U E");

        }
        else
        {

            Debug.Log("- - - F A L S E");

        }

    }
}
