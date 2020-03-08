using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class man_walk : MonoBehaviour
{
    public float speed = 3.0f;
    public GameObject man;
    
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed*Time.deltaTime;
        man.transform.position += man.transform.forward * step;
    }
}
