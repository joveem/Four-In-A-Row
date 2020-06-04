using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool is_local_player = false, IN = true;
    public float current_vel = 0, player_vel_max = 8, player_vel_acceleration = 8, player_vel_slowdown = 12, player_vel_rot_max = 720;
    public Vector3 destination_position;
    public Quaternion destination_rotation;

    Animator animator_;
    // Start is called before the first frame update
    void Start()
    {

        animator_ = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (is_local_player)
        {
            if (!FourInARow.instance.is_selecting_move)
            {

                if (Vector3.Distance(transform.position, destination_position) > 0.1f)
                {

                    transform.Translate(0, 0, player_vel_max * Time.deltaTime);

                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(destination_position - transform.position, Vector3.up), player_vel_rot_max * Time.deltaTime);
                    //transform.rotation = Quaternion.LookRotation(destination_position - transform.position, Vector3.up);

                }

            }
            else
            {

                if (Vector3.Distance(transform.position, destination_position) > 0.01f)
                {

                    transform.position = Vector3.MoveTowards(transform.position, destination_position, player_vel_max * Time.deltaTime);

                }

                if (Quaternion.Angle(transform.rotation, destination_rotation) > 0.3f)
                {

                    transform.rotation = Quaternion.RotateTowards(transform.rotation, destination_rotation, player_vel_rot_max * Time.deltaTime);

                }

            }

        }
        else
        {

            if (Vector3.Distance(transform.position, destination_position) > 0.1f)
            {

                if (Vector3.Distance(transform.position, destination_position) < 3f)
                {

                    transform.position = Vector3.MoveTowards(transform.position, destination_position, player_vel_max * Time.deltaTime);

                }
                else
                {

                    transform.position = destination_position;


                }
            }

            if (Quaternion.Angle(transform.rotation, destination_rotation) > 1f)
            {

                transform.rotation = Quaternion.RotateTowards(transform.rotation, destination_rotation, player_vel_rot_max * Time.deltaTime);

            }

        }

        if (is_local_player)
        {

            if (!FourInARow.instance.is_selecting_move)
            {

                if (Vector3.Distance(transform.position, destination_position) > 0.2f)
                {

                    current_vel += player_vel_acceleration * Time.deltaTime;

                }
                else
                {

                    current_vel -= player_vel_acceleration * Time.deltaTime;

                }

            }
            else
            {

                if (Vector3.Distance(transform.position, destination_position) > 0.025f)
                {

                    current_vel += player_vel_acceleration * Time.deltaTime;

                }
                else
                {

                    current_vel -= player_vel_acceleration * Time.deltaTime;

                }

            }

        }
        else
        {

            if (Vector3.Distance(transform.position, destination_position) > 0.2f)
            {

                current_vel += player_vel_acceleration * Time.deltaTime;

            }
            else
            {

                current_vel -= player_vel_acceleration * Time.deltaTime;

            }

        }

        current_vel = Mathf.Clamp(current_vel, 0, 1);

        animator_.SetFloat("speed", current_vel);

    }

    public void setDestination(Vector3 destination_position_)
    {

        destination_position = destination_position_;

        destination_rotation = Quaternion.LookRotation(destination_position - transform.position, Vector3.up);

    }

    public void setDestination(Vector3 destination_position_, Quaternion destination_rotation_)
    {

        destination_position = destination_position_;
        destination_rotation = destination_rotation_;

    }
}
