using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

    [SerializeField]
    GameObject doorOne;

    [SerializeField]
    GameObject doorTwo;

    [SerializeField]
    GameObject doorThree;

    [SerializeField]
    GameObject doorFour;

    private Vector3 doorOneRestingLocation;

    private Vector3 doorTwoRestingLocation;

    private Vector3 doorThreeRestingLocation;

    private Vector3 doorFourRestingLocation;


    // Use this for initialization
    void Start () {
        doorOneRestingLocation = doorOne.transform.position;
        doorTwoRestingLocation = doorTwo.transform.position;
        doorThreeRestingLocation = doorThree.transform.position;
        doorFourRestingLocation = doorFour.transform.position;


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
