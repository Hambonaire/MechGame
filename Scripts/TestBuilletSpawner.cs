using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBuilletSpawner : MonoBehaviour
{
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SB", 0, 1);   
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SB()
    {
        var bullet = Instantiate(prefab, transform.position, transform.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 10;

        bullet.GetComponent<Bullet>().SetDamage(10);
        bullet.GetComponent<Bullet>().SetFaction(0);

        // Destroy the bullet after _ seconds
        GameObject.Destroy(bullet, 5);
    }
}
