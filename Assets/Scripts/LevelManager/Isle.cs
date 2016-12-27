using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Isle : MonoBehaviour
{
    public IsleAbstract isleAbstract;

    public Vector3 NavMeshPosition;

    public Portal PortalUp;
    public Portal PortalUpRight;
    public Portal PortalDownRight;
    public Portal PortalDown;
    public Portal PortalDownLeft;
    public Portal PortalUpLeft;

    public List<ItemPoint> ItemPoints;
    public List<EnemyPoint> EnemyPoints;
    [HideInInspector]
    public int EnemyCount;

    [HideInInspector]
    public List<GameObject> ListEnemies;

    [HideInInspector]
    public Portal[] Portals;

    private ObjectPool mr;
    private GameObject playerObject;

    private Coroutine levelCheckRoutine;

    private System.Random rnd;

    public void Initialize(IsleAbstract isle)
    {
        isleAbstract = isle;

        mr = ObjectPool.getObjectPool();
        playerObject = mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.player);

        rnd = mr.random;

        ListEnemies = new List<GameObject>();

        Portals = new Portal[6];
        Portals[0] = PortalUp;
        Portals[1] = PortalUpRight;
        Portals[2] = PortalDownRight;
        Portals[3] = PortalDown;
        Portals[4] = PortalDownLeft;
        Portals[5] = PortalUpLeft;

        // hide Enemy Spawns
        for (int i = 0; i < EnemyPoints.Count; i++)
        {
            EnemyPoints[i].GetComponent<Renderer>().enabled = false;
        }

        // hide Item Spawns
        for (int i = 0; i < ItemPoints.Count; i++)
        {
            ItemPoints[i].GetComponent<Renderer>().enabled = false;
        }

        // Portal stuff
        for (int i = 0; i < 6; i++)
        {
            // hide Portal-Tempaltes
            Portals[i].gameObject.SetActive(false);

            // show real Portals (and remove old ones
            
            if (isleAbstract.Portals[i] != null)
            {
                Portal realPortal = mr.getObject(ObjectPool.categorie.structures, (int)ObjectPool.structures.portal).GetComponent<Portal>();
                realPortal.transform.position = Portals[i].transform.position;
                realPortal.transform.rotation = Portals[i].transform.rotation;
                realPortal.spawnPoint.transform.position = Portals[i].spawnPoint.transform.position;
                realPortal.spawnPoint.SetActive(false);
                realPortal.portalSpiral.gameObject.SetActive(false);
                realPortal.setDirection(i);
                isleAbstract.Portals[i].portalObj = realPortal;
                realPortal.portalAbstract = isleAbstract.Portals[i];
                Portals[i] = realPortal;
                realPortal.transform.SetParent(gameObject.transform);
            }
        }

        // set items ---------------------

        int keyPoint =-1;

        if (isleAbstract.isleObjectType == IsleAbstract.IsleObjectType.key)
        {
            keyPoint = rnd.Next(0, ItemPoints.Count);
        }

        for (int i = 0; i < ItemPoints.Count; i++)
        {
            // check for key
            if (i == keyPoint)
            {
                // place key
                GameObject key = null;
                switch (isleAbstract.keyNumber)
                {
                    case 1: key = mr.getObject(ObjectPool.categorie.items, (int)ObjectPool.items.key1);
                        break;
                    case 2: key = mr.getObject(ObjectPool.categorie.items, (int)ObjectPool.items.key2);
                        break;
                    case 3: key = mr.getObject(ObjectPool.categorie.items, (int)ObjectPool.items.key3);
                        break;
                    default: print("Error Key-Isle, but no valid key-Number. " + isleAbstract.keyNumber);
                        break;
                }

                key.transform.position = ItemPoints[i].transform.position;
            }
            else
            {
                // normal box
                GameObject box = null;
                if (rnd.Next(0, 101) < 75)
                {
                    box = mr.getObject(ObjectPool.categorie.items, (int)ObjectPool.items.smallBox);
                    print("s");
                }else{
                    box = mr.getObject(ObjectPool.categorie.items, (int)ObjectPool.items.bigBox);
                    print("b");
                }
                box.transform.position = ItemPoints[i].transform.position;
            }
        }
       
    }

    public void StartIsle()
    {
        // clear list
        for (int i = 0; i < ListEnemies.Count; i++)
        {
            mr.returnObject(ListEnemies[i].GetComponent<GhostCopy>().ghost.gameObject);
            mr.returnObject(ListEnemies[i]);
        }
        ListEnemies.Clear();
        EnemyCount = 0;

        // create enemies
        for (int i = 0; i < EnemyPoints.Count; i++)
        {
            EnemyPoints[i].IslePosition = transform.position;

            GameObject slime = mr.getObject(ObjectPool.categorie.enemy, (int)ObjectPool.enemy.slime);
            slime.GetComponent<Enemy>().Initialize();
            slime.transform.position = EnemyPoints[i].transform.position;
            slime.GetComponent<GhostCopy>().IslePosition = transform.position;

            GameObject slimeGhost = mr.getObject(ObjectPool.categorie.enemy, (int)ObjectPool.enemy.ghost);
            slimeGhost.transform.position = NavMeshPosition + EnemyPoints[i].getPositionOnIsle();
            slimeGhost.GetComponent<GhostMovement>().NavMashPosition = NavMeshPosition;
            slimeGhost.GetComponent<GhostMovement>().setTarget(playerObject.GetComponent<NavMeshTarget>());
            slimeGhost.GetComponent<GhostMovement>().setghostCopy(slime.GetComponent<GhostCopy>());
            slimeGhost.GetComponent<NavMeshAgent>().enabled = true;
            slime.GetComponent<GhostCopy>().ghost = slimeGhost.GetComponent<GhostMovement>();

            EnemyPoints[i].gameObject.SetActive(false);

            ListEnemies.Add(slime);
            EnemyCount++;

        }

        playerObject.GetComponent<NavMeshTarget>().IslePosition = transform.position;

        levelCheckRoutine = StartCoroutine(LevelCheckHandler());
    }

    public IEnumerator LevelCheckHandler()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (ListEnemies.Count <= 0 || EnemyCount <= 0)
            {
                // Level finished

                UnlockPortals();

                isleAbstract.setFinishState(true);

                StopCoroutine(levelCheckRoutine);

                yield return null;
            }
        }
    }

    public void LockPortals()
    {
        for (int i = 0; i < 6; i++)
        {
            if (isleAbstract.Portals[i] != null)
            {
                Portals[i].PortalActivated = false;
                Portals[i].portalSpiral.gameObject.SetActive(false);
            }
        }
    }

    public void UnlockPortals()
    {
        for (int i = 0; i < 6; i++)
        {
            if (isleAbstract.Portals[i] != null)
            {
                Portals[i].PortalActivated = true;
                Portals[i].portalSpiral.gameObject.SetActive(true);
            }
        }
    }

}
