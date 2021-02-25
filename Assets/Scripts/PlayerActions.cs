using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum InputType { Keyboard, JoyStick}
public class PlayerActions : MonoBehaviour
{
    [Header("UI Stuff")]
    public Button ToggleBTN;
    public Image HpFill;
    public Joystick Joystick;
    public Image InfectionTimerFill;
    public Image Infection;
    public Image TipEnterPanel;
    [Header("Player Data")]
    public float Speed;
    public float RotationSpeed;
    public float SenseRadius;
    public InputType inputMode;
    public QuestionType Qtype;
    public QuestionType Ttype;

    private Vector3 movementVector;
    private Vector3 movementDirection;
    private Animator myAnim;
    private int CivilianLayer { get {  return 1 << LayerMask.NameToLayer("Civilian"); } }
    private float smoothVelocity;
    private float horizontal;
    private float vertical;

    private bool isSafe = false;
    private float infectionSpeed = 1f;
    private bool foundCivilian = false;



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
            Qtype = other.GetComponent<FrontLineTrigger>().Type;
            GameManager.Instance.InstantiateQuiz(Qtype);
            AudioManager.Instance.PlaySound(SoundEffectsType.PopUP, this.transform.position);
            Core.BroadcastEvent("OnSendTrigger", this, other.GetComponent<FrontLineTrigger>().QuizTrigger);
        }
        if (other.tag == "Tip")
        {
            TipEnterPanel.gameObject.SetActive(true);
            Ttype = other.GetComponent<TipsLogic>().TipType;
            Core.BroadcastEvent("OnSendTipType", this, Ttype);
        }
        if (other.tag == "LoadNext")
        {
            SceneManager.LoadScene(3, LoadSceneMode.Additive);
            Destroy(other.GetComponent<BoxCollider>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Finish")
        {
            isSafe = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, SenseRadius);
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
    public void EnableInfectionUI(bool foundCivilian,Collider[] civilian)
    {

            foundCivilian = true;
            if (foundCivilian)
            {

                Infection.gameObject.SetActive(true);
                InfectionTimerFill.gameObject.SetActive(true);
                infectionSpeed -= Time.deltaTime;
                InfectionTimerFill.fillAmount = infectionSpeed;         
            }
        if (infectionSpeed <= 0)
        {
            Infection.gameObject.SetActive(false);
            InfectionTimerFill.gameObject.SetActive(false);
            infectionSpeed = 1f;
            foundCivilian = false;
            foreach(Collider x in civilian)
            {
                x.GetComponent<CivilianLogic>().GetInfected();
            }
        }
            
    }
    // Update is called once per frame
    void Update()
    {
        if (!isSafe)
        {
            HpFill.fillAmount -= Time.deltaTime * 0.01f;
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

        Collider[] colls = Physics.OverlapSphere(this.transform.position, SenseRadius, CivilianLayer);
        if (colls.Length >0)
        {
            //Found a Civilian in range
            //Increase Mutation Points from GameManager
            EnableInfectionUI(foundCivilian, colls);
        }
        else
        {
            foundCivilian = false;
            Infection.gameObject.SetActive(false);
            InfectionTimerFill.gameObject.SetActive(false);
            infectionSpeed = 1f;
        }

    }
}
