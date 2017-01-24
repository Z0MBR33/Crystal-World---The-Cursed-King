using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PortalIsle : MonoBehaviour {

    public GameObject BossPortal;
    public GameObject Podest1;
    public GameObject Podest2;
    public GameObject Podest3;
    public GameObject portalParticle;

    [HideInInspector]
    public int PortalKeys = 0;
    [HideInInspector]
    public bool open;

    public void KeyArrived()
    {
        if (PortalKeys >= 3)
        {
            portalParticle.SetActive(true);
            open = true;
        }
    }

    public void teleport()
    {
        int level = GameStats.Level;

        if (level == 3)
        {
            // load End screen
            SceneManager.LoadScene("Scenes/End");
        }


        // update Level Settings
        level += 1;
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
