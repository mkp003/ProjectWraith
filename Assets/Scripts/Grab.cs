using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour {
    [SerializeField]
    private bool handedness;


    private bool holding = false;
    // Update is called once per frame



    void Update () {
        
	}
    
    private void OnTriggerStay(Collider collision)
    {
        if (holding == false)
        {
            if (collision.gameObject.tag == "Grabbable")
            {
                if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) != 0 && handedness)
                {
                    collision.gameObject.transform.SetParent(gameObject.transform);
                    holding = true;

                }else if(OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) != 0 && !handedness)
                {
                    collision.gameObject.transform.SetParent(gameObject.transform);
                    holding = true;
                }
            }
        }
        else
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) == 0 && handedness)
            {
                Debug.Log("end");
                gameObject.transform.GetChild(0).transform.parent = null;
                holding = false;
            }
            else if(OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) != 0 && !handedness)
            {
                Debug.Log("end");
                gameObject.transform.GetChild(0).transform.parent = null;
                holding = false;
            }
        }
    }
}
