using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {
    private ObjectPool mr;
    private LevelManager levelManager;
    private GameObject slime;
    private GameObject slimeGhost;
    //private GameObject island;

    [Header("TO-Do: In Object-Pool")]
    public GameObject GhostPrefab;

    public List<GameObject> ListEnemies;

    public static GameMaster gameMaster;

    public static GameMaster getGameMaster()
    {
        return gameMaster;
    }

    void Awake()
    {
        gameMaster = this;
    }


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

        StartCurrentIsle();

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

        Isle currentIsle = levelManager.getCurrentIsle().IsleObj;
        
        

        for (int i = 0; i < currentIsle.EnemyPoints.Count; i++)
        {
            currentIsle.EnemyPoints[i].IslePosition = currentIsle.transform.position;

            slime = mr.getEnemy(0);
            slime.GetComponent<Enemy>().Initialize();
            slime.transform.position = currentIsle.EnemyPoints[i].transform.position;
            slime.SetActive(true);
            slime.GetComponent<GhostCopy>().IslePosition = currentIsle.transform.position;

            slimeGhost = Instantiate(GhostPrefab, currentIsle.NavMeshPosition + currentIsle.EnemyPoints[i].getPositionOnIsle(), Quaternion.Euler(0, 0, 0)) as GameObject;
            slimeGhost.GetComponent<GhostMovement>().NavMashPosition = currentIsle.NavMeshPosition;
            slimeGhost.GetComponent<GhostMovement>().setTarget(mr.getPlayer().GetComponent<NavMeshTarget>());
            slimeGhost.GetComponent<GhostMovement>().setghostCopy(slime.GetComponent<GhostCopy>());
            slime.GetComponent<GhostCopy>().ghost = slimeGhost.GetComponent<GhostMovement>();

            currentIsle.EnemyPoints[i].gameObject.SetActive(false);

            ListEnemies.Add(slime);
        }

        mr.getPlayer().GetComponent<NavMeshTarget>().IslePosition = currentIsle.transform.position;

        StartCoroutine(timerHandler());
    }

    public IEnumerator timerHandler()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (ListEnemies.Count <= 0)
            {
                // Level finished

                // activate portals

                Isle currentIsle = levelManager.getCurrentIsle().IsleObj;

                currentIsle.PortalUp.PortalActivated = true;
                currentIsle.PortalUp.GetComponent<Renderer>().material.color = new Color(230, 230, 0);

                currentIsle.PortalUpRight.PortalActivated = true;
                currentIsle.PortalUpRight.GetComponent<Renderer>().material.color = new Color(230, 230, 0);

                currentIsle.PortalDownRight.PortalActivated = true;
                currentIsle.PortalDownRight.GetComponent<Renderer>().material.color = new Color(230, 230, 0);

                currentIsle.PortalDown.PortalActivated = true;
                currentIsle.PortalDown.GetComponent<Renderer>().material.color = new Color(230, 230, 0);

                currentIsle.PortalDownLeft.PortalActivated = true;
                currentIsle.PortalDownLeft.GetComponent<Renderer>().material.color = new Color(230, 230, 0);

                currentIsle.PortalUpLeft.PortalActivated = true;
                currentIsle.PortalUpLeft.GetComponent<Renderer>().material.color = new Color(230, 230, 0);

                currentIsle.isleAbstract.setFinishState(true);

                StopAllCoroutines();

                yield return null;
            }
        }
    }

}
