using UnityEngine;
using System.Collections;

public class test_motionControll : MonoBehaviour {
    private ObjectPool mr;
    private Rigidbody physics;
    private Transform playerPosition;
    private Transform cameraPosition;
	void Start () {        
        mr = ObjectPool.getObjectPool();
        physics = GetComponent<Rigidbody>();
        playerPosition = mr.getPlayer().transform;
        cameraPosition = mr.getCamera().transform;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 cameraToPlayer = playerPosition.position - cameraPosition.position;
        Vector3 appliedForce = new Vector3(cameraToPlayer.x, 0, cameraToPlayer.z) * Input.GetAxis("Vertical");
        appliedForce += new Vector3(cameraToPlayer.z, 0, -cameraToPlayer.x) * Input.GetAxis("Horizontal");
        appliedForce = appliedForce * 0.5f;
        physics.AddForce(appliedForce,ForceMode.VelocityChange);
	}
}
