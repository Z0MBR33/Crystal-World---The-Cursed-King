using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {
    private ObjectPool mr;
    private LevelManager levelManager;
    private GameObject slime;
    private GameObject slimeGhost;
    //private GameObject island;

    [Header("TO-Do: Ghost klären!")]
    public GameObject GhostPrefab;

    public List<GameObject> ListEnemies;

    //DOTween

    void Start () {
        mr = ObjectPool.getObjectPool();
        mr.getPlayer().SetActive(true);
        mr.getCamera().SetActive(true);

        ListEnemies = new List<GameObject>();

        // Create world
        levelManager = LevelManager.getLevelManager();
        levelManager.GenerateMap();

  
        // TODO  create spawn-Points!
        //Isle startIsle = levelManager.getRandomIsle().IsleObj;
        Isle startIsle = levelManager.getWorld()[0, 0].IsleObj;
        levelManager.setCurrentIsle(startIsle.isleAbstract);
        mr.getPlayer().transform.position = new Vector3(startIsle.transform.position.x, startIsle.transform.position.y + 2, startIsle.transform.position.z);
        mr.getPlayer().GetComponent<NavMeshTarget>().IslePosition = startIsle.transform.position;

        // TODO

        StartCurrentIsle();

        slime = mr.getEnemie(0);
        slime.transform.position = new Vector3(0, 0, 0);
        slime.SetActive(true);
        slime.GetComponent<GhostCopy>().IslePosition = startIsle.transform.position;
        slimeGhost = Instantiate(GhostPrefab, new Vector3(-100, 2 , 0), Quaternion.Euler(0,0,0)) as GameObject;

        slimeGhost.GetComponent<GhostMovement>().NavMashPosition = startIsle.NavMeshPosition;
        slimeGhost.GetComponent<GhostMovement>().setTarget(mr.getPlayer().GetComponent<NavMeshTarget>());
        slimeGhost.GetComponent<GhostMovement>().setghostCopy(slime.GetComponent<GhostCopy>());
        slime.GetComponent<GhostCopy>().ghost = slimeGhost.GetComponent<GhostMovement>();


        /*
        island = mr.getStructure(0);
        island.transform.position = new Vector3(0, 0, 0);
        island.SetActive(true);
        //Application.targetFrameRate = 20000000;
        */

        // show UI (inclusive Mini-Map)
        UI_Canvas ui = mr.getUI().GetComponent<UI_Canvas>();
        ui.ShowMiniMap();
    }

    public void StartCurrentIsle()
    {

    }

}
