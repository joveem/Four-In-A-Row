using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//change this values in this file
public class env
{
    //replace WITHOUT protocol ("ws://", "http://","https://", etc.) and WITHOUT slash at the end
    [HideInInspector]
    public static string local_server_url = "localhost:2929";

    //replace WITHOUT protocol ("ws://", "http://","https://", etc.), WITHOUT port number (":2929", etc.) and WITHOUT slash at the end
    [HideInInspector]
    public static string online_server_url = "EXAMPLE.COM";

}
