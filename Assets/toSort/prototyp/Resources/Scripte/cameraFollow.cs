using UnityEngine;
using System.Collections;
using System;

public class cameraFollow : MonoBehaviour {
    private ObjectPool mr;
    private Transform cameraPosition;
    private Transform playerPosition;
    private Vector3 offset;
    public float distance = 20.0f;
    private float cameraRotationDegree = 270;
    private float cameraHeight = 2;
    public float minCameraHeight = 0;
    public float maxCameraHeight = 5;
    // Use this for initialization
    void Start () {
        mr = ObjectPool.getObjectPool();
        playerPosition = mr.getPlayer().transform;
        cameraPosition = mr.getCamera().transform;
        offset = cameraPosition.position - playerPosition.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Horizontal_camera") != 0 || Input.GetAxis("Vertical_camera") != 0) {
            cameraHeight += Input.GetAxis("Vertical_camera");
            cameraRotationDegree += 2 * Input.GetAxis("Horizontal_camera");
            enforceCameraConstrains();
            offset = new Vector3((float)Math.Cos(cameraRotationDegree * Math.PI / 180), cameraHeight, (float)Math.Sin(cameraRotationDegree * Math.PI / 180));
        }
        offset = distance * offset.normalized;
        cameraPosition.position = playerPosition.position + offset;
        cameraPosition.LookAt(playerPosition.position);

    }
    void enforceCameraConstrains() {
        if (cameraHeight > maxCameraHeight) {
            cameraHeight = maxCameraHeight;
        }
        if (cameraHeight < minCameraHeight)
        {
            cameraHeight = minCameraHeight;
        }
        while (cameraRotationDegree > 360) {
            cameraRotationDegree -= 360;
        }
        while (cameraRotationDegree < 0)
        {
            cameraRotationDegree += 360;
        }

    }
}
