using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    SocketIOComponent socket;

    public int minigame_number = 0;

    public int players_quant;

    void Awake()
    {

        instance = this;

        socket = GetComponent<SocketIOComponent>();

    }
    void Start()
    {

        socket.On("connecting", DebugSettingConnection);
        socket.On("send-connection-result-to-client", DebugConnectionResult);
        socket.On("open-game", EnterGame);
        socket.On("update-players-quantity", UpdatePlayersQuant);
        socket.On("send-position-to-client", OnSendPositionToClient);
        socket.On("send-piece-move-to-client", OnSendPieceMoveToClient);
        socket.On("send-message-to-client", OnSendMessageToClient);

    }

    public void Connect(int minigame_number_)
    {

        minigame_number = minigame_number_;

        Debug.Log("- connecting... -");

        socket.Connect();

        //GameManager.instance.loading_alert.SetActive(true);

    }

    void DebugSettingConnection(SocketIOEvent message_)
    {

        Debug.Log("- setting connection...  -");

        Dictionary<string, string> pack_ = new Dictionary<string, string>();

        pack_["player_id"] = DataManager.instance.player_id.ToString();
        pack_["minigame_number"] = minigame_number.ToString();

        socket.Emit("send-configs-to-server", new JSONObject(pack_));

    }

    void DebugConnectionResult(SocketIOEvent message_)
    {

        Dictionary<string, string> pack_ = message_.data.ToDictionary();

        Debug.Log("- connection result: ");
        Debug.Log(pack_["result"]);

        if (pack_["result"] == "succeed")
        {

            Debug.Log("- connection has succeed");

            if (minigame_number == 6)
            {

                MenuUiManager.instance.showFiarLobby();

            }

        }
        else
        {

            Debug.Log("- connection has failed (player already is connected)");
            MenuUiManager.instance.showConnectionError();

        }

        GameManager.instance.loading_alert.SetActive(false);

    }

    public void Disconnect()
    {

        minigame_number = 0;

        Debug.Log("- disconnecting... -");

        socket.Close();

    }

    public void FiarEnterPublicQueue()
    {

        Dictionary<string, string> pack_ = new Dictionary<string, string>();

        pack_["langs"] = RoomLanguageSelector.instance.output_text;

        socket.Emit("fiar-enter-random-room", new JSONObject(pack_));

    }

    public void SendPositionToServer()
    {

        Dictionary<string, string> pack_ = new Dictionary<string, string>();

        pack_["pos_x"] = FourInARow.instance.local_player.transform.position.x.ToString();
        pack_["pos_y"] = FourInARow.instance.local_player.transform.position.y.ToString();
        pack_["pos_z"] = FourInARow.instance.local_player.transform.position.z.ToString();

        pack_["rot_y"] = FourInARow.instance.local_player.transform.rotation.eulerAngles.y.ToString();

        socket.Emit("send-position-to-server", new JSONObject(pack_));

    }

    public void SendPieceMoveToServer(int column_number_, int player_number_)
    {

        Dictionary<string, string> pack_ = new Dictionary<string, string>();

        pack_["column_number"] = column_number_.ToString();
        pack_["player_number"] = player_number_.ToString();

        socket.Emit("send-piece-move-to-server", new JSONObject(pack_));

    }

    public void SendMessageToServer(int player_number_, string message)
    {

        Dictionary<string, string> pack_ = new Dictionary<string, string>();

        pack_["player_number"] = player_number_.ToString();
        pack_["message"] = message;

        socket.Emit("send-message-to-server", new JSONObject(pack_));

    }

    void EnterGame(SocketIOEvent message_)
    {

        Debug.Log("- openning game...");

        Dictionary<string, string> msg_ = message_.data.ToDictionary();

        Debug.Log(msg_["player_number"] + " - ");
        Debug.Log(msg_["player_1_nick"] + " - ");
        Debug.Log(msg_["player_2_nick"] + " - ");

        GameManager.instance.loading_alert.SetActive(false);

        GameManager.instance.LoadGame(msg_["player_number"], msg_["player_1_nick"], msg_["player_2_nick"], msg_["player_1_appearance"], msg_["player_2_appearance"]);

    }

    void UpdatePlayersQuant(SocketIOEvent message_)
    {

        Dictionary<string, string> msg_ = message_.data.ToDictionary();

        players_quant = int.Parse(msg_["players_quant"]);
        Debug.Log("- players quantity updated");

    }

    void OnSendPositionToClient(SocketIOEvent message_)
    {

        Dictionary<string, string> msg_ = message_.data.ToDictionary();

        FourInARow.instance.server_player.GetComponent<PlayerMovement>().setDestination(new Vector3(float.Parse(msg_["pos_x"]), float.Parse(msg_["pos_y"]), float.Parse(msg_["pos_z"])), Quaternion.Euler(0, float.Parse(msg_["rot_y"]), 0));

        SendPositionToServer();

    }

    void OnSendPieceMoveToClient(SocketIOEvent message_)
    {

        Dictionary<string, string> msg_ = message_.data.ToDictionary();

        FourInARow.instance.ReproducePieceMove(int.Parse(msg_["column_number"]), int.Parse(msg_["player_number"]));

    }

    void OnSendMessageToClient(SocketIOEvent message_)
    {

        Dictionary<string, string> msg_ = message_.data.ToDictionary();

        ChatManager.instance.AddMessage(int.Parse(msg_["player_number"]),msg_["message"]);

    }
}
