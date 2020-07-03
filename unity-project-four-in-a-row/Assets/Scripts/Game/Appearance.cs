using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appearance : MonoBehaviour
{
    public GameObject skin;
    public GameObject item;

    public int app_, item_id_ = 6;

    
    public void applyAppearance(int appearance_code_)
    {

        for (int i_ = 1; i_ < 33; i_++)
        {

            if (DataManager.instance.hasItemInAppearance(i_, appearance_code_))
            {

                applyItem(ItemsDataBase.instance.getItemById(i_));

            }

        }

    }





    public void applyItem(Item item_)
    {

        changeMesh(item_, (int)item_.type == 0 ? item : skin);

    }

    void changeMesh(Item item_, GameObject destination)
    {

        if (item_.prefab != null)
        {
            Mesh meshInstance = Instantiate(item_.prefab.GetComponent<SkinnedMeshRenderer>().sharedMesh) as Mesh;
            destination.GetComponent<SkinnedMeshRenderer>().sharedMesh = meshInstance;

            destination.GetComponent<SkinnedMeshRenderer>().sharedMaterials = item_.prefab.GetComponent<SkinnedMeshRenderer>().sharedMaterials;
        }
        else
        {
            destination.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
            destination.GetComponent<SkinnedMeshRenderer>().sharedMaterials = new Material[] { };

        }

    }

    T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        var dst = destination.GetComponent(type) as T;
        if (!dst) dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst as T;
    }
}
