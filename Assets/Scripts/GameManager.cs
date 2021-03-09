using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum QuestionType { Level0=0,Level1,Level2,Level3,Level4,Level5,Level6,Level7,Level8,Level9,Level10};
public enum GameLevels { MainMenu=0,TutorialLvl=1,QuizLvl1=2,BatsLvl1=3,QuizLvl2=4,BatsLvl2=5,GameOver=6,GameWin=7};
public enum LevelType { None=0,QuizLevel,SurvivalLevel};
public class GameManager : MonoBehaviour
{
    private Dictionary<QuestionType, QuizData> _Qbank = new Dictionary<QuestionType, QuizData>();
    public static GameManager Instance { private set; get; }

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this.gameObject);

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
    private bool isSafe = false;
    public bool IsSafe
    {
        get { return isSafe; }
        set { isSafe = value; }
    }

    private int mutationPoints;
    public int MutationPoints { get { return mutationPoints; } }

    private int contributionPoints;
    public int ContributionPoints { get { return contributionPoints; } }

    [Header("UI Stuff")]
    public TMP_Text MutationText;
    public TMP_Text ContributionText;
    [Header("QuizData")]
    public QuizData[] Quiz;
    [Header("Quiz and Tip GameObjects")]
    public GameObject QuizPanel;
    public GameObject TipPanel;
    [HideInInspector]
    public GameObject WorldCanvas;
    [HideInInspector]
    public QuizData InstantiatedQuizData = null;
    [Header("LevelSpawner")]
    public Vector3 Level1Spawnner;
    public Vector3 Level2Spawnner;
    public GameObject Portal;
    public GameObject FadePanel;

    public void IncreaseMutation(int amount)
    {
        mutationPoints += amount;
        MutationText.text = string.Format("{0} / 10", mutationPoints);
        if (mutationPoints >= 10)
        {
            SceneManager.LoadScene(6); //6=GameOver
        }
    }
    public void IncreaseContribution(int amount)
    {
        contributionPoints += amount;
        ContributionText.text = string.Format("{0}", contributionPoints);
    }
    public  void InstantiateQuiz(QuestionType type) //Called on Trigger Enter
    {
        GameObject InstantiatedQuiz = Instantiate(QuizPanel, WorldCanvas.transform);
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
    public void InstantiateSpawner()
    {
        switch (MyData.CorrectAnswersCount)
        {
            case 5: //Answered all quiz in Lvl1
                GameObject k = Instantiate(Portal, Level1Spawnner, Portal.transform.rotation);
                PortalLogic l = k.GetComponent<PortalLogic>();
                l.LevelToLoad = GameLevels.BatsLvl1;
                l.NextLevelType = LevelType.SurvivalLevel;

                //TODO Load a Message screen saying Move to the portal to load next Level - !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                GameObject u = Instantiate(TipPanel, WorldCanvas.transform);
                UiTipPanel tp = u.GetComponent<UiTipPanel>();
                tp.TipDetail.text = "You have completed all the Quiz, Please Enter the Portal to unlock next level!";
                break;
            case 9: //Answered all quiz in Lvl2
                GameObject p = Instantiate(Portal, Level2Spawnner, Portal.transform.rotation);
                PortalLogic pl = p.GetComponent<PortalLogic>();
                pl.LevelToLoad = GameLevels.BatsLvl2;
                pl.NextLevelType = LevelType.SurvivalLevel;

                //TODO Load a Message screen saying Move to the portal to load next Level - !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                GameObject go = Instantiate(TipPanel, WorldCanvas.transform);
                UiTipPanel uitp = go.GetComponent<UiTipPanel>();
                uitp.TipDetail.text = "You have completed all the Quiz, Please Enter the Portal to unlock next level!";
                break;
        }
    }
}
[System.Serializable]
public class QuizData
{
    public QuestionType Type;
    public QuizScriptable quiz;
}