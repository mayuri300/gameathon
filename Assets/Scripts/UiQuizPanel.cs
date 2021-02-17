using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UiQuizPanel : MonoBehaviour
{
    public TMP_Text Question;
    public Button[] Options;
    public int correctAnswer;
    private QuestionType type;

    // Start is called before the first frame update
    void Start()
    {
        Core.SubscribeEvent("OnSendType", OnReceiveType);
    }

    private void OnReceiveType(object sender, object[] args)
    {
        type = (QuestionType)args[0];
    }

    private void OnDestroy()
    {
        foreach (Button x in Options)
        {
            x.onClick.RemoveAllListeners();
        }
        Core.UnsubscribeEvent("OnSendType", OnReceiveType);
    }

    public void OnOptionClick(Button butt)
    {
        string k = butt.gameObject.name;
        Debug.Log("Button Clicked : " + k + " Type of this Collision : " + type);
        Destroy(this.gameObject);

    }
    private void Update()
    {
        Debug.Log("GameEvent Type : " + type);
    }
}
