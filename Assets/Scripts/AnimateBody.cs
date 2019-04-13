using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateBody : MonoBehaviour {

    [SerializeField]
    private Transform body;
    [SerializeField]
    private Transform leftShoulder;
    [SerializeField]
    private Transform rightShoulder;
    [SerializeField]
    private Transform leftElbow;
    [SerializeField]
    private Transform rightElbow;
    [SerializeField]
    private Transform lefthand;
    [SerializeField]
    private Transform rightHand;
    [SerializeField]
    private Transform head;
    [SerializeField]
    private Transform neck;

    private Vector3 bodyPos;
    private Vector3 lSPos;
    private Vector3 rSPos;
    private Vector3 lEPos;
    private Vector3 rEPos;
    private Vector3 lHPos;
    private Vector3 rHPos;
    private Vector3 headPos;
    private Vector3 neckPos;

    [SerializeField]
    private Vector3 locChange;

    private float time = 0;

    private float hoverDirection = 1;



    // Use this for initialization
    void Start ()
    {
        locChange = new Vector3(0,0.1f,0);


        bodyPos = body.position;
        lSPos = leftShoulder.position;
        rSPos = rightShoulder.position;
        lEPos = leftElbow.position;
        rEPos = rightElbow.position;
        lHPos = lefthand.position;
        rHPos = rightHand.position;
        headPos = head.position;
        neckPos = neck.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        time += hoverDirection/200;


        Hover(time, hoverDirection);

        if (time >= 1)
        {
            hoverDirection = -1;
        }

        if (time <= 0)
        {
            hoverDirection = 1;
        }
    }

    public void MoveArm()
    {

    }

    public void MoveTowards(float timeStep, Vector3 location)
    {
        body.position = Vector3.Lerp(bodyPos + location, bodyPos, timeStep);
    }


    public void Hover(float timeStep, float direction)
    {
        if(direction <= 0)
        {
            body.position = Vector3.Lerp(bodyPos + locChange, bodyPos, timeStep);
        }
        else
        {
            body.position = Vector3.Lerp(bodyPos + locChange, bodyPos, timeStep);
        }
    }
}
