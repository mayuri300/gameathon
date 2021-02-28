using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LvlStub : MonoBehaviour
{
    public string SceneName;
    public LevelType LevelType;
    public GameObject BatSpawnnerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Scene sc = SceneManager.GetSceneByName(SceneName);
        if (sc.isLoaded)
        {
            LevelType = LevelType.BatsLevel;
        }
        else
        {
            LevelType = LevelType.QuizLevel;
        }
        if (LevelType == LevelType.BatsLevel)
        {
            //Enable Attacking for Player
            //Enable CountDownTimer ToSpawnBats
            Instantiate(BatSpawnnerPrefab, this.transform.position, Quaternion.identity);
        }
    }

}
