using UnityEngine;
using System.Collections;

public class Controll : MonoBehaviour
{
    private ObjectPool mr;
    private GameObject camObj;
    private GameObject playerObj;

    private Vector3 lastHorizontalCamVelocity = new Vector3(0, 0, 0);
    private Vector3 lastVerticalCamVelocity = new Vector3(0, 0, 0);

    public float desiredHorizontalDistance = 7;
    public float desiredVerticalDistance = 4;

    public int speed = 10;

    void Start()
    {
        mr = ObjectPool.getObjectPool();
        playerObj = mr.getPlayer();
        camObj = mr.getCamera();

        camObj.transform.position = playerObj.transform.position + new Vector3(1, 1, 1);
    }

    void FixedUpdate()
    {
        playerMovement();
        camHorizontalMovement();
        camVerticalMovement();
    }

    void playerMovement()
    {
        Vector3 playerVelo = new Vector3(0, 0, 0);
        playerVelo += Vector3.Scale(camObj.transform.forward, new Vector3(1, 0, 1)) * Input.GetAxisRaw("Vertical");
        playerVelo += Vector3.Scale(camObj.transform.right, new Vector3(1, 0, 1)) * Input.GetAxisRaw("Horizontal");
        playerVelo.Normalize();
        playerVelo *= speed;
        playerObj.GetComponent<CharacterController>().SimpleMove(playerVelo);
    }
    void camHorizontalMovement()
    {
        Vector3 camHorizontalVelo = new Vector3(0, 0, 0);
        Vector3 oldDistance;

        oldDistance = playerObj.transform.position - camObj.transform.position; //Vector cam to player
        oldDistance = Vector3.Scale(oldDistance, new Vector3(1, 0, 1));
        camObj.transform.LookAt(camObj.transform.position + oldDistance);
        camHorizontalVelo = camObj.transform.forward;
        camHorizontalVelo *= (Mathf.Abs(oldDistance.magnitude) - desiredHorizontalDistance);
        camHorizontalVelo *= (Mathf.Abs(oldDistance.magnitude) - desiredHorizontalDistance);

        if (oldDistance.sqrMagnitude < desiredHorizontalDistance * desiredHorizontalDistance)
        {
            camHorizontalVelo *= -1;
        }

        camHorizontalVelo = (camHorizontalVelo + lastHorizontalCamVelocity) / 2;
        lastHorizontalCamVelocity = camHorizontalVelo;
        camObj.GetComponent<Rigidbody>().velocity = Vector3.Scale(camHorizontalVelo, new Vector3(1, 0, 1)) + Vector3.Scale(camObj.GetComponent<Rigidbody>().velocity, new Vector3(0, 1, 0));
    }
    void camVerticalMovement()
    {
        Vector3 camVerticalVelo = new Vector3(0, 0, 0);
        float oldDistance = (playerObj.transform.position.y + desiredVerticalDistance) - camObj.transform.position.y; // vertical distance, "is" to "should be"

        camVerticalVelo = new Vector3(0, 1, 0);
        camVerticalVelo *= (Mathf.Abs(oldDistance));
        camVerticalVelo *= (Mathf.Abs(oldDistance));

        if (oldDistance < 0 )
        {
            camVerticalVelo *= -1;
        }

        camVerticalVelo = (camVerticalVelo + lastVerticalCamVelocity) / 2;
        lastVerticalCamVelocity = camVerticalVelo;
        camObj.GetComponent<Rigidbody>().velocity = Vector3.Scale(camVerticalVelo, new Vector3(0, 1, 0)) + Vector3.Scale(camObj.GetComponent<Rigidbody>().velocity, new Vector3(1, 0, 1));
    }
}