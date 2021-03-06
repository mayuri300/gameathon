﻿using System.Collections;
using UnityEngine;


public class BatSpawnerLogic : MonoBehaviour
{
    public float StartWait;
    public float SpawnDelay;
    public float MaxSpawns;
    public GameObject BatPrefab;
    public PositionInfo[] Pos;
    public GameObject PortalPrefab;
    [Header("Next Portal Spot")]
    public Vector3 NextPortalLocation;
    [Header("Next Level Enum")]
    public GameLevels NextLevel;

    private float spawnCount = 0;
    public float SpawnCount { get { return spawnCount; } }

    private bool finishedSpawns = false;
    public bool FinishedSpawns { get { return finishedSpawns; } }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveSpawner());
    }

    public IEnumerator MoveSpawner()
    {
        yield return new WaitForSeconds(StartWait);
        while (spawnCount<MaxSpawns)
         {
            int k = Random.Range(0, Pos.Length - 1);
            this.transform.position = Pos[k].SpawnPosition;
            Debug.Log("Spawnning From : " + Pos[k].SpawnLocation);
            yield return new WaitForSeconds(SpawnDelay);
            Spawn();
         }
        finishedSpawns = true;
        //Spawn Next Portal on specified location here after spawnning
        Instantiate(PortalPrefab, NextPortalLocation, PortalPrefab.transform.rotation);
        PortalLogic pl = PortalPrefab.GetComponent<PortalLogic>();
        pl.LevelToLoad = NextLevel;
        Destroy(this.gameObject);
    }
    public void Spawn()
    {
        Instantiate(BatPrefab, this.transform.position, Quaternion.identity);
        spawnCount++;
    }
    private void Update()
    {
    }
}
[System.Serializable]
public class PositionInfo
{
    public string SpawnLocation;
    public Vector3 SpawnPosition;
}