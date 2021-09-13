using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private bool track = true;

    public Transform target;

    private float turnSpeed;
    private Vector2 turnMinMax;
    private float speed = 25;
    private float randomOffset;
    private Vector2 offsetMinMax;

    private float timer = 0;
    private float lifeTimer;

    public void Initialize(WeaponItem refItem, Transform target)
    {
        this.target = target;

        speed = refItem.bulletSpeed;

        turnMinMax = refItem.turnMinMax;
        offsetMinMax = refItem.offsetMinMax;
        lifeTimer = refItem.bulletLife;

        turnSpeed = turnMinMax.y;
        randomOffset = offsetMinMax.x;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (target != null && target.gameObject.activeSelf)
        {
            track = true;

            if (0.1 < timer && timer < 6)
            {
                // Extremely close, stop tracking
                if ((target.position - transform.position).magnitude < 4)
                {
                    track = false;
                }
                // Very close, turn slower
                else if ((target.position - transform.position).magnitude < 6)
                {
                    turnSpeed = turnMinMax.x;
                }
                // Close, spread reduced (for visuals mostly)
                else if ((target.position - transform.position).magnitude < 8)
                {
                    randomOffset = offsetMinMax.x;
                }
                // Not close, max turn speed & max spread
                else
                {
                    randomOffset = offsetMinMax.y;
                    turnSpeed = turnMinMax.y;
                }

                if (track)
                {
                    Vector3 direction = target.position - transform.position + Random.insideUnitSphere * randomOffset;
                    direction.Normalize();
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            Vector3 direction = transform.forward + Random.insideUnitSphere * randomOffset;
            direction.Normalize();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (timer > lifeTimer)
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
