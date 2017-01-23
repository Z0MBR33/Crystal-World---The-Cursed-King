using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PortalIsle : MonoBehaviour {

    public GameObject BossPortal;
    public GameObject Podest1;
    public GameObject Podest2;
    public GameObject Podest3;

    [HideInInspector]
    public int PortalKeys = 0;
    [HideInInspector]
    public bool open;

    public void KeyArrived()
    {
        if (PortalKeys >= 3)
        {
            BossPortal.GetComponent<Renderer>().material.color = Color.yellow;
            open = true;
        }
    }

    public void teleport()
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
