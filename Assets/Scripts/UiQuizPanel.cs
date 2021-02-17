using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiQuizPanel : MonoBehaviour
{
    public TMP_Text Question;
    public Button[] Options;
    private int correctAnswer;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 10f);
        foreach(Button x in Options)
        {
            x.onClick.AddListener(OnOptionClick);
        }
    }
    private void OnDestroy()
    {
        foreach (Button x in Options)
        {
            x.onClick.RemoveAllListeners();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetQuestion(int indexer)
    {
        correctAnswer = GameManager.Instance.Quiz[indexer].quiz.CorrectOption;
        Question.text = GameManager.Instance.Quiz[indexer].quiz.Question;
        for(int i = 0; i <= Options.Length-1; i++)
        {
            Options[i].GetComponentInChildren<TMP_Text>().text = GameManager.Instance.Quiz[indexer].quiz.Options[i];
        }
    }

    public void OnOptionClick()
    {
        Debug.Log("CLICKED OPTIONS");
    }
}
