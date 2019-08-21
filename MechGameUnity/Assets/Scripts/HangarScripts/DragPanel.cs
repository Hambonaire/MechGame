using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPanel : MonoBehaviour
{
    public GameObject cameraTarget;
    Camera thisCam;

    [Range(0, 100)]
    public float sensitivity;

    float baseOrthographicSize = 1.5f;
    float mechScale = 1;
    float scalingFactor = (float)(1); // Something

    Vector3 currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        thisCam = GetComponent<Camera>();
        thisCam.orthographicSize = baseOrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        thisCam.orthographicSize = mechScale * scalingFactor * baseOrthographicSize;

        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");
        //use transform.Rotate(-transform.up * rotateHorizontal * sensitivity) instead if you dont want the camera to rotate around the player
        transform.RotateAround(cameraTarget.transform.position, -Vector3.up, rotateHorizontal * sensitivity);
        // again, use transform.Rotate(transform.right * rotateVertical * sensitivity) if you don't want the camera to rotate around the player
        //transform.RotateAround(cameraTarget.transform.position, transform.right, rotateVertical * sensitivity);
    }


}
