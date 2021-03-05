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
    public float MaxBatSpawns;
    public GameObject Level2Portal;

    private BatSpawnerLogic bsl;

    // Start is called before the first frame update
    void Start()
    {
        Scene sc = SceneManager.GetSceneByName(SceneName);
        if (sc.isLoaded)
        {
            LevelType = LevelType.SurvivalLevel;
        }
        else
        {
            LevelType = LevelType.QuizLevel;
        }
        if (LevelType == LevelType.SurvivalLevel)
        {
            //Enable Attacking for Player
            //Enable CountDownTimer ToSpawnBats
            Instantiate(BatSpawnnerPrefab, this.transform.position, Quaternion.identity);
            bsl = BatSpawnnerPrefab.GetComponent<BatSpawnerLogic>();
        }
    }
    private void Update()
    {
        if (bsl.SpawnCount >= MaxBatSpawns)
        {
            Debug.Log("Bat Level DOne!");
            //Spawn Level 2 Portal
        }
    }
}
