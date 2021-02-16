﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum InputType { Keyboard, JoyStick}
public class PlayerActions : MonoBehaviour
{
    public Button ToggleBTN;
    public Image HpFill;
    public float Speed;
    public float RotationSpeed;
    public InputType inputMode;
    public Joystick Joystick;

    private Vector3 movementVector;
    private Vector3 movementDirection;
    private Animator myAnim;



    private float smoothVelocity;
    private float horizontal;
    private float vertical;

    private bool isSafe = false;

    private void Awake()
    {
        ToggleBTN.onClick.AddListener(ToggleInput);
        inputMode = InputType.Keyboard;
        myAnim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            isSafe = true;
            Debug.Log("HIT TRIGGER!");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Finish")
        {
            isSafe = false;
            Debug.Log("EXITED SAFE ZONE!");
        }
    }
    private void ToggleInput()
    {
        if (inputMode == InputType.Keyboard)
        {
            inputMode = InputType.JoyStick;
            ToggleBTN.GetComponentInChildren<Text>().text = "JoyStick!";
        }
        else if(inputMode == InputType.JoyStick)
        {
            inputMode = InputType.Keyboard;
            ToggleBTN.GetComponentInChildren<Text>().text = "Keyboard!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSafe)
        {
            HpFill.fillAmount -= Time.deltaTime * 0.1f;
            if (HpFill.fillAmount <= 0)
                SceneManager.LoadScene(2);
        }
        if(inputMode == InputType.Keyboard)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

        }
        else
        {
            horizontal = Joystick.JoyPadInputVector.x;
            vertical = Joystick.JoyPadInputVector.z;
        }
            movementVector = new Vector3(horizontal, 0, vertical) * Time.deltaTime * Speed;
            movementDirection = movementVector.normalized;

        float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, RotationSpeed);

         myAnim.SetFloat("speed", movementDirection.magnitude);
        if (movementDirection.magnitude > 0f)
        {
             this.transform.position += movementVector;
            this.transform.rotation = Quaternion.Euler(0f, angle, 0f);

        }
        
    }
}
