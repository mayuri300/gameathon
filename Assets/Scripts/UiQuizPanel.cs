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
    public static Action OnCompleteAllQuiz;
    private void Awake()
    {
        Core.SubscribeEvent("OnSendTrigger", OnSendTrigger);

    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 10f);
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
            //ANSWER CORRECT LOGIC
            AudioManager.Instance.PlaySound(SoundEffectsType.Correct);
            int u = UnityEngine.Random.Range(3, 5);
            GameManager.Instance.IncreaseContribution(u);
            if (frontLinerTrigger != null)
            {
                frontLinerTrigger.enabled = false;
                GameManager.Instance.IsSafe = false;
            }
            MyData.CorrectAnswersCount++;
            GameManager.Instance.InstantiateSpawner();
            if (MyData.CorrectAnswersCount == 5 || MyData.CorrectAnswersCount == 9)
            {
                //show indicator
                OnCompleteAllQuiz?.Invoke();
            }
            GameManager.Instance.QuizAnsweredUi();
        }
        else
        {
            //WRONG ANSWER LOGIC
            AudioManager.Instance.PlaySound(SoundEffectsType.Wrong);
            GameManager.Instance.IncreaseMutation(1);
            OnAnsweredWrong?.Invoke(1.65f);
            MyData.SpreadRadius++;
        }
        Destroy(this.gameObject);

    }
}
