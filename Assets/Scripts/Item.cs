using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour {

    public enum ItemType { PortalKey1, PortalKey2, PortalKey3, BigBox, SmallBox }
    public ItemType Type;

    public enum ContentType { SmallKey, SpeedUpgrade, DamageUpgrade, RateUpgrade, ShotSpeedUpgrade };
    private ContentType Content;
    [HideInInspector]
    public GameObject ContentObj;


    [HideInInspector]
    public bool collected;
    [HideInInspector]
    public bool opened;

    private ObjectPool mr;
    private LevelManager lvlManager;

    private Coroutine checkTeleportFinished;

    private List<lerpInfo> lerpList;

    public Animator derObjectAnimator;


    public void initialize()
    {
        mr = ObjectPool.getObjectPool();
        lvlManager = LevelManager.getLevelManager();
        lerpList = new List<lerpInfo>();

        collected = false;
        opened = false;

        if (Type == ItemType.SmallBox || Type == ItemType.BigBox)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(0.296f, 0.141f, 0.057f, 1);
        }
        if (Type == ItemType.SmallBox)
        {
            generateSmallBoxContent();
            derObjectAnimator = gameObject.GetComponent<Animator>();

        }
        if (Type == ItemType.BigBox)
        {
            generateBigBoxContent();
        }

        transform.position = Vector3.zero;
    }

    private void generateSmallBoxContent()
    {

        if (ContentObj != null)
        {
            mr.returnObject(ContentObj);
            ContentObj = null;
        }

        int tmp = mr.random.Next(0, 5);

        Content = (ContentType)tmp;

        switch (Content)
        {
            case ContentType.SmallKey: ContentObj = mr.getObject(ObjectPool.categorie.items, (int)ObjectPool.items.smallKey);
                break;
            case ContentType.SpeedUpgrade: ContentObj = mr.getObject(ObjectPool.categorie.items, (int)ObjectPool.items.upgradeSpeed);
                break;
            case ContentType.DamageUpgrade: ContentObj = mr.getObject(ObjectPool.categorie.items, (int)ObjectPool.items.upgradeDamage);
                break;
            case ContentType.RateUpgrade: ContentObj = mr.getObject(ObjectPool.categorie.items, (int)ObjectPool.items.upgradeRate);
                break;
            case ContentType.ShotSpeedUpgrade: ContentObj = mr.getObject(ObjectPool.categorie.items, (int)ObjectPool.items.upgradeShotSpeed);
                break;
            default: print("Error: Kein Gültiger Inhalt für Trufe");
                break;
        }

        ContentObj.transform.position = transform.position;

    }

    private void generateBigBoxContent()
    {

    }        

    public void Collect(GameObject collecter)
    {
        if (Type == ItemType.PortalKey1 || Type ==  ItemType.PortalKey2 || Type == ItemType.PortalKey3)
        {
            teleport();

            lvlManager.currentIsle.isleObjectType = IsleAbstract.IsleObjectType.normal;
            mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.UI).GetComponent<UI_Canvas>().UpdateMiniMap();
            collected = true;
        }
        else if (Type == ItemType.SmallBox)
        {
            if (opened == true)
            {
                


                collected = true;
            }
        }
        else if (Type == ItemType.BigBox)
        {
            Player player = mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.player).GetComponent<Player>();

            if (player.NumberKeys > 0)
            {
                print("TODO GROSSE FETTE BOX!");
            }
        }

    }

    private void teleport()
    {
        lerpList.Clear();

        Vector3 podestPosition = Vector3.zero;

        PortalIsle portalIsle = lvlManager.bossIsle.IsleObj.GetComponent<PortalIsle>();

        switch (Type)
        {
            case ItemType.PortalKey1: podestPosition = portalIsle.Podest1.transform.position;
                break;
            case ItemType.PortalKey2: podestPosition = portalIsle.Podest2.transform.position;
                break;
            case ItemType.PortalKey3: podestPosition = portalIsle.Podest3.transform.position;
                break;
            default: print("Error: teleport von Items ohne gütigem Item-Type (nur PortalKey Erlaubt)");
                break;
        }
        lerpList.Add(new lerpInfo(gameObject.transform.position + new Vector3(0, 5, 0), 3));
        lerpList.Add(new lerpInfo(podestPosition + new Vector3(0, 5, 0), 25));
        lerpList.Add(new lerpInfo(podestPosition + new Vector3(0, 1, 0), 7));

        checkTeleportFinished = StartCoroutine(checkTeleportFinishedHandler());
    }

  
    public IEnumerator checkTeleportFinishedHandler()
    {
        while (lerpList.Count > 0)
        {
            Vector3 targetPos = lerpList[0].targetPos;
            float speed = lerpList[0].speed;

            gameObject.GetComponent<Lerper>().StartLerp(gameObject.transform.position, targetPos, speed);

            while (true)
            {
               
                if (gameObject.GetComponent<Lerper>().Lerping == false)
                {
                    break;
                }else{
                    yield return new WaitForSeconds(0.5f);
                }

            }

            lerpList.RemoveAt(0);
        }

        lvlManager.bossIsle.IsleObj.GetComponent<PortalIsle>().KeyArrived();

        StopCoroutine(checkTeleportFinished);

        yield return null;
    }
}

class lerpInfo
{
    public Vector3 targetPos;
    public float speed; 

    public lerpInfo(Vector3 targetPos, float speed)
    {
        this.targetPos = targetPos;
        this.speed = speed;
    }
}

