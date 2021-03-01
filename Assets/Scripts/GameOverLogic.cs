using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverLogic : MonoBehaviour
{
    public Button QuitBTN, RetryBTN;
    private void Awake()
    {
        QuitBTN.onClick.AddListener(QuitGame);
        RetryBTN.onClick.AddListener(BackToMainMenu);
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic(MusicEffectsType.GameOver);
    }


    private void OnDestroy()
    {
        QuitBTN.onClick.RemoveAllListeners();
        RetryBTN.onClick.RemoveAllListeners();
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        //Reset all MyData stats
        //Spread Radius to 3
        MyData.SpreadRadius = 3f;
        //correctely answered to 0
        MyData.CorrectAnswersCount = 0;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
