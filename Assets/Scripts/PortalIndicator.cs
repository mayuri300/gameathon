using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalIndicator : MonoBehaviour
{
    Scene currentScene;
    private string sceneName;

    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        switch (sceneName)
        {
            case "QuizLvl1":
                this.transform.rotation = Quaternion.LookRotation(GameManager.Instance.Level1Spawnner, Vector3.up);
                break;
            case "BatsLvl1":
                this.transform.rotation = Quaternion.LookRotation(GameManager.Instance.Level2Spawnner, Vector3.up);
                break;
        }
    }
}
