using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechCamera_Player : MonoBehaviour
{

    public Transform target;

    public float mouseSensitivity = 8;

    Vector2 pitchMinMax = new Vector2(-30, 45);

    Vector3 currentRotation;
    float rotationSmoothTime = .1f;
	Vector3 rotationSmoothVelocity;
    float camVerticalOffset = 1.5f;
    float pitch;
    float yaw;

    bool camRotEnabled;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
		
        camZoom = zoomMinMax.y;
        camRotEnabled = true;

        clipping = false;
        newPosition = transform.position;
        normalPosition = transform.position;
    }

    void Update()
    {
		
    }

    void LateUpdate()
    {
        Rotate();

        Move();
    }

    void Move()
    {
        transform.position =
            //For height
            (target.position + new Vector3(0, 1, 0))
            -
            // For distance behind
            (transform.forward * 8)
            +
            (transform.up * camVerticalOffset);
    }

    void Rotate()
    {
        if (camRotEnabled)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        }

        transform.eulerAngles = currentRotation;
    }
}
