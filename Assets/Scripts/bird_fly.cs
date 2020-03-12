using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird_fly : MonoBehaviour
{
    public GameObject birds;
    public float speed;
    public int loop;
    private int cnt = 250;
    // Start is called before the first frame update
    void Start()
    {
        cnt = loop;
    }

    // Update is called once per frame
    void Update()
    {
        if (cnt == 0)
        {
            cnt = loop;
            birds.transform.forward = birds.transform.forward * -1;
        }

        float step = speed * Time.deltaTime;
        birds.transform.position += birds.transform.forward * step;
        cnt -= 1;
    }
}
