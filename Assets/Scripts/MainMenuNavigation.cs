using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuNavigation : MonoBehaviour
{
    public Button PlayGameBTN;
    public Button ExitGameBTN;
    public Image LoadingPanel;
    public Text LoadingProgressText;

    private void Awake()
    {
        AudioManager.Instance.PlayMusic(MusicEffectsType.MainMenu);
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayGameBTN.onClick.AddListener(PlayGame);
        ExitGameBTN.onClick.AddListener(ExitGame);
    }
    private void OnDestroy()
    {
        PlayGameBTN.onClick.RemoveAllListeners();
        ExitGameBTN.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        //Async Load into Game Scene
        StartCoroutine(LoadGame());
    }
    IEnumerator LoadGame()
    {
        yield return null;
        AsyncOperation ao = SceneManager.LoadSceneAsync(1);
        ao.allowSceneActivation = false;
        while (!ao.isDone)
        {
            //display loading text and progress
            LoadingPanel.gameObject.SetActive(true);
            LoadingProgressText.text = "Loading : " + (ao.progress * 100) + "%";
            yield return new WaitForSeconds(2f);
            if(ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
