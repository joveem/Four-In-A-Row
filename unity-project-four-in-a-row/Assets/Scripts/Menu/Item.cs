using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item_00", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class Item : ScriptableObject
{
    public int id = 0;
    public string item_name = "Item Name";
    public ItemType type = ItemType.item;
    public Sprite sprite;
    public GameObject prefab;
}

public enum ItemType{

    item,
    skin

}