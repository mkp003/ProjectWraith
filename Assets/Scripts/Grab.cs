using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour {
    [SerializeField]
    private bool handedness;

    [SerializeField]
    private Grab otherHand;

    public bool holding = false;
    // Update is called once per frame

    private GameObject heldObject;

    private Transform oldParent;

    [SerializeField]
    private float grabRadius = 0.1f;

    [SerializeField]
    private int grabLayer = 10;

    private void Start()
    {
        
    }

    void Update () {
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) != 0 && handedness == false)
        {
            
            if(holding == false)
            {

                GrabObject();
                holding = true;
            }
        }
        else if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) != 0 && handedness == true)
        {
            if (holding == false)
            {
                GrabObject();
                holding = true;
            }
        }

        if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) == 0 && handedness == false)
        {
            if (holding == true)
            {
                Drop();
                holding = false;
            }
        }
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) == 0 && handedness == true)
        {
            if (holding == true)
            {
                Drop();
                holding = false;
            }
        }
    }

    void Drop()
    {
        Debug.Log("Drop");
        //Change the boolean so the object is not grabbed anymore
        this.holding = false;

        //If the object is attached change the transform to null so it keeps it's position
        if (this.heldObject != null)
        {
            this.heldObject.transform.parent = this.oldParent;
            this.heldObject = null;
        }
        
    }

    private void GrabObject()
    {
        Debug.Log("Grab");
        // Create an array for the amount of objects grabbed
        RaycastHit[] objectHits;

        // Store the amount of objects that are gathered from the Sphere Cast
        objectHits = Physics.SphereCastAll(transform.position, grabRadius, transform.forward, 0F, grabLayer);

        // Find the closes object from the SphereCast hits, if any exist
        if (objectHits.Length > 0)
        {
            int closestHit = 0;
            // Get the object that is closest to the center of the raycast
            for (int i = 0; i < objectHits.Length; i++)
            {
                if (objectHits[i].distance < objectHits[closestHit].distance)
                    closestHit = i;
            }

            // Check to see if the other hand is holding something
            // If it is, and it's the same as the closest raycast object, don't grab
            if (otherHand.holding)
            {
                if (otherHand.heldObject != objectHits[closestHit].transform.gameObject)
                {
                    AssignGrabbedObject(objectHits[closestHit].transform.gameObject);
                }
            }
            else
            {
                AssignGrabbedObject(objectHits[closestHit].transform.gameObject);
            }
            
        }
    }

    void AssignGrabbedObject(GameObject closest)
    {
        this.holding = true;
        this.heldObject = closest;
        this.heldObject.GetComponent<Rigidbody>().freezeRotation = true;
        this.oldParent = heldObject.transform.parent;
        //this.grabbedObject.transform.position = transform.position;
        this.heldObject.transform.parent = transform;
    }

}
