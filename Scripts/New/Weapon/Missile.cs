using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
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
            else if (timerSinceLaunch_Contor > 0.25)
            {
                if ((target.position - transform.position).magnitude >= 8)
                {
                    randomOffset = 50.0f;
                    rocketTurnSpeed = 400.0f;
                }
                else
                {
                    randomOffset = 2f;
                    //if close to target
                    if ((target.position - transform.position).magnitude < 4)
                    {
                        rocketTurnSpeed = 1000.0f;
                    }
                }

                Vector3 direction = target.position - transform.position + Random.insideUnitSphere * randomOffset;
                direction.Normalize();
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rocketTurnSpeed * Time.deltaTime);
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
            Destroy(gameObject);
        }
    }
}
