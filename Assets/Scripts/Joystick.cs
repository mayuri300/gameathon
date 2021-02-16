using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour,IDragHandler,IPointerUpHandler,IPointerDownHandler
{
    private Image joyBG;
    private Image joyPad;
    private Vector3 inputVector;

    public Vector3 JoyPadInputVector => new Vector3(inputVector.x, 0, inputVector.z);

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joyBG.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / joyBG.rectTransform.sizeDelta.x);
            pos.y = (pos.y / joyBG.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f ? inputVector.normalized : inputVector);

            joyPad.rectTransform.anchoredPosition = new Vector3(inputVector.x * (joyBG.rectTransform.sizeDelta.x / 3), inputVector.z * (joyBG.rectTransform.sizeDelta.y / 3),0f);
        }
     
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        joyBG.rectTransform.anchoredPosition = Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector3.zero;
        joyPad.rectTransform.anchoredPosition = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        joyBG = GetComponent<Image>();
        joyPad = transform.GetChild(0).GetComponent<Image>();
        
    }

    void Update()
    {
        int touchCount = Input.touchCount;
        if (touchCount >=1)
        {
            Touch t = Input.GetTouch(0);
            TouchPhase phase = t.phase;
            switch (phase)
            {
                case TouchPhase.Began:
                    joyBG.gameObject.SetActive(true);
                    joyBG.transform.position = t.position;
                    break;
                case TouchPhase.Ended:
                   // joyBG.gameObject.SetActive(false);
                    break;
                case TouchPhase.Canceled:
                    break;
            }
        }
    }

}
