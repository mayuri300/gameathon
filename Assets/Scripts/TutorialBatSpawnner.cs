using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBatSpawnner : MonoBehaviour
{
    public float StartWait;
    public float SpawnDelay;
    public float MaxSpawns;
    public GameObject BatPrefab;
    public GameObject PortalPrefab;
    public GameObject FadingTMPPrefab;
    public PositionInformation[] Pos;
    [Header("Next Portal Spot")]
    public Vector3 NextPortalLocation;
    [Header("Next Level Enum")]
    public GameLevels NextLevel;
    public LevelType NextLevelType;

    private FadingTMP ftp;

    private float spawnCount = 0;
    public float SpawnCount { get { return spawnCount; } }

    private bool finishedSpawns = false;
    public bool FinishedSpawns { get { return finishedSpawns; } }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveSpawner());
        ftp = FadingTMPPrefab.GetComponent<FadingTMP>();
    }

    public IEnumerator MoveSpawner()
    {
        yield return new WaitForSeconds(StartWait);
        while (spawnCount < MaxSpawns)
        {
            int k = Random.Range(0, Pos.Length - 1);
            this.transform.position = Pos[k].SpawnPosition;
            //Debug.Log("Spawnning From : " + Pos[k].SpawnLocation);
            ftp.Details.text = "A Bat has spawnned from : " + Pos[k].SpawnLocation;
            Instantiate(FadingTMPPrefab, TutorialManager.Instance.TutorialCanvas.transform);
            yield return new WaitForSeconds(SpawnDelay);
            Spawn();
        }
        finishedSpawns = true;
        //Spawn Next Portal on specified location here after spawnning
        PortalLogic pl = PortalPrefab.GetComponent<PortalLogic>();
        pl.LevelToLoad = NextLevel;
        pl.NextLevelType = this.NextLevelType;
        pl.NextLevelType = LevelType.QuizLevel;
        ftp.Details.text = "Portal has appeared towards north, kill all Bats and Proceed!.";
        Instantiate(FadingTMPPrefab, TutorialManager.Instance.TutorialCanvas.transform);
        Instantiate(PortalPrefab, NextPortalLocation, PortalPrefab.transform.rotation);
        Destroy(this.gameObject);
    }
    public void Spawn()
    {
        Instantiate(BatPrefab, this.transform.position, Quaternion.identity);
        spawnCount++;
    }

}
[System.Serializable]
public class PositionInformation
{
    public string SpawnLocation;
    public Vector3 SpawnPosition;
}
