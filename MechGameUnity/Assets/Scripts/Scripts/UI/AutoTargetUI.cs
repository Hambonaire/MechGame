using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Deprecated
public class AutoTargetUI : MonoBehaviour
{
    Rigidbody rb;   // A reference to the Rigidbody attached to this object
    float moveSpeed = 5f;
    float jumpForce = 5f;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 velocity = new Vector3(0, 0, 0);
        velocity.z = Input.GetAxis("Horizontal") * moveSpeed;

        rb.AddForce(velocity);

        if (Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(Vector3.up * jumpForce);
    }

}
