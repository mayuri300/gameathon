using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TipType {  None,QuizTip,InstructionTip};
public class UiTipPanel : MonoBehaviour
{
    public TMP_Text TipDetail;
    public Button ExitBTN;
    // Start is called before the first frame update
    void Start()
    {
        ExitBTN.onClick.AddListener(QuitTip);
    }
    private void OnDestroy()
    {
        ExitBTN.onClick.RemoveAllListeners();
    }
    public void QuitTip()
    {
        Destroy(this.gameObject);
    }
}
