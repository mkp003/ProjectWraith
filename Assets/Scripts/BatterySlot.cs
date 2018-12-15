using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The function of this class to to act as a slot which will take in a new 
/// battery and update the battery level of the arm's flashlight.  The new battery
/// is destroyed in the process.
/// </summary>
public class BatterySlot : MonoBehaviour {

    [SerializeField]
    private GameObject armController;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Battery")
        {
            armController.GetComponent<ArmController>().NewBatteryAdded();
            Destroy(other.gameObject);
        }
    }
}
