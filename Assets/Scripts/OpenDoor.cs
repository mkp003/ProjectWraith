using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

    [SerializeField]
    float doorSpeed = 1;

    [SerializeField]
    GameObject doorOne;

    [SerializeField]
    Vector3 openLocation1;//the localSpace Vector that the door will open to

    [SerializeField]
    GameObject doorTwo;

    [SerializeField]
    Vector3 openLocation2;

    [SerializeField]
    GameObject doorThree;

    [SerializeField]
    Vector3 openLocation3;

    [SerializeField]
    GameObject doorFour;

    [SerializeField]
    Vector3 openLocation4;

    [SerializeField]
    AudioClip openSound;

    [SerializeField]
    AudioClip closeSound;

    private AudioSource soundOutput;

    private Vector3 doorOneRestingLocation;

    private Vector3 doorTwoRestingLocation;

    private Vector3 doorThreeRestingLocation;

    private Vector3 doorFourRestingLocation;

    private bool doorOneActive = false;
    private bool doorTwoActive = false;
    private bool doorThreeActive = false;
    private bool doorFourActive = false;

    private bool stopOtherDoors;

    private string playerString = "Player";

    // Use this for initialization
    void Start () {
        if (doorOne)
        {
            doorOneRestingLocation = doorOne.transform.position;
            doorOneActive = true;
        }

        if (doorTwo)
        {
            doorTwoRestingLocation = doorTwo.transform.position;
            doorTwoActive = true;
        }

        if (doorThree)
        {
            doorThreeRestingLocation = doorThree.transform.position;
            doorThreeActive = true;
        }

        if (doorFour)
        {
            doorFourRestingLocation = doorFour.transform.position;
            doorFourActive = true;
        }


        try//setup audio source
        {
            soundOutput = this.GetComponent<AudioSource>();
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }
        




    }
	

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerString))
        {
            Debug.Log("player hit");
            if (doorOneActive)
            {
                StartCoroutine(lerpOpen(doorOne, doorOneRestingLocation, doorOne.transform.position + openLocation1, doorSpeed));//1.5f
                soundOutput.PlayOneShot(openSound);
            }
            if (doorTwoActive)
            {
                StartCoroutine(lerpOpen(doorTwo, doorTwoRestingLocation, doorTwo.transform.position + openLocation2, doorSpeed));//-1.5f
            }
            if (doorThreeActive)
            {
                StartCoroutine(lerpOpen(doorThree, doorThreeRestingLocation, doorThree.transform.position + openLocation3, doorSpeed));//-1.5f
            }
            if (doorFourActive)
            {
                StartCoroutine(lerpOpen(doorFour, doorFourRestingLocation, doorFour.transform.position + openLocation4, doorSpeed));//-1.5f
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(playerString))
        {
            Debug.Log("player hit");
            if (doorOneActive)
            {
                StartCoroutine(lerpClose(doorOne, doorOneRestingLocation, doorOne.transform.position + openLocation1, doorSpeed));//1.5f
                soundOutput.PlayOneShot(closeSound);
            }
            if (doorTwoActive)
            {
                StartCoroutine(lerpClose(doorTwo, doorTwoRestingLocation, doorTwo.transform.position + openLocation2, doorSpeed));//-1.5f
            }
            if (doorThreeActive)
            {
                StartCoroutine(lerpClose(doorThree, doorThreeRestingLocation, doorThree.transform.position + openLocation3, doorSpeed));//-1.5f
            }
            if (doorFourActive)
            {
                StartCoroutine(lerpClose(doorFour, doorFourRestingLocation, doorFour.transform.position + openLocation4, doorSpeed));//-1.5f
            }
        }
    }




    private IEnumerator lerpOpen(GameObject doorPart, Vector3 restingPlace, Vector3 endPos, float time)
    {
        stopOtherDoors = true;
        yield return new WaitForEndOfFrame();
        stopOtherDoors = false;
        float elapsedTime = 0;
        Vector3 startPos = doorPart.transform.position;

        Vector3 velocity = Vector3.zero;

            while (elapsedTime < time + 1f)
            {
                //doorPart.transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / time));
                if(stopOtherDoors)
                {
                    Debug.Log("doors are stopped");
                    yield break;
                }
                startPos = doorPart.transform.position;
                doorPart.transform.position = Vector3.SmoothDamp(startPos, endPos, ref velocity, time);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

    }


    private IEnumerator lerpClose(GameObject doorPart, Vector3 restingPlace, Vector3 endPos, float time)
    {
        stopOtherDoors = true;
        yield return new WaitForEndOfFrame();
        stopOtherDoors = false;
        float elapsedTime = 0;
        Vector3 startPos = doorPart.transform.position;

        Vector3 velocity = Vector3.zero;

        while (elapsedTime < time)
        {
            if (stopOtherDoors)
            {
                Debug.Log("doors are stopped");
                yield break;
            }
            doorPart.transform.position = Vector3.Lerp(startPos, restingPlace, (elapsedTime / time));
            //startPos = doorPart.transform.position;
            //doorPart.transform.position = Vector3.SmoothDamp(startPos, restingPlace, ref velocity, time);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        doorPart.transform.position = restingPlace;
    }

}
