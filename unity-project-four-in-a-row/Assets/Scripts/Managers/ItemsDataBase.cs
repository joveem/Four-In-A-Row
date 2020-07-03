using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDataBase : MonoBehaviour
{
    public static ItemsDataBase instance;

    public Item[] itens;

    // Start is called before the first frame update
    void Awake()
    {
        
        instance = this;

    }

    public Item getItemById(int id_){

        Item i_ = null;

        foreach(Item item_ in itens){

            if(item_ != null){

                if(item_.id == id_){

                    i_ = item_;
                    
                }

            }

        }

        return i_;

    }
}
