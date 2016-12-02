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

    private ObjectPool mr;

    public void Initialize(IsleAbstract isle)
    {
        isleAbstract = isle;

        mr = ObjectPool.getObjectPool();

        // hide Enemy Spawns
        for(int i = 0; i < EnemyPoints.Count; i++)
        {
            EnemyPoints[i].GetComponent<Renderer>().enabled = false;
        }

        // hide Portal-Tempaltes
        PortalUp.gameObject.SetActive(false);
        PortalUpRight.gameObject.SetActive(false);
        PortalDownRight.gameObject.SetActive(false);
        PortalDown.gameObject.SetActive(false);
        PortalDownLeft.gameObject.SetActive(false);
        PortalUpLeft.gameObject.SetActive(false);


        // show real Portals
        Portal realPortal;

        if (isle.PortalUp != null)
        {
            realPortal = Instantiate(mr.PortalPrefab).GetComponent<Portal>();
            realPortal.transform.position = PortalUp.transform.position;
            realPortal.transform.rotation = PortalUp.transform.rotation;
            realPortal.portalSpiral.gameObject.SetActive(false);
            realPortal.setDirection(0);
            isleAbstract.PortalUp.portalObj = realPortal;
            realPortal.portalAbstract = isleAbstract.PortalUp;
            PortalUp = realPortal;
            realPortal.transform.SetParent(gameObject.transform);
        }
        if (isle.PortalUpRight != null)
        {
            realPortal = Instantiate(mr.PortalPrefab).GetComponent<Portal>();
            realPortal.transform.position = PortalUpRight.transform.position;
            realPortal.transform.rotation = PortalUpRight.transform.rotation;
            realPortal.portalSpiral.gameObject.SetActive(false);
            realPortal.setDirection(1);
            isleAbstract.PortalUpRight.portalObj = realPortal;
            realPortal.portalAbstract = isleAbstract.PortalUpRight;
            PortalUpRight = realPortal;
            realPortal.transform.SetParent(gameObject.transform);
        }
        if (isle.PortalDownRight != null)
        {
            realPortal = Instantiate(mr.PortalPrefab).GetComponent<Portal>();
            realPortal.transform.position = PortalDownRight.transform.position;
            realPortal.transform.rotation = PortalDownRight.transform.rotation;
            realPortal.portalSpiral.gameObject.SetActive(false);
            realPortal.setDirection(2);
            isleAbstract.PortalDownRight.portalObj = realPortal;
            realPortal.portalAbstract = isleAbstract.PortalDownRight;
            PortalDownRight = realPortal;
            realPortal.transform.SetParent(gameObject.transform);
        }
        if (isle.PortalDown != null)
        {
            realPortal = Instantiate(mr.PortalPrefab).GetComponent<Portal>();
            realPortal.transform.position = PortalDown.transform.position;
            realPortal.transform.rotation = PortalDown.transform.rotation;
            realPortal.portalSpiral.gameObject.SetActive(false);
            realPortal.setDirection(3);
            isleAbstract.PortalDown.portalObj = realPortal;
            realPortal.portalAbstract = isleAbstract.PortalDown;
            PortalDown = realPortal;
            realPortal.transform.SetParent(gameObject.transform);
        }
        if (isle.PortalDownLeft != null)
        {
            realPortal = Instantiate(mr.PortalPrefab).GetComponent<Portal>();
            realPortal.transform.position = PortalDownLeft.transform.position;
            realPortal.transform.rotation = PortalDownLeft.transform.rotation;
            realPortal.portalSpiral.gameObject.SetActive(false);
            realPortal.setDirection(4);
            isleAbstract.PortalDownLeft.portalObj = realPortal;
            realPortal.portalAbstract = isleAbstract.PortalDownLeft;
            PortalDownLeft = realPortal;
            realPortal.transform.SetParent(gameObject.transform);
        }
        if (isle.PortalUpLeft != null)
        {
            realPortal = Instantiate(mr.PortalPrefab).GetComponent<Portal>();
            realPortal.transform.position = PortalUpLeft.transform.position;
            realPortal.transform.rotation = PortalUpLeft.transform.rotation;
            realPortal.portalSpiral.gameObject.SetActive(false);
            realPortal.setDirection(5);
            isleAbstract.PortalUpLeft.portalObj = realPortal;
            realPortal.portalAbstract = isleAbstract.PortalUpLeft;
            PortalUpLeft = realPortal;
            realPortal.transform.SetParent(gameObject.transform);
        }

    }
}
