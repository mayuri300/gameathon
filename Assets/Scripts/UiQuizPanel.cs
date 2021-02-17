using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiQuizPanel : MonoBehaviour
{
    public TMP_Text Question;
    public Button[] Options;
    public int correctAnswer;
    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnDestroy()
    {
        foreach (Button x in Options)
        {
            x.onClick.RemoveAllListeners();
        }
    }

    public void OnOptionClick(Button butt)
    {
        string k = butt.gameObject.name;
        Debug.Log("Button Clicked : " + k);
        Destroy(this.gameObject);

       
    }
}
