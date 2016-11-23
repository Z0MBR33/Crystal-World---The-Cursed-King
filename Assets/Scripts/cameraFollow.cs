using UnityEngine;
using System.Collections;
using System;

public class cameraFollow : MonoBehaviour
{
    private ObjectPool mr;
    private GameObject cameraObj;
    private GameObject playerObj;

    private Vector3 oldDistance;
    private bool wasOutOfLowerBound = false;
    private bool wasOutOfUpperBound = false;

    public int minDistance = 1;
    public int maxDistance = 3;

    private float cameraHeight = 2;
    public int minCameraHeight = 0;
    public int maxCameraHeight = 5;

    // Use this for initialization
    void Start()
    {
        mr = ObjectPool.getObjectPool();
        playerObj = mr.getPlayer();
        cameraObj = mr.getCamera();
        cameraObj.transform.position = playerObj.transform.position + new Vector3(1, cameraHeight, 1);
        StartCoroutine(cameraUpperBound());
        StartCoroutine(cameraLowerBound());
        //StartCoroutine(LookAt());
    }

    void LateUpdate()
    {
    }

    void FixedUpdate()
    {
        oldDistance = Vector3.Scale(playerObj.transform.position, new Vector3(1, 0, 1)) - Vector3.Scale(cameraObj.transform.position, new Vector3(1, 0, 1));
    }

    void OnWillRenderObject()
    {
        cameraObj.transform.LookAt(playerObj.transform.position);
    }

    public IEnumerator LookAt()
    {
        Vector3 oldLookPosition = playerObj.transform.position;
        while (true)
        {
            yield return new WaitForSeconds(.1f);
            cameraObj.transform.LookAt(playerObj.transform.position);
            
        }
    }

    public IEnumerator cameraUpperBound()
    {
        Vector3 newCameraVelo;
        while (true)
        {
            yield return new WaitUntil(() => oldDistance.magnitude > maxDistance | wasOutOfUpperBound);
                FixedUpdate();
                newCameraVelo = oldDistance;
                newCameraVelo.Normalize();
                newCameraVelo *= playerObj.GetComponent<Rigidbody>().velocity.magnitude;
            newCameraVelo *= ((oldDistance.magnitude - maxDistance)*(Math.Abs(oldDistance.magnitude) - maxDistance));
                cameraObj.GetComponent<Rigidbody>().velocity = newCameraVelo;
            if(oldDistance.magnitude > (maxDistance * 1.05))
            {
                wasOutOfUpperBound = false;
            }
        }
    }

    public IEnumerator cameraLowerBound()
    {
        Vector3 newCameraVelo = new Vector3(0, 0, 0);
        while (true)
        {
            yield return new WaitUntil(() => oldDistance.magnitude < minDistance | wasOutOfLowerBound);
                FixedUpdate();
                newCameraVelo = -oldDistance;
                newCameraVelo.Normalize();
                newCameraVelo *= playerObj.GetComponent<Rigidbody>().velocity.magnitude;
                cameraObj.GetComponent<Rigidbody>().velocity = newCameraVelo;
            if(oldDistance.magnitude < (minDistance * 1.05))
                {
                    wasOutOfLowerBound = false;
                }
        }
    }
}