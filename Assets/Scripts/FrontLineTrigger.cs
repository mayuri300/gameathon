using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontLineTrigger : MonoBehaviour
{
    public QuestionType Type;
    public BoxCollider QuizTrigger;
    public GameObject Indicator;
    public GameObject TipObject;

    public void RemoveIndicator()
    {
        Destroy(Indicator);
    }

}
