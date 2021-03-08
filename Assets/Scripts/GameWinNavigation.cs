using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWinNavigation : MonoBehaviour
{
    public Button ExitBTN;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic(MusicEffectsType.GameWin, true);
        ExitBTN.onClick.AddListener(QuitGame);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
