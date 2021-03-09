using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BatLogic : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody myRbdy;
    public float MoveSpeed;
    public Transform TargetPlayer;

    private void Awake()
    {
        myRbdy = this.GetComponent<Rigidbody>();
    }
    private void OnDestroy()
    {
    }
    void Start()
    {
        Scene c = SceneManager.GetActiveScene();
        if (c.name == "TutorialLvl")
            TargetPlayer = GameObject.FindObjectOfType<TutorialPlayerActions>().transform;
        else
            TargetPlayer = GameObject.FindObjectOfType<PlayerActions>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (TargetPlayer.position - this.transform.position).normalized;
        myRbdy.velocity = direction * MoveSpeed * Time.deltaTime;
        this.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}
