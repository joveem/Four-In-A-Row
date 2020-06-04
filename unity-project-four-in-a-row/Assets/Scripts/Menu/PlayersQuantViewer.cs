using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayersQuantViewer : MonoBehaviour
{

    public TextMeshProUGUI players_quant_text;

    void Update()
    {
        players_quant_text.text = NetworkManager.instance.players_quant.ToString();
    }
}
