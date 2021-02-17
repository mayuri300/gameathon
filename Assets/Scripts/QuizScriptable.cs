using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="QuizScriptable", menuName="Scriptable/Quiz")]
public class QuizScriptable : ScriptableObject
{
    public QuestionType Type;
    public string Question;
    public string[] Options;
    public int CorrectOption;
}
