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
    private LevelManager lvlManager;
    private GameObject player;

    private Coroutine checkTeleportFinied;
    private Coroutine portalTimeOutRoutine;

    private Portal targetPortal;
    private IsleAbstract targetIsle;

    private void Start()
    {
        mr = ObjectPool.getObjectPool();
        lvlManager = LevelManager.getLevelManager();
        player = mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.player);
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

        CharacterController cr = player.GetComponent<CharacterController>();
        cr.velocity.Set(0, 0, 0);

        IsleAbstract currentIsle = lvlManager.getCurrentIsle();

        int direction = getDirection();

        targetIsle = currentIsle.getIsleFromForection(direction);
        targetPortal = targetIsle.IsleObj.Portals[(direction + 3) % 6];

        Vector3 startPos = transform.position + new Vector3(0, 1, 0);
        Vector3 targetPos = targetPortal.spawnPoint.transform.position + new Vector3(0, 1, 0);

        cr.enabled = false;

        player.GetComponent<Lerper>().StartLerp(startPos, targetPos, 50);

        checkTeleportFinied = StartCoroutine(checkTeleportFiniedHandler());

        //player.transform.position = targetPortal.spawnPoint.transform.position + new Vector3(0, 1, 0);

    }

    public IEnumerator checkTeleportFiniedHandler()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (player.GetComponent<Lerper>().Lerping == false)
            {
                player.GetComponent<CharacterController>().enabled = true;

                lvlManager.setCurrentIsle(targetIsle);

                if (targetIsle.getFinishState() == false)
                {
                    targetIsle.IsleObj.StartIsle();

                }
                else
                {
                    targetPortal.StartPortalTimeOut();
                }

                mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.UI).GetComponent<UI_Canvas>().UpdateMiniMap();

                StopCoroutine(checkTeleportFinied);

                print("Ende");

                yield return null;
            }

        }
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