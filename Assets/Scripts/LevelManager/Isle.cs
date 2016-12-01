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

        // disable Portal-Tempaltes
        PortalUp.gameObject.SetActive(false);
        PortalUpRight.gameObject.SetActive(false);
        PortalDownRight.gameObject.SetActive(false);
        PortalDown.gameObject.SetActive(false);
        PortalDownLeft.gameObject.SetActive(false);
        PortalUpLeft.gameObject.SetActive(false);

        Portal realPortal;

        if (isle.ConnectionUp != null)
        {
            realPortal = Instantiate(mr.PortalPrefab).GetComponent<Portal>();
            realPortal.transform.position = PortalUp.transform.position;
            realPortal.transform.rotation = PortalUp.transform.rotation;
            realPortal.portalSpiral.gameObject.SetActive(false);
            realPortal.setDirection(0);
            PortalUp = realPortal;
        }
        if (isle.ConnectionUpRight != null)
        {
            realPortal = Instantiate(mr.PortalPrefab).GetComponent<Portal>();
            realPortal.transform.position = PortalUpRight.transform.position;
            realPortal.transform.rotation = PortalUpRight.transform.rotation;
            realPortal.portalSpiral.gameObject.SetActive(false);
            realPortal.setDirection(1);
            PortalUpRight = realPortal;
        }
        if (isle.ConnectionDownRight != null)
        {
            realPortal = Instantiate(mr.PortalPrefab).GetComponent<Portal>();
            realPortal.transform.position = PortalDownRight.transform.position;
            realPortal.transform.rotation = PortalDownRight.transform.rotation;
            realPortal.portalSpiral.gameObject.SetActive(false);
            realPortal.setDirection(2);
            PortalDownRight = realPortal;
        }
        if (isle.ConnectionDown != null)
        {
            realPortal = Instantiate(mr.PortalPrefab).GetComponent<Portal>();
            realPortal.transform.position = PortalDown.transform.position;
            realPortal.transform.rotation = PortalDown.transform.rotation;
            realPortal.portalSpiral.gameObject.SetActive(false);
            realPortal.setDirection(3);
            PortalDown = realPortal;
        }
        if (isle.ConnectionDownLeft != null)
        {
            realPortal = Instantiate(mr.PortalPrefab).GetComponent<Portal>();
            realPortal.transform.position = PortalDownLeft.transform.position;
            realPortal.transform.rotation = PortalDownLeft.transform.rotation;
            realPortal.portalSpiral.gameObject.SetActive(false);
            realPortal.setDirection(4);
            PortalDownLeft = realPortal;
        }
        if (isle.ConnectionUpLeft != null)
        {
            realPortal = Instantiate(mr.PortalPrefab).GetComponent<Portal>();
            realPortal.transform.position = PortalUpLeft.transform.position;
            realPortal.transform.rotation = PortalUpLeft.transform.rotation;
            realPortal.portalSpiral.gameObject.SetActive(false);
            realPortal.setDirection(5);
            PortalUpLeft = realPortal;
        }

    }
}
