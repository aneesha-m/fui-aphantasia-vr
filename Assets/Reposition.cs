using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
	public GameObject obj;
	public bool selected = false;
    void Start()
    {
        //transform.position = m_vecStart;
    }
 
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.F))
    //     {
    //         RaycastHit HitInfo;
    //         if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out HitInfo, range))
    //         {
    //             var angle = fpsCam.transform.rotation.eulerAngles;
    //             transform.rotation = Quaternion.Euler(0f, angle.y, 0f);
    //             transform.position = (HitInfo.point);
    //         }
    //     }
    // }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || OVRInput.Get(OVRInput.RawButton.LHandTrigger) || OVRInput.Get(OVRInput.RawButton.RHandTrigger))
        {
            RaycastHit hit;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
            	//if(hit.transform.CompareTag ("moveable")){
		    	
	            	Debug.Log ("Raycast has hit object");
	                //hit.rigidbody.AddForceAtPosition(ray.direction * pokeForce, hit.point);
	                Debug.Log ("Object position before: " + obj.transform.position);
	                //obj.transform.Translate(hit.point*Time.deltaTime);
	                obj.transform.position = (hit.point);
	                Debug.Log ("Object position after: " + obj.transform.position);
            	//}
            }
        }
    }
 
//     static readonly Vector3 m_vecStart = new Vector3(0, -10, 0);
 
// #pragma warning disable 649
//     [SerializeField] [Range(5f, 15f)] float range = 10f;
//     [SerializeField] Camera fpsCam;
// #pragma warning restore 649
}

