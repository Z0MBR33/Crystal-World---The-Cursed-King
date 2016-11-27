using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {
    private ObjectPool mr;
    private LevelManager levelManager;
    // private GameObject slime;
    //private GameObject island;

    //DOTween

    void Start () {
        mr = ObjectPool.getObjectPool();
        mr.getPlayer().SetActive(true);
        mr.getCamera().SetActive(true);

        levelManager = LevelManager.getLevelManager();
        levelManager.GenerateMap();

        /*
        slime = mr.getEnemie(0);
        slime.GetComponent<enemyMovement>().setTarget(mr.getPlayer().transform);
        slime.transform.position = new Vector3(0,0,0);
        slime.SetActive(true);
        */


        // TODO  create spawn-Points!
        Isle startIsle = levelManager.getRandomIsle().IsleObj;
        levelManager.setCurrentIsle(startIsle.isleAbstract);
        mr.getPlayer().transform.position = new Vector3(startIsle.transform.position.x, startIsle.transform.position.y + 10, startIsle.transform.position.z);
        /*
        island = mr.getStructure(0);
        island.transform.position = new Vector3(0, 0, 0);
        island.SetActive(true);
        //Application.targetFrameRate = 20000000;
        */
    }
    
}
