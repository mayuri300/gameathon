using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialPlayerActions : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float smoothVelocity;
    private float infectionSpeed = 1f;
    private bool foundCivilian = false;
    private int CivilianLayer { get { return 1 << LayerMask.NameToLayer("Civilian"); } }
    public InputType inputMode;
    public QuestionType Qtype;
    public QuestionType Ttype;
    private Vector3 movementVector;
    private Vector3 movementDirection;
    private Animator myAnim;

    public float Speed;
    public float RotationSpeed;
    public Joystick JoyStick;
    public Image HpFill;
    public Button AttackBTN;
    public Image InfectionTimerFill;
    public Image Infection;
    public GameObject TipEnterPanel;
    public GameObject SpreadFX;
    public GameObject MagicBullet;
    public Transform NozzlePos;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = this.GetComponent<Animator>();
        inputMode = InputType.JoyStick;
        AttackBTN.onClick.AddListener(AttackLogic);

        UiTutorialQuizPanel.OnAnsweredWrongTutorial += IncreaseSpreadFXRadius;
        EnableAttack(false);
    }
    private void OnDestroy()
    {
        UiTutorialQuizPanel.OnAnsweredWrongTutorial -= IncreaseSpreadFXRadius;
    }
    private void OnTriggerEnter(Collider other)
    {
 
        //Quiz Trigger
        if(other.tag == "Finish")
        {
            TutorialManager.Instance.isSafe = true;
            FrontLineTrigger kk = other.GetComponent<FrontLineTrigger>();
            Qtype = kk.Type;
            //Instantiate Quiz and Sound
            TutorialManager.Instance.InstantiateQuiz(Qtype);
            Core.BroadcastEvent("OnSendTrigger", this, other.GetComponent<FrontLineTrigger>().QuizTrigger);
        }
        //Tips Trigger
        if(other.tag == "Tip")
        {
            //AudioManager.Instance.PlaySound(SoundEffectsType.PopUP);
            TipEnterPanel.SetActive(true);
            Ttype = other.GetComponent<TipsLogic>().TipType;
            BoxCollider k = other.GetComponent<TipsLogic>().Collider;
            Core.BroadcastEvent("OnSendTipType", this, Ttype, k);
            //send Ttype to tip ui panel
        }
        //Bats Trigger
        if (other.tag == "Bat")
        {
            //AudioManager.Instance.PlaySound(SoundEffectsType.Infected);
            TutorialManager.Instance.IncreaseMutation(1);
            Destroy(other.gameObject);
        }
        if (other.tag == "LoadNext")
        {
            PortalLogic pl = other.GetComponent<PortalLogic>();
            SceneManager.LoadScene((int)pl.LevelToLoad);
        }
        if (other.tag == "HP")
        {
            //AudioManager.Instance.PlaySound(SoundEffectsType.Completed);
            TutorialManager.Instance.HP += 3f;
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Quiz Trigger
        if (other.tag == "Finish")
        {
            TutorialManager.Instance.isSafe = false;
        }
    }
    public void IncreaseSpreadFXRadius(float amount)
    {
        SpreadFX.transform.localScale = new Vector3(SpreadFX.transform.localScale.x + amount, SpreadFX.transform.localScale.y + amount, SpreadFX.transform.localScale.z + amount);
        TutorialManager.Instance.SpreadRadius += 1;
    }
    public void AttackLogic()
    {
        if (TutorialManager.Instance.ContributionPoints <= 0)
            return;
        else
        {
            //AudioManager.Instance.PlaySound(SoundEffectsType.Attack);
            myAnim.SetTrigger("attack");
            TutorialManager.Instance.IncreaseContribution(-1);
            inputMode = InputType.None;
            AttackBTN.interactable = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        #region MOVEMENTLOGIC
        if (inputMode == InputType.Keyboard) //TODO REmove Later after DEPLOYMENT
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

        }
        else
        {
            horizontal = JoyStick.JoyPadInputVector.x;
            vertical = JoyStick.JoyPadInputVector.z;
        }
        if (inputMode == InputType.None)
        {
            horizontal = 0f;
            vertical = 0f;
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
        #endregion

        Collider[] colls = Physics.OverlapSphere(this.transform.position, TutorialManager.Instance.SpreadRadius, CivilianLayer);
        if (colls.Length > 0)
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
    public void EnableInfectionUI(bool foundCivilian, Collider[] civilian)
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
            //AudioManager.Instance.PlaySound(SoundEffectsType.Infected);
            Infection.gameObject.SetActive(false);
            InfectionTimerFill.gameObject.SetActive(false);
            infectionSpeed = 1f;
            foundCivilian = false;
            foreach (Collider x in civilian)
            {
                x.GetComponent<CivilianLogic>().GetInfected();
            }
        }

    }
    public void PlayFootStepsSound()
    {
        AudioManager.Instance.PlaySound(SoundEffectsType.Footsteps, this.transform.position);
    }

    public void AnimEventStartAttack()
    {
        //Start Instantiating at this frame
        Instantiate(MagicBullet, NozzlePos.position, NozzlePos.rotation);
    }
    public void AnimEventStopAttack()
    {
        //Enable Movement
        //ReEnable Button Spam
        AttackBTN.interactable = true;
        inputMode = InputType.JoyStick;
    }
    public void EnableAttack(bool canAtk)
    {
        AttackBTN.interactable=(canAtk);
    }
}
