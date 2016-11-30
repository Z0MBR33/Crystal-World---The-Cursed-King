using UnityEngine;
using System.Collections;

public class Controll : MonoBehaviour
{
    private ObjectPool mr;
    private GameObject camObj;
    private GameObject playerObj;

    private Vector3 lastHorizontalCamVelocity = new Vector3(0, 0, 0);
    private Vector3 lastVerticalCamVelocity = new Vector3(0, 0, 0);
    private float maxSpeed = 10000f;

    public float desiredHorizontalDistance = 4;
    public float minDesiredHorizontalDistance = 3;
    public float maxDesiredHorizontalDistance = 10.5f;

    public float desiredVerticalDistance = 1.6f;

    public int speed = 10;

    void Start()
    {
        mr = ObjectPool.getObjectPool();
        playerObj = mr.getPlayer();
        camObj = mr.getCamera();

        camObj.transform.position = playerObj.transform.position + new Vector3(desiredHorizontalDistance, desiredVerticalDistance, desiredHorizontalDistance);
    }

    void FixedUpdate()
    {
        playerMovement();
        if (Input.GetAxisRaw("Cam_setback") != 0)
        {
            camSetBack();
        }
        camAutoHorizontalMovement();
        camAutoVerticalMovement();
        camManualHorizontalMovement();
        camManualVerticalMovement();
    }

    void playerMovement()
    {
        Vector3 playerVelo = new Vector3(0, 0, 0);
        playerVelo += Vector3.Scale(camObj.transform.forward, new Vector3(1, 0, 1)) * Input.GetAxisRaw("Vertical");
        playerVelo += Vector3.Scale(camObj.transform.right, new Vector3(1, 0, 1)) * Input.GetAxisRaw("Horizontal");
        playerVelo.Normalize();
        playerObj.transform.LookAt(playerObj.transform.position + playerVelo);
        playerVelo *= speed;
        playerObj.GetComponent<CharacterController>().SimpleMove(playerVelo);
    }

    void camAutoHorizontalMovement()
    {
        Vector3 camHorizontalVelo = new Vector3(1, 1, 1);
        Vector3 oldDistance;

        oldDistance = playerObj.transform.position - camObj.transform.position; //Vector cam to player
        oldDistance = Vector3.Scale(oldDistance, new Vector3(1, 0, 1));

        camHorizontalVelo *= (Mathf.Abs(oldDistance.magnitude) - desiredHorizontalDistance);
        camHorizontalVelo *= (Mathf.Abs(oldDistance.magnitude) - desiredHorizontalDistance);
        camHorizontalVelo = new Vector3(Mathf.Clamp(camHorizontalVelo.x, 0, maxSpeed), 0, Mathf.Clamp(camHorizontalVelo.z, 0, maxSpeed));
        camHorizontalVelo = Vector3.Scale(camHorizontalVelo, oldDistance.normalized);

        if (oldDistance.sqrMagnitude < desiredHorizontalDistance * desiredHorizontalDistance)
        {
            camHorizontalVelo *= -1;
        }

        camHorizontalVelo = (camHorizontalVelo + lastHorizontalCamVelocity) / 2;
        lastHorizontalCamVelocity = camHorizontalVelo;
        camObj.GetComponent<Rigidbody>().velocity = Vector3.Scale(camHorizontalVelo, new Vector3(1, 0, 1)) + Vector3.Scale(camObj.GetComponent<Rigidbody>().velocity, new Vector3(0, 1, 0));
    }
    void camAutoVerticalMovement()
    {
        Vector3 camVerticalVelo = new Vector3(0, 0, 0);
        float oldDistance = (playerObj.transform.position.y + desiredVerticalDistance) - camObj.transform.position.y; // vertical distance, "is" to "should be"

        camVerticalVelo = new Vector3(0, 1, 0);
        camVerticalVelo *= (Mathf.Abs(oldDistance));
        camVerticalVelo *= (Mathf.Abs(oldDistance));

        camVerticalVelo = new Vector3(0, Mathf.Clamp(camVerticalVelo.y, 0, maxSpeed), 0);
        if (oldDistance < 0)
        {
            camVerticalVelo *= -1;
        }
        camVerticalVelo = (camVerticalVelo + lastVerticalCamVelocity) / 2;
        lastVerticalCamVelocity = camVerticalVelo;
        camObj.GetComponent<Rigidbody>().velocity = Vector3.Scale(camVerticalVelo, new Vector3(0, 1, 0)) + Vector3.Scale(camObj.GetComponent<Rigidbody>().velocity, new Vector3(1, 0, 1));
    }

    void camManualHorizontalMovement()
    {
        Vector3 camVelo = new Vector3(0, 0, 0);
        camVelo += Vector3.Scale(camObj.transform.right, new Vector3(1, 0, 1)) * Input.GetAxisRaw("Horizontal_camera");

        camObj.GetComponent<Rigidbody>().velocity += camVelo * 10;
    }

    void camManualVerticalMovement()
    {
        desiredHorizontalDistance += 0.5f * Input.GetAxisRaw("Vertical_camera");
        desiredHorizontalDistance = Mathf.Clamp(desiredHorizontalDistance, minDesiredHorizontalDistance, maxDesiredHorizontalDistance);

        desiredVerticalDistance = 0.1f * desiredHorizontalDistance * desiredHorizontalDistance;
    }

    void camSetBack()
    {
            lastHorizontalCamVelocity = new Vector3(0, 0, 0);
            lastVerticalCamVelocity = new Vector3(0, 0, 0);

            camObj.transform.position = playerObj.transform.position - (Vector3.Scale(playerObj.transform.forward, new Vector3(1, 0, 1)) * desiredHorizontalDistance) + new Vector3(0, desiredVerticalDistance, 0);
    }
}