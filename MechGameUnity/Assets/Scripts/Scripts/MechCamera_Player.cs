using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechCamera_Player : MonoBehaviour
{

    public Transform target;

    Vector3 newPosition;
    Vector3 normalPosition;

    public float mouseSensitivity = 8;

    float rotationSmoothTime = .1f;
    float zoomSmoothTime = .2f;
    float zoomSpeed = 5;
    float camZoom = 8;
    float camVerticalOffset = 3.5f;
    float pitch;
    float yaw;
    float moveSmoothTime = 3f;

    float zoomVelocity;
    float currentZoom;

    bool camRotEnabled;
    bool clipping = false;

    //public LayerMask CamOcclusion;
    //Vector3 camMask;
    //Vector3 followMask;

    Vector2 zoomMinMax = new Vector2(2, 7);
    Vector2 pitchMinMax = new Vector2(-30, 45);

    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    void Start()
    {
        //target = GameObject.FindGameObjectWithTag("Player").transform;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        camZoom = zoomMinMax.y;
        camRotEnabled = true;

        clipping = false;
        newPosition = transform.position;
        normalPosition = transform.position;
    }

    void Update()
    {
        //camZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        //camZoom = Mathf.Clamp(camZoom, zoomMinMax.x, zoomMinMax.y);
    }

    void LateUpdate()
    {
        Rotate();

        Move();

        /*
        //camZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        //camZoom = Mathf.Clamp(camZoom, zoomMinMax.x, zoomMinMax.y);
        //currentZoom = Mathf.SmoothDamp(currentZoom, camZoom, ref zoomVelocity, zoomSmoothTime);

        //Sets the camera's angle to calculated position
        

        //Sets the would-be position of the camera
        //normalPosition = target.position - transform.forward * camZoom + transform.up * camVerticalOffset * Mathf.Lerp(0, 1, (currentZoom / zoomMinMax.y));
        

        //Checks if the camera would-be clipping an object, if not then sets the camera to the would-be position
        if (!clipping)
        {
            #region Basic clipping movement
            //smoother zoom but not full up-close
            transform.position = target.position - transform.forward * camZoom + transform.up * camVerticalOffset * Mathf.Lerp(0, 1, (currentZoom / zoomMinMax.y));

            //less smooth but full up-close
            //transform.position = target.position - transform.forward * camZoom + transform.up * camVerticalOffset * Mathf.Lerp (0, 1, (currentZoom - camVerticalOffset));
            #endregion
        }
        

        //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
        //RaycastHit wallHit = new RaycastHit();

        //Checks to see if there is an object between the player and would-be position of the camera
        if (Physics.Raycast(target.position, (normalPosition - target.position).normalized, out wallHit, Vector3.Distance(this.transform.position, target.position)))
        {

            clipping = true;

            #region Non-clipping cam
            //the x and z coordinates are pushed away from the wall by hit.normal.
            //the y coordinate stays the same.
            newPosition = new Vector3(wallHit.point.x + wallHit.normal.x * 0.5f, transform.position.y, wallHit.point.z + wallHit.normal.z * 0.5f);
            #endregion

            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * moveSmoothTime);

        }
        
        else
        {
            clipping = false;
        }
        */
    }

    void Move()
    {
        transform.position =
            //For height
            (target.position + new Vector3(0, (target.gameObject.GetComponent<PlayerController>().OverallScaleFactor - 1) * 3, 0))
            -
            // For distance behind
            (transform.forward * (camZoom + Mathf.Pow(target.gameObject.GetComponent<PlayerController>().OverallScaleFactor - 1, 2f)))
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
