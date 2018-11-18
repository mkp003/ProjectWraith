using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ArmController class is the controller for all events, attributes, and actions
/// that involve functionality to the arm device and GUI.  There should be an arm controller
/// for both hands but only one will be activated depending on the player's left/right hand 
/// preference.
/// </summary>
public class ArmController : MonoBehaviour {

    // Reference to the hand the arm is on
    [SerializeField]
    private GameObject handReference;

    // Reference to the canvas of the arm display GUI
    [SerializeField]
    private GameObject armDisplay;

    // Is the arm display active?
    private bool isArmDisplayActive = false;

    // Current battery level (%)
    private float batteryLevel = 100.0f;

    // The TextMeshPro text that displays the battery level text
    [SerializeField]
    private GameObject batteryDisplay;

    // Rate at which the battery is depleted when on
    [SerializeField]
    private float batteryDepletionRate;

    // Current reading of how close the main monster is
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


    /// <summary>
    /// CheckToActivate() determines if the arm GUI can be activated based on it's rotation.
    /// </summary>
    /// <returns>True if can be activated, false otherwise</returns>
    private bool CheckToActivate()
    {
        float rotationZ = handReference.transform.rotation.eulerAngles.z;
        float rotationY = handReference.transform.rotation.eulerAngles.y;
        if (rotationZ > 80.0f && rotationZ < 100)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// CheckToDeactivate() will check if the arm GUI should be deactivated based on it's rotation
    /// </summary>
    /// <returns>True if can be deactivated, false otherwise</returns>
    private bool CheckToDeactivate()
    {
        float rotation = handReference.transform.rotation.eulerAngles.z;
        if (rotation < 50.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// ActivateArmGUI() will activate the UI of the arm display.
    /// </summary>
    private void ActivateArmGUI()
    {
        armDisplay.SetActive(true);
        isArmDisplayActive = true;
    }


    /// <summary>
    /// DeactivateArmGUI() will deactivate the UI of the arm display.
    /// </summary>
    private void DeactiveateArmGUI()
    {
        armDisplay.SetActive(false);
        isArmDisplayActive = false;
    }


    /// <summary>
    /// BatteryUsage() is called whenever a portion of the battery's power has been used.
    /// </summary>
    private void BatteryUsage()
    {
        if (!(batteryLevel <= 0))
        {
            if (batteryLevel > batteryDepletionRate)
            {
                batteryLevel = batteryLevel - batteryDepletionRate;
            }
            else
            {
                batteryLevel = 0;
            }
            UpdateBatteryDisplay();
        }
    }


    /// <summary>
    /// NewBatteryAdded() is called when the user adds a new battery to their arm.
    /// This increases the battery level to 100%
    /// </summary>
    private void NewBatteryAdded()
    {
        batteryLevel = 100.0f;
        UpdateBatteryDisplay();
    }


    /// <summary>
    /// UpdateBatteryDisplay() will update the text on the battery display with the current battery level
    /// </summary>
    private void UpdateBatteryDisplay()
    {
        batteryDisplay.GetComponent<TextMesh>().text = "Battery Level: " + batteryLevel + "%";
    }




}
