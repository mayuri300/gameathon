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

    }



    private void OnDestroy()
    {
        foreach (Button x in Options)
        {
            x.onClick.RemoveAllListeners();
        }
    }

    public void OnOptionClick(Button butt)
    {
        string k = butt.gameObject.name;
        string m = GameManager.Instance.InstantiatedQuizData.quiz.CorrectOption.ToString();
        //Debug.Log("Button Clicked : " + k + " Type of this Collision : " + type +" Instantiated data Correctopt : " + m);
        if(k == m)
        {
            Debug.Log("Correct Answer!!");
            GameManager.Instance.IncreaseContribution(1);
        }
        else
        {
            Debug.LogError("Wrong Answer!");
        }
        Destroy(this.gameObject);

    }
    private void Update()
    {
        Debug.Log("GameEvent Type : " + type);
    }
}
