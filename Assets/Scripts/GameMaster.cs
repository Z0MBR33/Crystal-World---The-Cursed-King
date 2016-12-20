using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {
    private ObjectPool mr;
    private GameObject playerObject;
    private GameObject camObject;
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
        playerObject = mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.player);
        camObject = mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.camera);

        playerObject.SetActive(true);
        camObject.SetActive(true);

        ListEnemies = new List<GameObject>();

        // Create world
        levelManager = LevelManager.getLevelManager();
        levelManager.GenerateMap();


        //Set Player on Start Isle
        Isle startIsle = levelManager.getStartIsle().IsleObj;
        //Isle startIsle = levelManager.getWorld()[0, 0].IsleObj;
        levelManager.setCurrentIsle(startIsle.isleAbstract);
        playerObject.transform.position = new Vector3(startIsle.transform.position.x, startIsle.transform.position.y + 2, startIsle.transform.position.z);
        playerObject.GetComponent<NavMeshTarget>().IslePosition = startIsle.transform.position;

        StartCurrentIsle();

        // show UI (inclusive Mini-Map)
        UI_Canvas ui = mr.getObject(ObjectPool.categorie.essential,(int)ObjectPool.essential.UI).GetComponent<UI_Canvas>();
        ui.ShowMiniMap();
        ui.UpdateLive(playerObject.GetComponent<Stats>().health);
    }

    public void StartCurrentIsle()
    {

        Isle currentIsle = levelManager.getCurrentIsle().IsleObj;
        
        

        for (int i = 0; i < currentIsle.EnemyPoints.Count; i++)
        {
            currentIsle.EnemyPoints[i].IslePosition = currentIsle.transform.position;

            slime = mr.getObject(ObjectPool.categorie.enemy,(int)ObjectPool.enemy.slime);
            slime.GetComponent<Enemy>().Initialize();
            slime.transform.position = currentIsle.EnemyPoints[i].transform.position;
            slime.GetComponent<GhostCopy>().IslePosition = currentIsle.transform.position;

            slimeGhost = mr.getObject(ObjectPool.categorie.enemy, (int)ObjectPool.enemy.ghost);
            slimeGhost.transform.position = currentIsle.NavMeshPosition + currentIsle.EnemyPoints[i].getPositionOnIsle();
            slimeGhost.GetComponent<GhostMovement>().NavMashPosition = currentIsle.NavMeshPosition;
            slimeGhost.GetComponent<GhostMovement>().setTarget(playerObject.GetComponent<NavMeshTarget>());
            slimeGhost.GetComponent<GhostMovement>().setghostCopy(slime.GetComponent<GhostCopy>());
            slimeGhost.GetComponent<NavMeshAgent>().enabled = true;
            slime.GetComponent<GhostCopy>().ghost = slimeGhost.GetComponent<GhostMovement>();

            currentIsle.EnemyPoints[i].gameObject.SetActive(false);

            ListEnemies.Add(slime);

      
        }

        playerObject.GetComponent<NavMeshTarget>().IslePosition = currentIsle.transform.position;

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

                for(int i = 0; i < 6; i++)
                {
                    if (currentIsle.isleAbstract.Portals[i] != null)
                    {
                        currentIsle.Portals[i].PortalActivated = true;
                        currentIsle.Portals[i].portalSpiral.gameObject.SetActive(true);
                    }
                }

                currentIsle.isleAbstract.setFinishState(true);

                StopAllCoroutines();

                yield return null;
            }
        }
    }

}
