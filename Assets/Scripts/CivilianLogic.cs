using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianLogic : MonoBehaviour
{
    public bool isInfected = false;
    public BoxCollider CivilianCollider;
    public Transform[] PatrolPoints;

    private Vector3 direction;
    private int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetInfected()
    {
        GameManager.Instance.IncreaseMutation(1);
        CivilianCollider.enabled = false;
        isInfected = true;

    }
}
