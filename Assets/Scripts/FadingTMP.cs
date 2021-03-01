using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadingTMP : MonoBehaviour
{
    private CanvasGroup cg;
    public TMP_Text Details;
    public float Duration;

    private void Awake()
    {        
        cg = this.GetComponent<CanvasGroup>();
    }
    public void FadeDetails(string msg)
    {
        Details.text = msg;
    }
    // Start is called before the first frame update
    void Start()
    {
        FadeIn();
        Invoke("FadeOut", Duration);
    }
    public void FadeIn()
    {
        StartCoroutine(FadePanel(cg, cg.alpha, 1));
    }
    public void FadeOut()
    {
        StartCoroutine(FadePanel(cg, cg.alpha, 0));
        Destroy(this.gameObject, 1f);
    }

   public IEnumerator FadePanel(CanvasGroup cg, float start,float end,float duration=0.5f)
    {
        float _started = Time.time; //starting time stamp
        float elapsedTime = Time.time - _started;
        float percentageDone = elapsedTime / duration;
     
        while (true)
        {
            elapsedTime = Time.time - _started;
            percentageDone = elapsedTime / duration;
            float currentValue = Mathf.Lerp(start, end, percentageDone);
            cg.alpha = currentValue;
            if (percentageDone >= 1) 
                break;
            
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
