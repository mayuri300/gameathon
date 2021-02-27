using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public float Speed;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 7f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += this.transform.forward * Speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bat")
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }
    }
}
