using UnityEngine;
using System.Collections;

public class test_motionControll : MonoBehaviour
{
    private ObjectPool mr;
    private Rigidbody physics;
    private Transform playerPosition;
    private Transform cameraPosition;

    public int speed = 10;

    void Start()
    {
        mr = ObjectPool.getObjectPool();
        physics = GetComponent<Rigidbody>();
        playerPosition = mr.getPlayer().transform;
        cameraPosition = mr.getCamera().transform;
    }

    void Update()
    {
        Vector3 appliedVelo = new Vector3(0, 0, 0);
        appliedVelo += Vector3.Scale(cameraPosition.forward, new Vector3(1, 0, 1)) * Input.GetAxisRaw("Vertical");
        appliedVelo += Vector3.Scale(cameraPosition.right, new Vector3(1, 0, 1))   * Input.GetAxisRaw("Horizontal"); 
        appliedVelo.Normalize();
        appliedVelo *= speed;
        physics.velocity = Vector3.Scale(appliedVelo, new Vector3(1, 0, 1)) + Vector3.Scale(physics.velocity, new Vector3(0, 1, 0));
    }
}
