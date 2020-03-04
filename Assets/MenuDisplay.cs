using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDisplay : MonoBehaviour
{
    public GameObject menu;
    private bool trigger;
    // Start is called before the first frame update
    void Start()
    {
        trigger = true;
        menu.SetActive(trigger);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.Start))
        {
            trigger = !trigger;
            menu.SetActive(trigger);
        }
    }
}
