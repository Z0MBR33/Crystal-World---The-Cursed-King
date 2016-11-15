using UnityEngine;
using System.Collections;

public class spawn : MonoBehaviour {
    public GameObject prefab;
    private Transform spawnPosition;
    private int frameCounter;
    private GameObject spawned;
    public Transform playerTarget;
    private enemyMovement script;

	// Use this for initialization
	void Start () {
        spawnPosition = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        if (frameCounter++ == 100) {
            spawned = (GameObject)Instantiate(prefab, gameObject.transform,true);
            script = spawned.GetComponent<enemyMovement>();
            script.setTarget(playerTarget);
            frameCounter = 0;
        }

	}
}
