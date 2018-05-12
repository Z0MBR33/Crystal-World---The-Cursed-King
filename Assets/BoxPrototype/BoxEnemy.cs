using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoxEnemy : MonoBehaviour {

    public Transform target;
    private  NavMeshAgent agent;

    public int Life = 3;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        agent.SetDestination(target.position);
	}

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
