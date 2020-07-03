using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FourInARow : MonoBehaviour
{
    public static FourInARow instance;

    public int player_number, moves = 0;
    public string[] table_rows = new string[6] { "0000000", "0000000", "0000000", "0000000", "0000000", "0000000" };

    public bool can_move = true, is_selecting_move;

    public Animator camera_animator_;
    public Camera cam_;
    public GameObject player_prefab, local_player, server_player, select_board_trigger, board_menu, move_pivot, piece_pivot;
    [Space(5)]
    public TextMeshPro[] board_players_usernames;
    public TextMeshProUGUI[] chat_players_usernames;
    [Space(5)]
    public GameObject[] piece_prefab;
    public GameObject[] player_spawn;
    Ray ray_;
    int layer_mask;


    void Awake()
    {

        instance = this;

        player_number = int.Parse(GameManager.instance.persistent_data.Split(',')[0]);

        board_players_usernames[0].text = GameManager.instance.persistent_data.Split(',')[1];
        board_players_usernames[1].text = GameManager.instance.persistent_data.Split(',')[2];
        LightUsernameByPlayerNumber(1);

    }
    void Start()
    {

        layer_mask = LayerMask.GetMask("Default");

        if (player_number == 1)
        {

            local_player = Instantiate(player_prefab, player_spawn[0].transform.position, player_spawn[0].transform.rotation);
            server_player = Instantiate(player_prefab, player_spawn[1].transform.position, player_spawn[1].transform.rotation);

            local_player.GetComponent<Appearance>().applyAppearance(int.Parse(GameManager.instance.persistent_data.Split(',')[3]));
            server_player.GetComponent<Appearance>().applyAppearance(int.Parse(GameManager.instance.persistent_data.Split(',')[4]));


            can_move = true;


            chat_players_usernames[0].text = GameManager.instance.persistent_data.Split(',')[1];
            chat_players_usernames[1].text = GameManager.instance.persistent_data.Split(',')[2];

        }
        else
        {

            local_player = Instantiate(player_prefab, player_spawn[1].transform.position, player_spawn[1].transform.rotation);
            server_player = Instantiate(player_prefab, player_spawn[0].transform.position, player_spawn[0].transform.rotation);

            local_player.GetComponent<Appearance>().applyAppearance(int.Parse(GameManager.instance.persistent_data.Split(',')[4]));
            server_player.GetComponent<Appearance>().applyAppearance(int.Parse(GameManager.instance.persistent_data.Split(',')[3]));


            can_move = false;

            chat_players_usernames[0].text = GameManager.instance.persistent_data.Split(',')[2];
            chat_players_usernames[1].text = GameManager.instance.persistent_data.Split(',')[1];

        }

        local_player.GetComponent<PlayerMovement>().is_local_player = true;

        local_player.GetComponent<PlayerMovement>().setDestination(local_player.transform.position, local_player.transform.rotation);
        server_player.GetComponent<PlayerMovement>().setDestination(server_player.transform.position, server_player.transform.rotation);

        NetworkManager.instance.SendPositionToServer();

    }

    // Update is called once per frame
    void Update()
    {

        if (is_selecting_move)
        {

            if (can_move)
            {

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {

                    if (!canva.instance.IsMouseOverUI())
                    {

                        ray_ = cam_.ScreenPointToRay(Input.mousePosition);

                        if (Physics.Raycast(ray_, out RaycastHit hit_, Mathf.Infinity, layer_mask))
                        {

                            if (hit_.transform.tag == "move_trigger")
                            {

                                Debug.Log("Moved selected: " + int.Parse(hit_.transform.name.Split('_')[2]));

                                if (CanSelectColumn(int.Parse(hit_.transform.name.Split('_')[2])))
                                {

                                    DoPieceMove(int.Parse(hit_.transform.name.Split('_')[2]), player_number);

                                }


                            }

                        }


                    }


                }

            }



        }
        else
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                if (!canva.instance.IsMouseOverUI())
                {

                    ray_ = cam_.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray_, out RaycastHit hit_, Mathf.Infinity, layer_mask))
                    {

                        if (hit_.transform.tag == "move_trigger")
                        {

                            lookBoard();

                        }

                        if (hit_.transform.tag == "walkable_ground")
                        {

                            local_player.GetComponent<PlayerMovement>().setDestination(hit_.point);

                        }

                    }

                }

            }

        }


    }

    public void lookBoard()
    {

        is_selecting_move = true;

        select_board_trigger.SetActive(false);
        board_menu.SetActive(true);

        camera_animator_.SetInteger("zoom", 1);

        local_player.GetComponent<PlayerMovement>().setDestination(player_spawn[player_number - 1].transform.position, player_spawn[player_number - 1].transform.rotation);

    }

    public void lookRoom()
    {

        is_selecting_move = false;

        select_board_trigger.SetActive(true);
        board_menu.SetActive(false);

        camera_animator_.SetInteger("zoom", 0);

    }

    void DoPieceMove(int column_number_, int player_number_)
    {

        StartCoroutine(makeMove(column_number_, player_number_));
        NetworkManager.instance.SendPieceMoveToServer(column_number_, player_number_);

    }

    public void ReproducePieceMove(int column_number_, int player_number_)
    {

        StartCoroutine(makeMove(column_number_, player_number_));

    }

    IEnumerator makeMove(int column_number_, int player_number_)
    {
        GameObject obj_instance_;

        LightUsernameByPlayerNumber(0);

        if (player_number == player_number_)
        {
            can_move = false;

            local_player.GetComponent<PlayerMovement>().setDestination(move_pivot.transform.position + new Vector3(0, 0, 0.2f) * column_number_, move_pivot.transform.rotation);

            yield return new WaitForSeconds(0.1f);

            local_player.GetComponent<Animator>().SetTrigger("makeMove");

            yield return new WaitForSeconds(0.7f);

            obj_instance_ = Instantiate(piece_prefab[player_number_ - 1], piece_pivot.transform.position + new Vector3(0, 0, 0.2f) * column_number_, piece_pivot.transform.rotation, piece_pivot.transform);

            int row_number_ = InsertMove(column_number_, player_number);

            yield return new WaitForSeconds(0.14f);

            obj_instance_.transform.LeanMove(obj_instance_.transform.position + new Vector3(0, -0.3f - 0.2f * row_number_), 0.2f + (0.4f / 5) * row_number_);

            local_player.GetComponent<PlayerMovement>().setDestination(player_spawn[player_number - 1].transform.position, player_spawn[player_number - 1].transform.rotation);

        }
        else
        {

            server_player.GetComponent<PlayerMovement>().setDestination(move_pivot.transform.position + new Vector3(0, 0, 0.2f) * column_number_, move_pivot.transform.rotation);

            yield return new WaitForSeconds(0.1f);

            server_player.GetComponent<Animator>().SetTrigger("makeMove");

            yield return new WaitForSeconds(0.66f);

            obj_instance_ = Instantiate(piece_prefab[player_number_ - 1], piece_pivot.transform.position + new Vector3(0, 0, 0.2f) * column_number_, piece_pivot.transform.rotation, piece_pivot.transform);

            int row_number_ = InsertMove(column_number_, player_number);

            yield return new WaitForSeconds(0.14f);

            obj_instance_.transform.LeanMove(obj_instance_.transform.position + new Vector3(0, -0.3f - 0.2f * row_number_), 0.2f + (0.4f / 5) * row_number_);

            server_player.GetComponent<PlayerMovement>().setDestination(player_spawn[player_number - 1].transform.position, player_spawn[player_number - 1].transform.rotation);

            can_move = true;

        }

        if (player_number_ == 1)
        {
            LightUsernameByPlayerNumber(2);
        }
        else
        {
            LightUsernameByPlayerNumber(1);
        }


        //GameObject obj_instance_ = Instantiate(piece_prefab[player_number_ - 1]);
    }

    bool CanSelectColumn(int column_number_)
    {
        bool can_ = false;

        foreach (string st_ in table_rows)
        {

            if (st_[column_number_] == '0')
            {

                can_ = true;

            }

        }

        return can_;
    }

    int InsertMove(int column_number_, int player_number_)
    {

        int row_number_ = -1;

        if (CanSelectColumn(column_number_))
        {

            bool moved = false;
            row_number_ = 5;


            while (!moved)
            {

                if (table_rows[row_number_][column_number_] == '0')
                {

                    table_rows[row_number_] = table_rows[row_number_].Substring(0, column_number_) + player_number_ + table_rows[row_number_].Substring(column_number_ + 1, 6 - column_number_);

                    moved = true;
                }
                else
                {

                    row_number_--;

                }

            }

        }
        else
        {

            Debug.LogError("--- CAN'T INSERT MOVE --- IN COLLUMN NUMBER " + column_number_);

        }

        return row_number_;
    }

    public void LightUsernameByPlayerNumber(int player_number_)
    {

        if (player_number_ == 0)
        {

            board_players_usernames[0].color = new Color(0.4f, 0, 0);
            board_players_usernames[1].color = new Color(0, 0, 0.4f);

        }

        if (player_number_ == 1)
        {

            board_players_usernames[0].color = new Color(1, 0, 0);
            board_players_usernames[1].color = new Color(0, 0, 0.4f);

        }

        if (player_number_ == 2)
        {

            board_players_usernames[0].color = new Color(0.4f, 0, 0);
            board_players_usernames[1].color = new Color(0, 0, 2);

        }

    }
}