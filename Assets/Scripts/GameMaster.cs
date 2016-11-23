using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {
    private ObjectPool mr; 
    private GameObject slime;
    private GameObject island;

    //DOTween

    void Start () {
        mr = ObjectPool.getObjectPool();
        mr.getPlayer().transform.position = new Vector3(2, 2, 0);
        mr.getPlayer().SetActive(true);
        mr.getCamera().SetActive(true);
        
        /*
        slime = mr.getEnemie(0);
        slime.GetComponent<enemyMovement>().setTarget(mr.getPlayer().transform);
        slime.transform.position = new Vector3(0,0,0);
        slime.SetActive(true);
        */
        island = mr.getStructure(0);
        island.transform.position = new Vector3(0, 0, 0);
        island.SetActive(true);
        //Application.targetFrameRate = 20000000;
    }
	
	void Update () {

    }
}
