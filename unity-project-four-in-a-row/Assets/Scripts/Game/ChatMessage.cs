using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatMessage : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI message_text;
    string text_content;
    public int player_number;
    public bool is_temporary;

    void Start()
    {

        message_text.text = "";

        StartCoroutine(ApplyContent());

    }

    void Update()
    {

        if (message_text.text != text_content)
        {

            message_text.text = text_content;

        }

    }

    public void SetContent(int player_number_, string text_content_, bool is_temporary_)
    {

        player_number = player_number_;
        text_content = text_content_;
        is_temporary = is_temporary_;

    }

    IEnumerator ApplyContent()
    {

        yield return new WaitForSeconds(0.5f);

        message_text.text = text_content;

        yield return new WaitForSeconds(14.5f);

        if(is_temporary){

            Destroy(gameObject);

        }
        
    }
}
