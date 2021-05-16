using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiQuitPanel : MonoBehaviour
{
    public Button YesBTN;
    public Button NoBTN;

    // Start is called before the first frame update
    void Start()
    {
        YesBTN.onClick.AddListener(delegate { Application.Quit(); });
        NoBTN.onClick.AddListener(delegate { Destroy(this.gameObject); });
    }
    private void OnDestroy()
    {
        YesBTN.onClick.RemoveAllListeners();
        NoBTN.onClick.RemoveAllListeners();
    }

}
