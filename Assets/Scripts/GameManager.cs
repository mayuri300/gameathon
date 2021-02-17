using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    private int mutationPoints;
    private int contributionPoints;

    public TMP_Text MutationText;
    public TMP_Text ContributionText;
    [Header("QuizData")]
    public QuizData[] Quiz;
    public GameObject QuizPanel;
    public GameObject WorldCanvas;
    public GameObject InstantiatedQuiz = null;

    public void IncreaseMutation(int amount)
    {
        MutationText.text = string.Format("{0}/10", amount);
        if (amount >= 10)
        {
            SceneManager.LoadScene(2);
        }
    }
    public void IncreaseContribution(int amount)
    {
        ContributionText.text = string.Format("{0}/10", amount);
    }
    public  void InstantiateQuiz(int quizIndexer)
    {
        QuizData data;
        //instantiate panel
        Instantiate(QuizPanel, WorldCanvas.transform);
        //QuizPanel.transform.SetParent(WorldCanvas.transform, false);
        //Get quiz from
        QuizPanel.GetComponent<UiQuizPanel>().SetQuestion(quizIndexer);
    }
}
[System.Serializable]
public class QuizData
{
    public QuizScriptable quiz;
}