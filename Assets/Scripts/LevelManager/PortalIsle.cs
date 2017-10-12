using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PortalIsle : MonoBehaviour {

    public GameObject BossPortalCollider;
    public GameObject Podest1;
    public GameObject Podest2;
    public GameObject Podest3;
    public GameObject portalParticle;
    //public string theScene;
    

    [HideInInspector]
    public int PortalKeys = 0;
    [HideInInspector]
    public bool open;

    public void KeyArrived()
    {
        if (PortalKeys >= 3)
        {
            portalParticle.SetActive(true);
            BossPortalCollider.SetActive(true);
            open = true;
        }
    }

    public void teleport()
    {
        int level = GameStats.Level;

        if (level == 4)
        {
            // load End screen
            SceneManager.LoadScene("Scenes/End");
            return;
        }


        // update Level Settings
        level += 1;
        GameStats.UpdateLevelSettings(level);

        // save char stats

        GameStats.LoadCharStats = true;

        ObjectPool mr = ObjectPool.getObjectPool();
        GameObject player = mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.player);

        GameStats.SaveCharSets(player);

        // load Scene
        //theScene = Application.loadedLevelName;
       // Debug.Log(theScene);
        if (level == 2)
        {
             SceneManager.LoadScene("Scenes/SnowIsles");
            return;
        }
        if (level == 3)
        {
            SceneManager.LoadScene("Scenes/LavaIsles");
            return;
        }


    }
}
