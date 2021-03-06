using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialBatSpawnTrigger : MonoBehaviour
{
    public GameObject BatsSpawnner;
    private BatSpawnerLogic bsl;
    private float cd = 3f;
    private void Start()
    {
        bsl = BatsSpawnner.GetComponent<BatSpawnerLogic>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(BatsSpawnner);
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
    }
    private void Update()
    {
        //Debug.Log("DONE? : " + bsl.FinishedSpawns);
        //if(bsl.FinishedSpawns)
        //{
        //    cd -= Time.deltaTime;
        //    if (cd <= 0)
        //        SceneManager.LoadScene((int)GameLevels.MainMenu);
        //}
    }
}
