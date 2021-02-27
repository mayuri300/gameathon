using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawnerLogic : MonoBehaviour
{
    public float StartWait;
    public float SpawnDelay;
    public float MaxSpawns;
    public GameObject BatPrefab;
    public PositionInfo[] Pos;

    private float spawnCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveSpawner());
    }

    public IEnumerator MoveSpawner()
    {
        while (spawnCount<=MaxSpawns)
        {

             yield return new WaitForSeconds(StartWait);
            int k = Random.Range(0, Pos.Length - 1);
            this.transform.position = Pos[k].SpawnPosition;
            Debug.Log("Spawnning From : " + Pos[k].SpawnLocation);
            yield return new WaitForSeconds(SpawnDelay);
            Spawn();

  
        }
    }
    public void Spawn()
    {
        Instantiate(BatPrefab, this.transform.position, Quaternion.identity);
        spawnCount++;
    }
}
[System.Serializable]
public class PositionInfo
{
    public string SpawnLocation;
    public Vector3 SpawnPosition;
}