using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popmenu : MonoBehaviour
{
	public GameObject menu;
    public bool trigger;
    // Start is called before the first frame update
    void Start()
    {
        trigger = true;
        menu.SetActive(trigger);
    }

    // Update is called once per frame
    void Update()
    {
        //menu.SetActive(trigger);
        OVRInput.Update();
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            trigger = !trigger;
            menu.SetActive(trigger);
        }
    }
}
