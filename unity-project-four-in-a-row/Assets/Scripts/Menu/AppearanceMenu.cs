using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppearanceMenu : MonoBehaviour
{

    public GameObject player_preview, button_prefab, skins_list, items_list;
    

    void Start()
    {

        foreach (Item item_ in ItemsDataBase.instance.itens)
        {

            if (item_ != null)
            {

                GameObject inst_ = null;

                if (item_.type == ItemType.skin)
                {

                    inst_ = Instantiate(button_prefab, skins_list.transform);


                }

                if (item_.type == ItemType.item)
                {

                    inst_ = Instantiate(button_prefab, items_list.transform);

                }

                inst_.GetComponent<Button>().onClick.AddListener(() => itemButton(item_.id));

            }

        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void itemButton(int id_)
    {

        player_preview.GetComponent<Appearance>().applyItem(ItemsDataBase.instance.getItemById(id_));
        DataManager.instance.appearance = DataManager.instance.putItemInAppearance(id_, ref DataManager.instance.appearance);

    }
}
