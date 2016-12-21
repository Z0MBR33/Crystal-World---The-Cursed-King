using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {
    private ObjectPool mr;
    private GameObject playerObject;
    private GameObject camObject;
    private LevelManager levelManager;
   
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

        // Create world
        levelManager = LevelManager.getLevelManager();
        levelManager.GenerateMap();


        //Set Player on Start Isle
        Isle startIsle = levelManager.getStartIsle().IsleObj;
        //Isle startIsle = levelManager.getWorld()[0, 0].IsleObj;
        levelManager.setCurrentIsle(startIsle.isleAbstract);
        playerObject.transform.position = new Vector3(startIsle.transform.position.x, startIsle.transform.position.y + 2, startIsle.transform.position.z);
        playerObject.GetComponent<NavMeshTarget>().IslePosition = startIsle.transform.position;

        levelManager.getCurrentIsle().IsleObj.StartIsle();

        // show UI (inclusive Mini-Map)
        UI_Canvas ui = mr.getObject(ObjectPool.categorie.essential,(int)ObjectPool.essential.UI).GetComponent<UI_Canvas>();
        ui.ShowMiniMap();
        ui.UpdateLive(playerObject.GetComponent<Stats>().health);
    }

}
