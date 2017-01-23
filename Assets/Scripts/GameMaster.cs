using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

        levelManager = LevelManager.getLevelManager();

        LoadGameStats();

        StartLevel();
    }

    private void StartLevel()
    {   
        levelManager.GenerateMap();

        //Set Player on Start Isle
        playerObject.GetComponent<CharacterController>().enabled = false;

        Isle startIsle = levelManager.startIsle.IsleObj;
        levelManager.currentIsle = startIsle.isleAbstract;
        playerObject.transform.position = new Vector3(startIsle.transform.position.x, startIsle.transform.position.y + 2, startIsle.transform.position.z);
        playerObject.GetComponent<NavMeshTarget>().IslePosition = startIsle.transform.position;

        levelManager.currentIsle.IsleObj.StartIsle();
        levelManager.currentIsle.IsleObj.AddBorders();

        // activate Character Controller
        playerObject.GetComponent<CharacterController>().enabled = true;

        // show UI (inclusive Mini-Map)
        UI_Canvas ui = mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.UI).GetComponent<UI_Canvas>();
        ui.ShowMiniMap();
        Stats stats = playerObject.GetComponent<Stats>();
        ui.UpdateLive(stats.health, stats.maxHealth);
        ui.UpdateKeys(playerObject.GetComponent<Player>().NumberSmallKeys);
    }

    private void LoadGameStats()
    {
        if (GameStats.LoadLevelSettings == true)
        {
            // load Level Settings

            int currentLevel = GameStats.Level;

            GameStats.UpdateLevelSettings(currentLevel);

            levelManager.WorldWidth = GameStats.LvlWorldWidht;
            levelManager.WorldHeight = GameStats.LvlWorldHeight;
            levelManager.IsleDensity = GameStats.LvlIsleDensity;

        }

        if (GameStats.LoadCharStats == true)
        {
            // load Player Stats

            Stats stats = playerObject.GetComponent<Stats>();

            GameStats.ReadCharStats(stats);
            
        }

    }

    public void BackToMenue()
    {
        StartCoroutine(BackToMenueHandler());
    }

    IEnumerator BackToMenueHandler()
    {

        yield return new WaitForSeconds(3);

        StopAllCoroutines();

        SceneManager.LoadScene("Scenes/Main_Menue");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // update Level Settings
            int level = GameStats.Level + 1;
            GameStats.UpdateLevelSettings(level);

            // save char stats

            GameStats.LoadCharStats = true;

            ObjectPool mr = ObjectPool.getObjectPool();
            GameObject player = mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.player);
            Stats stats = player.GetComponent<Stats>();

            GameStats.SaveCharSets(stats);

            // load Scene
            SceneManager.LoadScene("Scenes/World");
        }
    }
}
