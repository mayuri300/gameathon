using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuNavigation : MonoBehaviour
{
    public Button PlayGameBTN;
    //public Button ExitGameBTN; // Replace with phone back function
    public Button TutorialBTN;
    public Button SettingsBTN;
    public GameObject SettingsPanel;
    public GameObject QuitPanel;
    public Image LoadingPanel;
    public Text LoadingProgressText;
    public Text LoadingTipText;
    public GameLevels GameLevel;
    public GameLevels TutorialLevel;

    private void Awake()
    {
        AudioManager.Instance.PlayMusic(MusicEffectsType.MainMenu);
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayGameBTN.onClick.AddListener(PlayGame);
        TutorialBTN.onClick.AddListener(LoadTutorial);
        SettingsBTN.onClick.AddListener(ShowSettings);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Instantiate(QuitPanel, this.transform);
        }
    }
    private void OnDestroy()
    {
        PlayGameBTN.onClick.RemoveAllListeners();
        TutorialBTN.onClick.RemoveAllListeners();
        SettingsBTN.onClick.RemoveAllListeners();
    }

    public void PlayGame()
    {
        //Async Load into Game Scene
        StartCoroutine(LoadGame((int)GameLevel));
    }
    public void LoadTutorial()
    {
        StartCoroutine(LoadGame((int)TutorialLevel));
    }
    public void ShowSettings()
    {
        Instantiate(SettingsPanel,this.transform);
    }
    IEnumerator LoadGame(int level)
    {
        yield return null;
        AsyncOperation ao = SceneManager.LoadSceneAsync(level);
        ao.allowSceneActivation = false;
        while (!ao.isDone)
        {
            //display loading text and progress
            LoadingPanel.gameObject.SetActive(true);
            LoadingProgressText.text = "Loading : " + (ao.progress * 100).ToString("f1") + "%";
            if(level == (int)GameLevel)
            {
                LoadingTipText.text = "Loading all resources for Main Game. Please be patient!";

            }
            else
            {
                LoadingTipText.text = "Loading all resources for Tutorial Game Level. Make Best use of it, Please be patient! Thank You.";
            }
            yield return new WaitForSeconds(2f);
            if(ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
