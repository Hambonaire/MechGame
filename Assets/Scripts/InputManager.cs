using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager _instance;

    [SerializeField]
    bool inputActive;
    public bool InputActive 
    {
        get => inputActive;
        set => inputActive = value;
    }

    float timer = 0;
    float onStartDelay = 1f;
    [SerializeField]
    bool firstPass = false;

    // Game Inputs
    public float MoveAxisX { get; private set; } = 0;
    public float MoveAxisY { get; private set; } = 0;
    public float MouseX { get; private set; } = 0;
    public float MouseY { get; private set; } = 0;
    public bool LeftClick { get; private set; } = false;
    public bool RightClick { get; private set; } = false;
    public bool LeftHeld { get; private set; } = false;
    public bool RightHeld { get; private set; } = false;
    public float ScrollDelta { get; private set; } = 0;

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (!firstPass && timer >= onStartDelay)
        {
            InputActive = true;
            firstPass = true;
        }

        GetGameInputs();
    }

    private void LateUpdate()
    {
        ResetGameInputs();
    }

    public void ToggleInput(bool val)
    {
        InputActive = val;
    }

    void GetGameInputs()
    {
        if (InputActive)
        {
            MoveAxisX = Input.GetAxis("Horizontal");
            MoveAxisY = Input.GetAxis("Vertical");

            MouseX = Input.GetAxis("Mouse X");
            MouseY = Input.GetAxis("Mouse Y");

            LeftClick = Input.GetMouseButtonDown(0);
            RightClick = Input.GetMouseButtonDown(1);

            LeftHeld = Input.GetMouseButton(0);
            RightHeld = Input.GetMouseButton(1);

            ScrollDelta = Input.mouseScrollDelta.y;
        }
    }

    void ResetGameInputs()
    {
        MoveAxisX = 0;
        MoveAxisY = 0;

        MouseX = 0;
        MouseY = 0;

        LeftClick = false;
        RightClick = false;

        LeftHeld = false;
        RightHeld = false;

        ScrollDelta = 0;
    }
}
