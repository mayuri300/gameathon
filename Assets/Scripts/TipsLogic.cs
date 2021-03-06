﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsLogic : MonoBehaviour
{
    public QuestionType TipType;
    [HideInInspector]
    public GameObject Top;
    [HideInInspector]
    public GameObject Base;
    public float RoatateSpeed;
    [HideInInspector]
    public BoxCollider Collider;
    private void Awake()
    {
        Collider = this.GetComponent<BoxCollider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Top.transform.Rotate(0f, RoatateSpeed * Time.deltaTime, 0f);
        Base.transform.Rotate(0, -RoatateSpeed * Time.deltaTime, 0f);
    }
}
