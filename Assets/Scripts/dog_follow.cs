using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dog_follow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject man;
    public GameObject dog;
    public float speed = 1.0f;
    public float rotationSpeed = 10.0f;
    void Start()
    {
        //man = GameObject.Find("man");
        //dog = GameObject.Find("dog");
        //dog.transform.rotation = man.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed*Time.deltaTime;
        //dog.transform.rotation = Quaternion.Slerp(dog.transform.rotation, Quaternion.LookRotation(man.transform.position - dog.transform.position), rotationSpeed * Time.deltaTime);
        dog.transform.position = Vector3.MoveTowards(dog.transform.position, man.transform.position, step);
    }
}
