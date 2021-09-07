using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    bool track;

    public Transform target;
    private float rocketTurnSpeed;
    private float rocketSpeed;
    private float randomOffset;

    private float timerSinceLaunch_Contor;
    private float objectLifeTimerValue;

    // Use this for initialization
    void Start()
    {
        rocketTurnSpeed = 200.0f;
        rocketSpeed = 25f;
        randomOffset = 0.0f;

        timerSinceLaunch_Contor = 0;
        objectLifeTimerValue = 10;

        track = true;
    }

    // Update is called once per frame
    void Update()
    {
        timerSinceLaunch_Contor += Time.deltaTime;

        if (target != null)
        {
            if (timerSinceLaunch_Contor > 6)
            {
                transform.Translate(Vector3.forward * rocketSpeed * Time.deltaTime);
            }
            else if (timerSinceLaunch_Contor > 0.1)
            {
                if ((target.position - transform.position).magnitude >= 8)
                {
                    randomOffset = 50.0f;
                    rocketTurnSpeed = 400.0f;
                }
                else
                {
                    //track = false;

                    randomOffset = 3f;
                    //if close to target
                    if ((target.position - transform.position).magnitude < 4)
                    {
                        track = false;
                    }
                    else if ((target.position - transform.position).magnitude < 6)
                    {
                        rocketTurnSpeed = 50.0f;
                    }
                }

                if (track)
                {
                    Vector3 direction = target.position - transform.position + Random.insideUnitSphere * randomOffset;
                    direction.Normalize();
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rocketTurnSpeed * Time.deltaTime);
                }
                else
                    Debug.Log("Not tracking");

                transform.Translate(Vector3.forward * rocketSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.forward * rocketSpeed * Time.deltaTime);
            }

        }

        if (timerSinceLaunch_Contor > objectLifeTimerValue)
        {
            Destroy(transform.gameObject, 1);
        }

        if (Vector3.Distance(transform.position, target.transform.position) <= 1f)
        {
            target.GetComponent<HitRegister>()?.RegisterHit(2.0f);

            Destroy(gameObject);
        }
    }
}
