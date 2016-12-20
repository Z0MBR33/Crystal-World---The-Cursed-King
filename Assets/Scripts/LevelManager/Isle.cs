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

    public List<EnemyPoint> EnemyPoints;

    [HideInInspector]
    public Portal[] Portals;

    private ObjectPool mr;

    public void Initialize(IsleAbstract isle)
    {
        isleAbstract = isle;

        mr = ObjectPool.getObjectPool();

        Portals = new Portal[6];
        Portals[0] = PortalUp;
        Portals[1] = PortalUpRight;
        Portals[2] = PortalDownRight;
        Portals[3] = PortalDown;
        Portals[4] = PortalDownLeft;
        Portals[5] = PortalUpLeft;

        // hide Enemy Spawns
        for(int i = 0; i < EnemyPoints.Count; i++)
        {
            EnemyPoints[i].GetComponent<Renderer>().enabled = false;
        }

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
                realPortal.portalSpiral.gameObject.SetActive(false);
                realPortal.setDirection(i);
                isleAbstract.Portals[i].portalObj = realPortal;
                realPortal.portalAbstract = isleAbstract.Portals[i];
                Portals[i] = realPortal;
                realPortal.transform.SetParent(gameObject.transform);
            }
        }
       
    }
}
