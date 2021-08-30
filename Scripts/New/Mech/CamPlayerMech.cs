using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPlayerMech : MonoBehaviour
{

    public Transform target;
    public Transform baseTarget;
    public Vector3 camOffset;

    public float mouseSensitivity = 8;

    Vector2 pitchMinMax = new Vector2(-30, 45);

    Vector3 currentRotation;
    float rotationSmoothTime = .1f;
	Vector3 rotationSmoothVelocity;

    /*
    float camXOffset = 1.3f;
    float camYOffset = 0.66f;
    float camZOffset = 4.3f;
    */

    float pitch;
    float yaw;

    bool camRotEnabled;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
		
        camRotEnabled = true;

    }

    void Update()
    {
        Rotate();

        Move();
    }

    void LateUpdate()
    {


    }

    void Move()
    {

        //mechCamPos.position =
        //For height
        //new Vector3(target.position.x + camXOffset, target.position.y + camYOffset, target.position.z + camZOffset);//

        transform.position =
            //For height
            new Vector3(target.position.x, baseTarget.position.y + camOffset.y, target.position.z)//
            +
            transform.right * camOffset.x
            -
            // For distance behind
            (transform.forward * camOffset.z);
            //+
            //(transform.up * camYOffset);
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

        transform.localEulerAngles = currentRotation;
    }
}
