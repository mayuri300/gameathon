using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    public float SmoothValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPos = Target.position + Offset;
        Vector3 smoothPos = Vector3.Lerp(desiredPos, this.transform.position, SmoothValue * Time.deltaTime);
        this.transform.position = smoothPos;
    }
}
