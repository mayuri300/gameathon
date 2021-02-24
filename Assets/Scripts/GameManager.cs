using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum QuestionType { Level0=0,Level1,Level2,Level3,Level4,Level5,Level6,Level7,Level8,Level9,Level10};
public class GameManager : MonoBehaviour
{
    private Dictionary<QuestionType, QuizData> _Qbank = new Dictionary<QuestionType, QuizData>();
    public static GameManager Instance { private set; get; }

    private void Awake()
    {
        Instance = this;
        foreach(QuizData x in Quiz)
        {
            _Qbank[x.Type] = x;
        }
        AudioManager.Instance.PlayMusic(MusicEffectsType.GamePlay, true); //TO Play Sounds when Game Loads
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
    public GameObject TipPanel;
    public GameObject WorldCanvas;
    public QuizData InstantiatedQuizData = null;

    public void IncreaseMutation(int amount)
    {
        mutationPoints += amount;
        MutationText.text = string.Format("{0} / 10", mutationPoints);
        if (amount >= 10)
        {
            SceneManager.LoadScene(2);
        }
    }
    public void IncreaseContribution(int amount)
    {
        contributionPoints += amount;
        ContributionText.text = string.Format("{0} / 10", contributionPoints);
    }
    public  void InstantiateQuiz(QuestionType type) //Called on Trigger Enter
    {
        //instantiate panel
        GameObject InstantiatedQuiz = Instantiate(QuizPanel, WorldCanvas.transform);
        //QuizPanel.transform.SetParent(WorldCanvas.transform, false);
        //Get quiz from
        //QuizData data;
        if(_Qbank.TryGetValue(type, out InstantiatedQuizData))
        {
            UiQuizPanel panel = InstantiatedQuiz.GetComponent<UiQuizPanel>();
            panel.Question.text = InstantiatedQuizData.quiz.Question;
            panel.correctAnswer = InstantiatedQuizData.quiz.CorrectOption;
            for(int i =0; i <= panel.Options.Length - 1; i++)
            {
                panel.Options[i].GetComponentInChildren<TMP_Text>().text = InstantiatedQuizData.quiz.Options[i];
            }
        }
    }
    public void InstantiateTips(QuestionType type)
    {
        GameObject InstantiatedTip = Instantiate(TipPanel, WorldCanvas.transform);
        QuizData data;
        if(_Qbank.TryGetValue(type,out data))
        {
            UiTipPanel panel = InstantiatedTip.GetComponent<UiTipPanel>();
            panel.TipDetail.text = data.quiz.Tips;
        }
    }
}
[System.Serializable]
public class QuizData
{
    public QuestionType Type;
    public QuizScriptable quiz;
}