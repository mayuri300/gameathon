using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { private set; get; }
    private void Awake()
    {
        Instance = this;
        foreach (QuizData x in Quiz)
        {
            _Qbank[x.Type] = x;
        }
    }
    private void OnDestroy()
    {
        Instance = null;
        Destroy(this.gameObject);
    }

    public bool isSafe = false;
    public Image HPFill;
    public Canvas TutorialCanvas;
    private float maxHp = 100f;
    private float tickTime = 2f;
    private Dictionary<QuestionType, QuizData> _Qbank = new Dictionary<QuestionType, QuizData>();
    public GameObject QuizPanel;
    public GameObject TipPanel;
    public QuizData[] Quiz;
    private QuizData InstantiatedQuizData = null;
    public float HP { get { return maxHp; } set { maxHp = value; } }

    private int contributionPoints;
    public int ContributionPoints { get { return contributionPoints; } }
    private int mutationPoints;
    public int MutationPoints { get{ return mutationPoints; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void InstantiateQuiz(QuestionType type)
    {
        GameObject InstantiatedQuiz = Instantiate(QuizPanel, TutorialCanvas.transform);
        if (_Qbank.TryGetValue(type, out InstantiatedQuizData))
        {
            UiQuizPanel panel = InstantiatedQuiz.GetComponent<UiQuizPanel>();
            panel.Question.text = InstantiatedQuizData.quiz.Question;
            panel.correctAnswer = InstantiatedQuizData.quiz.CorrectOption;
            for (int i = 0; i <= panel.Options.Length - 1; i++)
            {
                panel.Options[i].GetComponentInChildren<TMP_Text>().text = InstantiatedQuizData.quiz.Options[i];
            }
        }
    }
    public void InstantiateTips(QuestionType type)
    {
        GameObject InstantiatedTip = Instantiate(TipPanel, TutorialCanvas.transform);
        QuizData data;
        if (_Qbank.TryGetValue(type, out data))
        {
            UiTipPanel panel = InstantiatedTip.GetComponent<UiTipPanel>();
            panel.TipDetail.text = data.quiz.Tips;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isSafe)
        {
            HPFill.fillAmount = maxHp / 100f;
            tickTime -= Time.deltaTime;
            if (tickTime <= 0)
            {
                maxHp -= 2f;
                tickTime = 2f;
            }
        }
    }
}
