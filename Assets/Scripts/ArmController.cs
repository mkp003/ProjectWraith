using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour {

    [SerializeField]
    private GameObject handReference;

    [SerializeField]
    private GameObject armDisplay;

    private bool isArmDisplayActive;

    private float batteryLevel;

    private float geigerCounterRate;

    // Update is called once per frame
    void Update ()
    {
        if (!isArmDisplayActive)
        {
            if (CheckToActivate())
            {
                ActivateArmGUI();
            }
        }
        else
        {
            if (CheckToDeactivate())
            {
                DeactiveateArmGUI();
            }
        }
	}

    private bool CheckToActivate()
    {
        if(handReference.transform.rotation.z > 90.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckToDeactivate()
    {
        if (handReference.transform.rotation.z < 70.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ActivateArmGUI()
    {
        armDisplay.SetActive(true);
        isArmDisplayActive = true;
    }

    private void DeactiveateArmGUI()
    {
        armDisplay.SetActive(false);
        isArmDisplayActive = false;
    }


}
