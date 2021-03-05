using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiTutorialTrigger : MonoBehaviour
{
    public TMP_Text Message;
    public Button ExitBTN;

    // Start is called before the first frame update
    void Start()
    {
        ExitBTN.onClick.AddListener(Quit);
    }

    public void Quit()
    {
        Message.text = string.Empty;
        TutorialPlayerActions Player = GameObject.FindObjectOfType<TutorialPlayerActions>();
        Player.inputMode = InputType.JoyStick;
        Destroy(this.gameObject);
    }
}
