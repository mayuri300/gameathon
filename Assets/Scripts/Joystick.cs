using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour,IDragHandler,IPointerUpHandler,IPointerDownHandler
{
    public RectTransform JoyBG; //Outer Circle
    public RectTransform joyHandle; //The knob or Handle
    [Range(0, 2f)]
    public float HandleLimit;

    private Vector3 inputVector;
    private Vector2 joyPositioning;
    private Vector2 currentJoyPos;
    public Vector3 JoyPadInputVector => new Vector3(inputVector.x, 0, inputVector.y);

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 joyDirection = eventData.position - RectTransformUtility.WorldToScreenPoint(new Camera(), JoyBG.position);
        inputVector = (joyDirection.magnitude > JoyBG.sizeDelta.x / 2f) ? joyDirection.normalized : joyDirection / (JoyBG.sizeDelta.x / 2f);
        joyHandle.anchoredPosition = (inputVector * JoyBG.sizeDelta.x / 2f) * HandleLimit;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        joyPositioning = eventData.position;
        JoyBG.position = eventData.position;
        joyHandle.anchoredPosition = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        JoyBG.position = currentJoyPos;
        inputVector = Vector3.zero;
        joyHandle.anchoredPosition = Vector3.zero;
    }

    private void Start()
    {
        currentJoyPos = JoyBG.transform.position;
    }
}
