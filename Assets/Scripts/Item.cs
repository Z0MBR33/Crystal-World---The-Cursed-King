using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour {

    public enum ItemType { PortalKey1, PortalKey2, PortalKey3, BigBox, SmallBox }
    public ItemType Type;

    public enum ContentType { SmallKey, StatUpgrade, Splitter, Tornado }; // TODO

    [HideInInspector]
    public bool collected;
    public bool opened;

    private ObjectPool mr;
    private LevelManager lvlManager;

    private Coroutine checkTeleportFinished;

    private List<lerpInfo> lerpList;

    public void Start()
    {
        mr = ObjectPool.getObjectPool();
        lvlManager = LevelManager.getLevelManager();
        lerpList = new List<lerpInfo>();
    }

    public void initialize()
    {
        collected = false;
        opened = false;

        if (Type == ItemType.SmallBox || Type == ItemType.BigBox)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(0.296f, 0.141f, 0.057f, 1);
        }

        transform.position = Vector3.zero;
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
                // TODO
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

