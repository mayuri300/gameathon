using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public float Speed;
    public GameObject ExplosionFX;
    public GameObject HPPrefab;
    private Vector3 offset = new Vector3(0,1,0);
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
            int x = UnityEngine.Random.RandomRange(0, 10);
            if (x >= 7)
                Instantiate(HPPrefab, this.transform.position + offset, Quaternion.identity);

            Instantiate(ExplosionFX, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }
    }
}
