using UnityEngine;
using System.Collections;

public class Controll : MonoBehaviour
{
    private ObjectPool mr;
    private GameObject camObj;
    private GameObject playerObj;

    private Vector3 oldDistance;
    private Vector3 lastCamVelocity = new Vector3(0, 0, 0);
    private Vector3 camVelocity = new Vector3(0, 0, 0);
    public float desiredDistance = 7;

    public int speed = 10;

    void Start()
    {
        mr = ObjectPool.getObjectPool();
        playerObj = mr.getPlayer();
        camObj = mr.getCamera();
    }

    void FixedUpdate()
    {

        Vector3 playerVelo = new Vector3(0, 0, 0);
        Vector3 camVelo = new Vector3(0, 0, 0);

        playerVelo += Vector3.Scale(camObj.transform.forward, new Vector3(1, 1, 1)) * Input.GetAxisRaw("Vertical");
        playerVelo += Vector3.Scale(camObj.transform.right, new Vector3(1, 1, 1)) * Input.GetAxisRaw("Horizontal");
        playerVelo.Normalize();
        playerVelo *= speed;
        playerObj.GetComponent<CharacterController>().SimpleMove(playerVelo);

        oldDistance = playerObj.transform.position - camObj.transform.position;
        camVelocity = camObj.transform.forward;
        camVelocity *= (Mathf.Abs(oldDistance.magnitude) - desiredDistance);
        camVelocity *= (Mathf.Abs(oldDistance.magnitude) - desiredDistance);
        //camVelocity *= speed;
        //camVelo *= 0.1f;
        if (oldDistance.sqrMagnitude < desiredDistance * desiredDistance)
        {
            camVelocity *= -1;
        }
        camVelocity = (camVelocity + lastCamVelocity)/2;
        lastCamVelocity = camVelocity;
        camObj.GetComponent<Rigidbody>().velocity = camVelocity;
    }
}
