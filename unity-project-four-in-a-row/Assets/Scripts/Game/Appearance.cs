using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appearance : MonoBehaviour
{
    public GameObject skin;
    public GameObject cloth;

    public void mudaroupa(Item item_)
    {

        CopyComponent(item_.prefab.GetComponent<SkinnedMeshRenderer>(), (int)item_.type == 0 ? skin : cloth);

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
