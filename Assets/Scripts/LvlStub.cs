using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LvlStub : MonoBehaviour
{
    public GameObject BatSpawnnerPrefab;
    public GameObject FadingTMPPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            AudioManager.Instance.PlayMusic(MusicEffectsType.Survival);
            Instantiate(BatSpawnnerPrefab, this.transform.position, Quaternion.identity);
            BatSpawnerLogic bsl = BatSpawnnerPrefab.GetComponent<BatSpawnerLogic>();
            FadingTMP ftp = FadingTMPPrefab.GetComponent<FadingTMP>();
            ftp.Details.text = "Virus will start spawnning in : " + bsl.StartWait + " seconds";
            Instantiate(FadingTMPPrefab, GameManager.Instance.WorldCanvas.transform);
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
