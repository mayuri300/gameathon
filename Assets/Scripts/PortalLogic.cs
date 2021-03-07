using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalLogic : MonoBehaviour
{
    public LevelType NextLevelType;
    public GameLevels LevelToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerActions pa = other.GetComponent<PlayerActions>();
            if (pa == null)
                return;
            else
            {
                pa.DisableIndicator();
            }
        }
    }
}
