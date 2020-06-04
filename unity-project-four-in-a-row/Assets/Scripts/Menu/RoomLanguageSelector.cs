using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomLanguageSelector : MonoBehaviour
{
    public static RoomLanguageSelector instance;

    public Language[] languages;
    public string output_text;
    public GameObject[] buttons;


    void Awake(){

        instance = this;

    }

    void Start(){

        RenderButtonsCollors();
        RenderOutputText();

    }

    public void LangButton(int index_){

        if(languages[index_].is_selected){

            if(CanDeselect(index_)){

                languages[index_].is_selected = false;

            }

        }else{

            languages[index_].is_selected = true;

        }

        RenderButtonsCollors();
        RenderOutputText();

    }

    bool CanDeselect(int index_){

        bool can = false;

        foreach(Language lang_ in languages){

            if(lang_.index != index_){

                if(lang_.is_selected){

                    can = true;

                }

            }

        }

        return can;
    }

    void RenderButtonsCollors(){

        for(int i_ = 0; i_ < languages.Length; i_++){

            if(languages[i_].is_selected){

                buttons[i_].GetComponent<Image>().color = new Color(0.06f,0.8f,0);

            }else{

                buttons[i_].GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f);

            }

        }

    }

    void RenderOutputText(){

        output_text = "";

        for(int i_ = 0; i_ < languages.Length; i_++){

            if(languages[i_].is_selected){

                if(output_text != ""){

                    output_text += ",";

                }

                output_text += languages[i_].name;

            }

        }

    }
}