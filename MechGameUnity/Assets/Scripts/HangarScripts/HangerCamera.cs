using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangerCamera : MonoBehaviour
{
    public Vector3 mainCameraAngle;
    public Vector3 mainCameraPosition;
    public int mainFOV;

    public Vector3 rightArmCameraAngle;
    public Vector3 rightArmCameraPosition;
    public int rightArmFOV;

    public Vector3 leftArmCameraAngle;
    public Vector3 leftArmCameraPosition;
    public int leftArmFOV;

    public Vector3 cockpitCameraAngle;
    public Vector3 cockpitCameraPosition;
    public int cockpitFOV;

    public Vector3 legsCameraAngle;
    public Vector3 legsCameraPosition;
    public int legsFOV;

    Vector3 currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = mainCameraPosition;
        this.transform.rotation = Quaternion.Euler(mainCameraAngle);
        this.GetComponent<Camera>().fieldOfView = mainFOV;
    }

    // Update is called once per frame
    void Update()
    {
        //currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        //this.transform.eulerAngles = currentRotation;
    }

    public void ToCockpit()
    {
        this.transform.position = cockpitCameraPosition;
        this.transform.rotation = Quaternion.Euler(cockpitCameraAngle);
        this.GetComponent<Camera>().fieldOfView = cockpitFOV;
    }

    public void ToLegs()
    {
        this.transform.position = legsCameraPosition;
        this.transform.rotation = Quaternion.Euler(legsCameraAngle);
        this.GetComponent<Camera>().fieldOfView = legsFOV;
    }

    public void ToRightArm()
    {
        this.transform.position = rightArmCameraPosition;
        this.transform.rotation = Quaternion.Euler(rightArmCameraAngle);
        this.GetComponent<Camera>().fieldOfView = rightArmFOV;
    }

    public void ToLeftArm()
    {
        this.transform.position = leftArmCameraPosition;
        this.transform.rotation = Quaternion.Euler(leftArmCameraAngle);
        this.GetComponent<Camera>().fieldOfView = leftArmFOV;
    }

    public void ToMain()
    {
        this.transform.position = mainCameraPosition;
        this.transform.rotation = Quaternion.Euler(mainCameraAngle);
        this.GetComponent<Camera>().fieldOfView = mainFOV;
    }
}
