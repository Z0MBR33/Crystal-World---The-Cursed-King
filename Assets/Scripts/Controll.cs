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

    public float desiredHorizontalDistance = 9;
    public float minDesiredHorizontalDistance = 3;
    public float maxDesiredHorizontalDistance = 10.5f;

    public float desiredVerticalDistance = 8.1f;

    private Coroutine CoolDownShooting;
    private bool canShoot = true;
    private Coroutine CoolDownCamSetBack;
    private bool canCamSetBack = true;
    private float camSetBackCoolDownInSeconds = 1;

    private bool teleporting = false;
    private Vector3 teleportingStartPoint;
    private Vector3 teleportingEndPoint;

    void Start()
    {
        mr = ObjectPool.getObjectPool();
        playerObj = mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.player);
        camObj = mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.camera);

        camObj.transform.position = playerObj.transform.position + new Vector3(desiredHorizontalDistance, desiredVerticalDistance, desiredHorizontalDistance);
    }

    void Update()
    {
            playerShoot();
    }
    void FixedUpdate()
    {
        if (!teleporting && playerObj.GetComponent<Player>().IsControllable)
        {
            playerMovement();
        }
        if (Input.GetAxisRaw("Cam_setback") != 0 && canCamSetBack)
        {
            camSetBack();
            canCamSetBack = false;
            CoolDownCamSetBack = StartCoroutine(camSetBackCoolDown());
        }
        if (!teleporting)
        {
            camAutoHorizontalMovement();
            camAutoVerticalMovement();
            camManualHorizontalMovement();
            camManualVerticalMovement();
        }
    }

    void playerMovement()
    {
        Vector3 playerVelo = new Vector3(0, 0, 0);
        playerVelo += Vector3.Scale(camObj.transform.forward, new Vector3(1, 0, 1)) * Input.GetAxisRaw("Vertical");
        playerVelo += Vector3.Scale(camObj.transform.right, new Vector3(1, 0, 1)) * Input.GetAxisRaw("Horizontal");
        playerVelo.Normalize();
        playerObj.transform.LookAt(playerObj.transform.position + playerVelo);
        playerVelo *= playerObj.GetComponent<Stats>().speed;
        playerObj.GetComponent<CharacterController>().SimpleMove(playerVelo);
    }

    void playerShoot()
    {
        Vector3 inputVector = new Vector3(0, 0, 0);
        inputVector += Vector3.Scale(camObj.transform.right, new Vector3(1, 0, 1)) * Input.GetAxisRaw("fire_x");
        inputVector += Vector3.Scale(camObj.transform.forward, new Vector3(1, 0, 1)) * Input.GetAxisRaw("fire_z");
        if (inputVector.normalized.sqrMagnitude > 0.2)
        {
            if (canShoot)
            {
                canShoot = false;
                Shot shot = mr.getObject(ObjectPool.categorie.shot, (int)ObjectPool.shot.round).GetComponent<Shot>();
                shot.gameObject.SetActive(true);
                shot.reset(playerObj, inputVector, transform.rotation);
                CoolDownShooting = StartCoroutine(shootCoolDown());
            }
        }
    }

    IEnumerator shootCoolDown()
    {
        yield return new WaitForSeconds(playerObj.GetComponent<Stats>().fireRate);
        StopCoroutine(CoolDownShooting);
        canShoot = true;
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

    IEnumerator camSetBackCoolDown()
    {
        yield return new WaitForSeconds(camSetBackCoolDownInSeconds);
        StopCoroutine(CoolDownCamSetBack);
        canCamSetBack = true;
    }

    public void startTeleporting(Vector3 trajectory)
    {
        teleporting = true;
        teleportingStartPoint = camObj.transform.position;
        teleportingEndPoint = teleportingStartPoint + trajectory;
        updateTeleportProgress(0);
    }

    public void updateTeleportProgress(float percentage)
    {
        if (teleporting)
        {
            camObj.transform.position = Vector3.Lerp(teleportingStartPoint, teleportingEndPoint, Mathf.Clamp01(percentage));
            float viewProgress = (float) Mathf.Sin( Mathf.Lerp(0,Mathf.PI,percentage));
            camObj.GetComponent<Camera>().fieldOfView = Mathf.Lerp(60,90,viewProgress);
        }
    }

    public void endTeleporting()
    {
        teleporting = false;
        camObj.GetComponent<Camera>().fieldOfView = 60;
    }
}