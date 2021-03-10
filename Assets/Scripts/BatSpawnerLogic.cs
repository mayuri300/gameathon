using System.Collections;
using UnityEngine;


public class BatSpawnerLogic : MonoBehaviour
{
    public float StartWait;
    public float SpawnDelay;
    public float MaxSpawns;
    public GameObject BatPrefab;
    public GameObject PortalPrefab;
    public GameObject FadingTMPPrefab;
    public PositionInfo[] Pos;
    [Header("Next Portal Spot")]
    public Vector3 NextPortalLocation;
    [Header("Next Level Enum")]
    public GameLevels NextLevel;
    public LevelType NextLevelType;

    private FadingTMP ftp;

    private float spawnCount = 0;
    public float SpawnCount { get { return spawnCount; } }

    private bool killedAllBats = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveSpawner());
        ftp = FadingTMPPrefab.GetComponent<FadingTMP>();
    }

    public IEnumerator MoveSpawner()
    {
        yield return new WaitForSeconds(StartWait);
        while (spawnCount<MaxSpawns)
         {
            int k = Random.Range(0, Pos.Length - 1);
            this.transform.position = Pos[k].SpawnPosition;
            //Debug.Log("Spawnning From : " + Pos[k].SpawnLocation);
            ftp.Details.text = "A Bat will spawn from : " + Pos[k].SpawnLocation;
            Instantiate(FadingTMPPrefab, GameManager.Instance.WorldCanvas.transform);
            yield return new WaitForSeconds(SpawnDelay);
            Spawn();
         }
    }
    public void Spawn()
    {
        Instantiate(BatPrefab, this.transform.position, Quaternion.identity);
        spawnCount++;
    }
    private void Update()
    {
        if (MyData.BatsKilled >= MaxSpawns)
        {
            killedAllBats = true;
        }
        else
            killedAllBats = false;

        if (killedAllBats)
        {
            PortalLogic pl = PortalPrefab.GetComponent<PortalLogic>();
            pl.LevelToLoad = NextLevel;
            pl.NextLevelType = this.NextLevelType;
            pl.NextLevelType = LevelType.QuizLevel;
            ftp.Details.text = "Portal has appeared towards north, Please Proceed to Next Level!.";
            Instantiate(FadingTMPPrefab, GameManager.Instance.WorldCanvas.transform);
            Instantiate(PortalPrefab, NextPortalLocation, PortalPrefab.transform.rotation);
            MyData.BatsKilled = 0;
            Destroy(this.gameObject);
        }
    }
}
[System.Serializable]
public class PositionInfo
{
    public string SpawnLocation;
    public Vector3 SpawnPosition;
}