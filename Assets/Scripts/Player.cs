using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private ObjectPool mr;
    private LevelManager lvlManager;
    private Coroutine currentImmortalHandler;
    private bool isImmportal;
    public GameObject Model;
    [HideInInspector]
    public bool DieOnCollision;
    [HideInInspector]
    public SkinnedMeshRenderer MeshRenderer;

    [HideInInspector]
    public int NumberSmallKeys = 0;
    [HideInInspector]
    public bool HasSplitter;
    [HideInInspector]
    public bool hasBluffer;

    [HideInInspector]
    public bool InControllable = true;

    // Use this for initialization
    void Start()
    {
        mr = ObjectPool.getObjectPool();
        lvlManager = LevelManager.getLevelManager();
        MeshRenderer = Model.GetComponent<SkinnedMeshRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (isImmportal)
        {
            // flicker player
            MeshRenderer.enabled = !MeshRenderer.enabled;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (DieOnCollision)
        {
            Stats stats = GetComponent<Stats>();
            stats.gotHit(100000);
        }

        if (isImmportal == false)
        {
            if (isImmportal == false)
            {
                if (hit.gameObject.tag == "Enemy")
                {
                    float damage = hit.gameObject.GetComponent<Stats>().strength;
                    TakeDamage(damage);
                }
            }
        }

        if (hit.gameObject.tag == "DeathPlane")
        {
            Stats stats = GetComponent<Stats>();
            stats.gotHit(hit.gameObject.GetComponent<Stats>().strength);
        }


        if (hit.gameObject.tag == "Portal")
        {
            Portal portal = hit.gameObject.GetComponent<Portal>();

            if (portal.PortalActivated == true)
            {
                portal.Teleport();
            }

        }

        if (hit.gameObject.tag == "Item")
        {
            Item item = hit.gameObject.GetComponent<Item>();

            if (item.collected == false)
            {
                item.Collect();
            }

        }

        if (hit.gameObject.tag == "Boss-Portal")
        {
            PortalIsle portalIsle = lvlManager.bossIsle.IsleObj.GetComponent<PortalIsle>();

            if (portalIsle.open == true)
            {
                portalIsle.teleport();
            }
        }
    }

    public void TakeDamage(float damage)
    {

        if (isImmportal == false)
        {
            becomeImmortal(3);
            Stats stats = GetComponent<Stats>();
            stats.gotHit(damage);
            mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.UI).GetComponent<UI_Canvas>().UpdateLive(stats.health, stats.maxHealth);
        }
    }

    public void becomeImmortal(float time)
    {
        isImmportal = true;

        if (currentImmortalHandler != null)
        {
            StopCoroutine(currentImmortalHandler);
        }

        currentImmortalHandler = StartCoroutine(ImmportalHandler(time));

    }

    public void Die()
    {
        InControllable = false;

        GameObject expl = ObjectPool.getObjectPool().getObject(ObjectPool.categorie.explosion, (int)ObjectPool.explosion.player);
        expl.GetComponent<ExplosionScript>().Initialize(transform.position);

        //ObjectPool mr = ObjectPool.getObjectPool();
        //mr.returnObject(gameObject); // no effect?
        gameObject.SetActive(false);

        GameMaster.getGameMaster().BackToMenue();
    }

    IEnumerator ImmportalHandler(float time)
    {
        yield return new WaitForSeconds(time);

        MeshRenderer.enabled = true;
        isImmportal = false;
    }

}
