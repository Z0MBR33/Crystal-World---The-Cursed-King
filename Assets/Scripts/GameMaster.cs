using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {
    private ObjectPool mr;
    private LevelManager levelManager;
    private GameObject slime;
    private GameObject slimeGhost;
    //private GameObject island;

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
        Isle startIsle = levelManager.getRandomIsle().IsleObj;
        //Isle startIsle = levelManager.getWorld()[0, 0].IsleObj;
        levelManager.setCurrentIsle(startIsle.isleAbstract);
        mr.getPlayer().transform.position = new Vector3(startIsle.transform.position.x, startIsle.transform.position.y + 2, startIsle.transform.position.z);
        mr.getPlayer().GetComponent<NavMeshTarget>().IslePosition = startIsle.transform.position;

        StartCurrentIsle();

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

            slimeGhost = Instantiate(mr.GhostPrefab, currentIsle.NavMeshPosition + currentIsle.EnemyPoints[i].getPositionOnIsle(), Quaternion.Euler(0, 0, 0)) as GameObject;
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

                if (currentIsle.isleAbstract.PortalUp != null)
                {
                    currentIsle.PortalUp.PortalActivated = true;
                    currentIsle.PortalUp.portalSpiral.gameObject.SetActive(true);
                }

                if (currentIsle.isleAbstract.PortalUpRight != null)
                {
                    currentIsle.PortalUpRight.PortalActivated = true;
                    currentIsle.PortalUpRight.portalSpiral.gameObject.SetActive(true);
                }

                if (currentIsle.isleAbstract.PortalDownRight != null)
                {
                    currentIsle.PortalDownRight.PortalActivated = true;
                    currentIsle.PortalDownRight.portalSpiral.gameObject.SetActive(true);
                }

                if (currentIsle.isleAbstract.PortalDown != null)
                {
                    currentIsle.PortalDown.PortalActivated = true;
                    currentIsle.PortalDown.portalSpiral.gameObject.SetActive(true);
                }

                if (currentIsle.isleAbstract.PortalDownLeft != null)
                {
                    currentIsle.PortalDownLeft.PortalActivated = true;
                    currentIsle.PortalDownLeft.portalSpiral.gameObject.SetActive(true);
                }

                if (currentIsle.isleAbstract.PortalUpLeft != null)
                {
                    currentIsle.PortalUpLeft.PortalActivated = true;
                    currentIsle.PortalUpLeft.portalSpiral.gameObject.SetActive(true);
                }

                currentIsle.isleAbstract.setFinishState(true);

                StopAllCoroutines();

                yield return null;
            }
        }
    }

}
