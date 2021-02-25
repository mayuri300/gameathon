using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianLogic : MonoBehaviour
{
    public bool isInfected = false;
    public BoxCollider CivilianCollider;
    public Transform[] PatrolPoints;
    public float MoveSpeed;
    public GameObject InfectionFX;

    private Transform target;
    private Vector3 direction;
    private float distance;
    private int currentIndex = 0;
    private Vector3 currentPos;
    private Vector3 velocity;
    private Animator myAnim;

    bool hasReached = false;
    float cd = 7f;
    private void Awake()
    {
        InfectionFX.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        target = PatrolPoints[currentIndex];
        currentPos = this.transform.position;
        myAnim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasReached == false)
        {
            direction = (target.position - this.transform.position).normalized;
            distance = (this.transform.position - target.position).magnitude;
            this.transform.Translate(direction * MoveSpeed * Time.deltaTime, Space.World);
            velocity = (currentPos - this.transform.position) / Time.deltaTime;
            currentPos = this.transform.position;
            if (velocity != Vector3.zero)
                myAnim.SetFloat("speed", velocity.magnitude);
            this.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        if (distance <= 0.2f)
        {
            hasReached = true;
            myAnim.SetFloat("speed", 0f);
            cd -= Time.deltaTime;
            if (cd <= 0)
            {
                GetNextPoint();
                hasReached = false;
                cd = 7f;
            }
        }
    }
    public void GetNextPoint()
    {
        if (currentIndex >= PatrolPoints.Length-1)
        {
            currentIndex = 0;
            target = PatrolPoints[currentIndex];
        }
        else
        {
            currentIndex++;
            target = PatrolPoints[currentIndex];

        }
    }
    public void GetInfected()
    {
        GameManager.Instance.IncreaseMutation(1);
        InfectionFX.SetActive(true);
        CivilianCollider.enabled = false;
        isInfected = true;

    }
}
