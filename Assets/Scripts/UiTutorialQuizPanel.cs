using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class UiTutorialQuizPanel : MonoBehaviour
{
    public TMP_Text Question;
    public Button[] Options;
    public int correctAnswer;

    private BoxCollider frontLinerTrigger;
    public static Action<float> OnAnsweredWrongTutorial;
    private void Awake()
    {
        Core.SubscribeEvent("OnSendTrigger", OnSendTrigger);
    }

    private void OnSendTrigger(object sender, object[] args)
    {
        frontLinerTrigger = (BoxCollider)args[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 10f);
    }
    private void OnDestroy()
    {
        foreach (Button x in Options)
        {
            x.onClick.RemoveAllListeners();
        }
    }
 
    public void OptionClick(Button butt)
    {
        string k = butt.gameObject.name;
        string m = TutorialManager.Instance.InstantiatedQuizData.quiz.CorrectOption.ToString();
        if (k == m)
        {
            //Correct
            Debug.Log("CORRECT!");
            //Sound
            int u = UnityEngine.Random.Range(3, 5);
            TutorialManager.Instance.IncreaseContribution(u);
            if (frontLinerTrigger != null)
            {
                frontLinerTrigger.enabled = false;
                TutorialManager.Instance.isSafe = false;
                GameObject.FindObjectOfType<TutorialPlayerActions>().EnableAttack(true);
            }
        }
        else
        {
            //Wrong
            Debug.Log("WRONG!");
            //Sound
            TutorialManager.Instance.IncreaseMutation(1);
            OnAnsweredWrongTutorial?.Invoke(1.6f);
        }
        Destroy(this.gameObject);
    }
}
