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


    private BoxCollider frontLinerTrigger;
    public static Action<float> OnAnsweredWrong;
    private void Awake()
    {
        Core.SubscribeEvent("OnSendTrigger", OnSendTrigger);

    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 7f);
    }

    private void OnSendTrigger(object sender, object[] args)
    {
          frontLinerTrigger = (BoxCollider)args[0];
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
        if(k == m)
        {
            Debug.Log("Correct Answer!!");
            AudioManager.Instance.PlaySound(SoundEffectsType.Correct);
            GameManager.Instance.IncreaseContribution(1);
            if (frontLinerTrigger != null)
            {
                frontLinerTrigger.enabled = false;
                GameManager.Instance.IsSafe = false;
            }
            MyData.CorrectAnswersCount++;
            GameManager.Instance.InstantiateSpawner();
        }
        else
        {
            Debug.LogError("Wrong Answer!");
            AudioManager.Instance.PlaySound(SoundEffectsType.Wrong);
            GameManager.Instance.IncreaseMutation(1);
            OnAnsweredWrong?.Invoke(1.6f);
            MyData.SpreadRadius++;
        }
        Destroy(this.gameObject);

    }
}
