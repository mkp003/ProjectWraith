using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

    [SerializeField]
    float doorSpeed = 1;

    [SerializeField]
    GameObject doorOne;

    [SerializeField]
    GameObject doorTwo;

    [SerializeField]
    GameObject doorThree;

    [SerializeField]
    GameObject doorFour;

    [SerializeField]
    SphereCollider opener;

    [SerializeField]
    SphereCollider closer;

    private Vector3 doorOneRestingLocation;

    private Vector3 doorTwoRestingLocation;

    private Vector3 doorThreeRestingLocation;

    private Vector3 doorFourRestingLocation;

    private bool doorOneActive = false;
    private bool doorTwoActive = false;
    private bool doorThreeActive = false;
    private bool doorFourActive = false;

    public bool closed = false;

    private bool stopOtherDoors;

    private bool test = true;

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




    }
	

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerString))
        {
            Debug.Log("player hit");
            StartCoroutine(lerpOpen(doorOne, doorOneRestingLocation, doorOne.transform.position + new Vector3(1.5f, 0, 0), doorSpeed));
            StartCoroutine(lerpOpen(doorTwo, doorTwoRestingLocation, doorTwo.transform.position + new Vector3(-1.5f, 0, 0), doorSpeed));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(playerString))
        {
            Debug.Log("player hit");
            StartCoroutine(lerpClose(doorOne, doorOneRestingLocation, doorOne.transform.position + new Vector3(1.5f, 0, 0), doorSpeed));
            StartCoroutine(lerpClose(doorTwo, doorTwoRestingLocation, doorTwo.transform.position + new Vector3(-1.5f, 0, 0), doorSpeed));
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
    }

}
