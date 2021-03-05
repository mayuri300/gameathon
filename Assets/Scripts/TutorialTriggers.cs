using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggers : MonoBehaviour
{
    public TutorialPlayerActions Player;
    public string Message;
    public GameObject TutorialTriggerTip;
    private void OnTriggerEnter(Collider other)
    {
        //Instantitate tip that dispalys message
 
        UiTutorialTrigger ut = TutorialTriggerTip.GetComponent<UiTutorialTrigger>();
        ut.Message.text = Message;
        Instantiate(TutorialTriggerTip, TutorialManager.Instance.TutorialCanvas.transform);
        Player.inputMode = InputType.None;
        
    }
   
}
