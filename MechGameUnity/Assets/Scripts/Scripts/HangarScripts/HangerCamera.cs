using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangerCamera : MonoBehaviour
{
    public GameObject cameraTarget;
    Camera thisCam;

    [Range(0, 100)]
    public float sensitivity;

    float baseOrthographicSize = 1.5f;
    float mechScale = 1;
    float scalingFactor = (float)(1); // Something

    bool dragHover;
    bool currentlyDragging;

    public Vector3 basePosition;
    float camZoom;
    float zoomVelocity;
    float minSize = 1.5f;
    float maxSize = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        thisCam = GetComponent<Camera>();
        thisCam.orthographicSize = baseOrthographicSize;
        camZoom = minSize;
    }

    // Update is called once per frame
    void Update()
    {
        thisCam.orthographicSize = mechScale * scalingFactor * baseOrthographicSize;

        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");

        if (currentlyDragging && Input.GetMouseButton(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //transform.RotateAround(cameraTarget.transform.position, -Vector3.up, rotateHorizontal * sensitivity);
            //transform.RotateAround(cameraTarget.transform.position, transform.right, rotateVertical * sensitivity);
            if (rotateHorizontal != 0 && Mathf.Abs(rotateHorizontal) > Mathf.Abs(rotateVertical))
            {
                cameraTarget.transform.Rotate(rotateHorizontal * sensitivity * Vector3.down);

            }
            else
            {
                this.transform.Translate(rotateVertical * Vector3.down * 0.25f);
                var pos = transform.position;
                pos.y = Mathf.Clamp(transform.position.y, 1.5f, 3.0f);
                pos.z = -5.25f;
                this.transform.position = pos;

                var rot = transform.rotation;
                rot.x = (pos.y - 1.5f) * 6f;
                this.transform.eulerAngles = new Vector3(rot.x, rot.y, rot.z);

            }

        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (dragHover)
        {
            currentlyDragging = Input.GetMouseButton(0);
        }
    }

    public void DragHoverTrue()
    {
        dragHover = true;
    }

    public void DragHoverFalse()
    {
        dragHover = false;
    }

}
