using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UiTipEnterLogic : MonoBehaviour
{
    public Button YesBTN;
    public Button NoBTN;
    private QuestionType Ttype;
    private BoxCollider Collider;
    private void Awake()
    {
        NoBTN.onClick.AddListener(HideTipEnterPanel);
        YesBTN.onClick.AddListener(DisplayTip);
        Core.SubscribeEvent("OnSendTipType", OnReceiveTipType);
    }

    private void OnReceiveTipType(object sender, object[] args)
    {
        Ttype = (QuestionType)args[0];
        Collider = (BoxCollider)args[1];
    }

    private void OnDestroy()
    {
        NoBTN.onClick.RemoveAllListeners();
        Core.UnsubscribeEvent("OnSendTipType", OnReceiveTipType);
    }
    public void HideTipEnterPanel()
    {
        this.gameObject.SetActive(false);
    }
    public void DisplayTip()
    {
        GameManager.Instance.InstantiateTips(Ttype);
        GameManager.Instance.IncreaseContribution(-1);
        Collider.enabled = false;
        this.gameObject.SetActive(false);
    }
}
