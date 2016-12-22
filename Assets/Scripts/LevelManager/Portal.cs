using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{

    public bool PortalActivated = false;
    public PortalSpiral portalSpiral;
    public GameObject spawnPoint;

    public PortalAbstract portalAbstract;

    private int Direction;

    private ObjectPool mr;

    private Coroutine portalTimeOutRoutine;

    private void Awake()
    {
        mr = ObjectPool.getObjectPool();
    }

    public void setDirection(int direction)
    {
        this.Direction = direction;
    }

    public int getDirection()
    {
        return this.Direction;
    }

    public void Teleport()
    {
        // teleport player to isle;

        GameObject player = mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.player);

        CharacterController cr = player.GetComponent<CharacterController>();
        cr.velocity.Set(0, 0, 0);

        LevelManager lvlManager = LevelManager.getLevelManager();
        IsleAbstract currentIsle = lvlManager.getCurrentIsle();

        int direction = getDirection();

        IsleAbstract targetIsle = currentIsle.getIsleFromForection(direction);
        Portal targetPortal = targetIsle.IsleObj.Portals[(direction + 3) % 6];

        player.transform.position = targetPortal.spawnPoint.transform.position + new Vector3(0, 1, 0);

        lvlManager.setCurrentIsle(targetIsle);

        if (targetIsle.getFinishState() == false)
        {
            targetIsle.IsleObj.StartIsle();

        } else {
            targetPortal.StartPortalTimeOut();
        }

        mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.UI).GetComponent<UI_Canvas>().UpdateMiniMap();
    }

    public void StartPortalTimeOut()
    {
        portalTimeOutRoutine = StartCoroutine(portalTimeOutHandler());
    }

    public IEnumerator portalTimeOutHandler()
    {
        PortalActivated = false;

        yield return new WaitForSeconds(2);

        PortalActivated = true;

        StopCoroutine(portalTimeOutRoutine);
    }
}