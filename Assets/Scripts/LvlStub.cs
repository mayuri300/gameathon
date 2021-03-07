using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LvlStub : MonoBehaviour
{
    public GameObject BatSpawnnerPrefab;

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(BatSpawnnerPrefab, this.transform.position, Quaternion.identity);
        this.GetComponent<BoxCollider>().enabled = false;
    }
}
