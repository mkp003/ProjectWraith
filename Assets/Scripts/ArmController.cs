using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    // Is this script on the left or right hand?
    [SerializeField]
    private bool isLeft = true;

    // Flashlight object reference
    [SerializeField]
    private GameObject flashlight;

    // Is the flashlight on?
    private bool isFlashlightOn = false;

    // Coroutine which runs the flashlight
    private IEnumerator runFlashlight;

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
        // Check for flashlight input based on arm
        if (isLeft)
        {
            if (Input.GetButtonDown("Oculus_CrossPlatform_Button4"))
            {
                FlashlightInput();
            }
        }
        else
        {
            if (Input.GetButtonDown("Oculus_CrossPlatform_Button2"))
            {
                FlashlightInput();
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
        if (!isLeft)
        {
            if (rotationZ > 80.0f && rotationZ < 100f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (rotationZ < 280.0f && rotationZ > 260.0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    /// <summary>
    /// CheckToDeactivate() will check if the arm GUI should be deactivated based on it's rotation
    /// </summary>
    /// <returns>True if can be deactivated, false otherwise</returns>
    private bool CheckToDeactivate()
    {
        if (!isLeft)
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
        else
        {
            float rotation = handReference.transform.rotation.eulerAngles.z;
            if (rotation > 310.0f)
            {
                return true;
            }
            else
            {
                return false;
            }
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
    /// FlashlightInput() will either turn on or turn off the flashlight based on 
    /// the current state of the flashlight.
    /// </summary>
    private void FlashlightInput()
    {
        if (isFlashlightOn)
        {
            StopCoroutine(runFlashlight);
            isFlashlightOn = false;
            flashlight.SetActive(false);
        }
        else
        {
            if (batteryLevel > 0)
            {
                isFlashlightOn = true;
                flashlight.SetActive(true);
                runFlashlight = BatteryUsage();
                StartCoroutine(runFlashlight);
            }
            else
            {
                // UI display of failed attempt to turn on flashlight might be cool
            }
        }
    }



    /// <summary>
    /// BatteryUsage() running while the battery is on and simulates the usage of the battery 
    /// while updating the arm display as the power is drained.
    /// </summary>
    IEnumerator BatteryUsage()
    {
        flashlight.SetActive(true);
        float elapsedtime = 0;
        while (isFlashlightOn)
        {
            if(elapsedtime > batteryDepletionRate) {
                if (batteryLevel > 0)
                {
                    batteryLevel = batteryLevel - 1;
                }
                else
                {
                    batteryLevel = 0;
                    isFlashlightOn = false;
                    flashlight.SetActive(false);
                }
                UpdateBatteryDisplay();
                elapsedtime = 0;
            }
            elapsedtime = elapsedtime + Time.deltaTime;
            yield return null;
        }
        yield return null;
    }


    /// <summary>
    /// NewBatteryAdded() is called when the user adds a new battery to their arm.
    /// This increases the battery level to 100%
    /// </summary>
    public void NewBatteryAdded()
    {
        batteryLevel = 100.0f;
        UpdateBatteryDisplay();
    }


    /// <summary>
    /// UpdateBatteryDisplay() will update the text on the battery display with the current battery level
    /// </summary>
    private void UpdateBatteryDisplay()
    {
        batteryDisplay.GetComponent<TextMeshProUGUI>().text = "Battery Level: " + batteryLevel + "%";
    }




}
