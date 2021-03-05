using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum InputType { None,Keyboard, JoyStick}
public class PlayerActions : MonoBehaviour
{
    [Header("UI Stuff")]
    public Button ToggleBTN;
    public Image HpFill;
    public Joystick Joystick;
    public Image InfectionTimerFill;
    public Image Infection;
    public Image TipEnterPanel;
    public Button AttackBTN;
    [Header("Player Data")]
    public float Speed;
    public float RotationSpeed;
    public float SenseRadius;
    public InputType inputMode;
    public QuestionType Qtype;
    public QuestionType Ttype;
    [Header("Prefabs")]
    public GameObject MagicBullet;
    public Transform NozzlePos;
    public GameObject SpreadFX;
    public GameObject SmokeHitFX;
    public GameObject FadePanel;
    

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

    private string batLevelIntro = "You have entered Survival Level. Defend yourself against bats by attacking them. Beware your attack ammo is based on your contribution points!!";
    private string quizLevelIntro = "You have entered Quiz Level. Find all FrontLine Workers and answer the quiz correctly. Do not infect the Civilians. Wrong answers will increase the spread radius beware!!";



    private void Awake()
    {
        ToggleBTN.onClick.AddListener(ToggleInput);
        inputMode = InputType.JoyStick;
        myAnim = GetComponent<Animator>();
        AttackBTN.onClick.AddListener(AttackLogic);
        AttackBTN.interactable = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        UiQuizPanel.OnAnsweredWrong += IncreaseSpreadFXRadius;
    }

    private void OnDestroy()
    {
        UiQuizPanel.OnAnsweredWrong -= IncreaseSpreadFXRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            GameManager.Instance.IsSafe = true;
            FrontLineTrigger kk = other.GetComponent<FrontLineTrigger>();
            Qtype = kk.Type;
            kk.RemoveIndicator();
            GameManager.Instance.InstantiateQuiz(Qtype);
            AudioManager.Instance.PlaySound(SoundEffectsType.PopUP, this.transform.position);
            Core.BroadcastEvent("OnSendTrigger", this, other.GetComponent<FrontLineTrigger>().QuizTrigger);
        }
        if (other.tag == "Tip")
        {
            TipEnterPanel.gameObject.SetActive(true);
            Ttype = other.GetComponent<TipsLogic>().TipType;
            BoxCollider k = other.GetComponent<TipsLogic>().Collider;
            Core.BroadcastEvent("OnSendTipType", this, Ttype,k);
        }
        if (other.tag == "LoadNext")
        {
            PortalLogic pl = other.GetComponent<PortalLogic>();
            SceneManager.LoadScene((int)pl.LevelToLoad, LoadSceneMode.Additive);

            //Load short Intro of Next Level in Tip Panel
            GameObject Tipobj = null;
            UiTipPanel tp = null;
            switch (pl.NextLevelType)
            {
                case LevelType.SurvivalLevel:
                    Tipobj = Instantiate(GameManager.Instance.TipPanel, GameManager.Instance.WorldCanvas.transform);
                    tp = Tipobj.GetComponent<UiTipPanel>();
                    tp.TipDetail.text = batLevelIntro;
                    break;
                case LevelType.QuizLevel:
                    Tipobj = Instantiate(GameManager.Instance.TipPanel, GameManager.Instance.WorldCanvas.transform);
                    tp = Tipobj.GetComponent<UiTipPanel>();
                    tp.TipDetail.text = quizLevelIntro;
                    break;
            }

            Destroy(other.GetComponent<BoxCollider>());
            AttackBTN.interactable = true;
        }
        if (other.tag == "Bat")
        {
            Destroy(other.gameObject);
            Instantiate(SmokeHitFX, this.transform);
            AudioManager.Instance.PlaySound(SoundEffectsType.Infected,this.transform.position);
            GameManager.Instance.IncreaseMutation(1);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Finish")
        {
            GameManager.Instance.IsSafe = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, MyData.SpreadRadius);
    }

    public void IncreaseSpreadFXRadius(float amount)
    {
        SpreadFX.transform.localScale = new Vector3(SpreadFX.transform.localScale.x + amount, SpreadFX.transform.localScale.y + amount, SpreadFX.transform.localScale.z + amount);
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
            AudioManager.Instance.PlaySound(SoundEffectsType.Infected);
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
    public  void AttackLogic()
    {
        if (GameManager.Instance.ContributionPoints <= 0) 
        {
            FadingTMP ftp = FadePanel.GetComponent<FadingTMP>();
            ftp.Details.text = "Out of Ammo!!";
            Instantiate(FadePanel, GameManager.Instance.WorldCanvas.transform);
        }
        else
        {
            AudioManager.Instance.PlaySound(SoundEffectsType.Attack, this.transform.position);
            myAnim.SetTrigger("attack");
            GameManager.Instance.IncreaseContribution(-1);
            //Stop Movement
            inputMode = InputType.None;
            //Disable interacable of this btn
            AttackBTN.interactable = false;
            //Instantiate Bullet based on AnimEvent
            //REnable Movement at end of animation
        } 
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsSafe) //Not Working because front liner trigger is getting destroyed!!
        {
            HpFill.fillAmount -= Time.deltaTime * 0.01f;
            if (HpFill.fillAmount <= 0)
                SceneManager.LoadScene(6); //6 => GO
        }
        if (Input.GetButtonDown("Jump"))  //TODO REmove Later after DEPLOYMENT
        {
            myAnim.SetTrigger("attack");
            AudioManager.Instance.PlaySound(SoundEffectsType.Attack, this.transform.position);
            
        }
        #region MOVEMENTLOGIC
        if (inputMode == InputType.Keyboard) //TODO REmove Later after DEPLOYMENT
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

        }
        else
        {
            horizontal = Joystick.JoyPadInputVector.x;
            vertical = Joystick.JoyPadInputVector.z;
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

        Collider[] colls = Physics.OverlapSphere(this.transform.position, MyData.SpreadRadius, CivilianLayer);
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
    #region AnimationEvents
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
    public void PlayFootStepsSound()
    {
        AudioManager.Instance.PlaySound(SoundEffectsType.Footsteps, this.transform.position);
    }
    #endregion
}
