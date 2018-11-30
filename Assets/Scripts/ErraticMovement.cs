using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ErraticMovement : MonoBehaviour {

    [SerializeField]
    private float speed = 0.05f;

    [SerializeField]
    private float erratic;

    [SerializeField]
    private float radius;

    [SerializeField]
    private int axis;

    private Vector3 startingPoint;
    private Vector3 endPoint;
    private Vector3 midPoint;

    private Vector3 offset;

    private float distanceFromMid = 0;

    private float progress = 0;

    private Vector3 temp;

    private bool returning = false;

    // Use this for initialization
    void Start () {
        startingPoint = gameObject.transform.position;
        if(axis == 0)
        {
            endPoint = startingPoint + new Vector3(radius*2, 0, 0);
        }else if(axis == 1)
        {
            endPoint = startingPoint + new Vector3(0, radius*2, 0);
        }
        else
        {
            endPoint = startingPoint + new Vector3(0, 0, radius*2);
        }
        midPoint = endPoint + new Vector3(startingPoint.x - endPoint.x, startingPoint.y - endPoint.y, startingPoint.z - endPoint.z)/2;
	}
	
	// Update is called once per frame
	void Update () {
        Method2();
	}

    private void Method2()
    {
        progress += speed;
        float temp;
        if (axis == 0)
        {
            temp = Mathf.Sin(this.gameObject.transform.position.x - endPoint.x);
            offset = new Vector3(temp, 0, 0);
        }
        else if (axis == 1)
        {
            temp = Mathf.Sin(this.gameObject.transform.position.y - endPoint.y);
            offset = new Vector3(0, temp, 0);
        }
        else
        {
            temp = Mathf.Sin(this.gameObject.transform.position.z - endPoint.z);
            offset = new Vector3(0, 0, temp);
        }

        gameObject.transform.position = Vector3.Lerp(startingPoint, endPoint, progress) + offset;
        if (progress >= 1 && speed > 0)
        {
            speed = -speed;
            returning = true;
        }
        if (progress <= 0 && speed < 0)
        {
            speed = -speed;
            returning = false;
        }
    }

    private void Method1()
    {
        progress += speed;
        if (axis == 0)
        {
            distanceFromMid = Mathf.Abs(gameObject.transform.position.x - midPoint.x);
            float temp = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(distanceFromMid, 2));
            offset = new Vector3(0, temp, 0);
            if (returning)
                offset = -offset;
        }
        else if (axis == 1)
        {
            distanceFromMid = Mathf.Abs(gameObject.transform.position.y - midPoint.y);
            float temp = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(distanceFromMid, 2));
            offset = new Vector3(0, 0, temp);
            if (returning)
                offset = -offset;
        }
        else
        {
            distanceFromMid = Mathf.Abs(gameObject.transform.position.z - midPoint.z);
            float temp = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(distanceFromMid, 2));
            offset = new Vector3(temp, 0, 0);
            if (returning)
                offset = -offset;
        }
        gameObject.transform.position = Vector3.Lerp(startingPoint, endPoint, progress) + offset;
        if (progress >= 1 && speed > 0)
        {
            speed = -speed;
            returning = true;
        }
        if (progress <= 0 && speed < 0)
        {
            speed = -speed;
            returning = false;
        }
    }
}
