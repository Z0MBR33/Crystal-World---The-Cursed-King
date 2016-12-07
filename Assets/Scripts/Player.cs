using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private ObjectPool mr;
    private Coroutine currentImmortalHandler;
    private bool isImmportal;

    private MeshRenderer meshRenderer;

    // Use this for initialization
    void Start()
    {
        mr = ObjectPool.getObjectPool();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isImmportal)
        {
            // flicker player
            meshRenderer.enabled = !meshRenderer.enabled;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isImmportal == false)
        {
            if (hit.gameObject.tag == "Enemy")
            {
                // TODO :  push player back

                becomeImmortal(3);
                Stats stats = GetComponent<Stats>();
                stats.gotHit(hit.gameObject.GetComponent<Stats>().strength);
                mr.getUI().UpdateLive(stats.health);
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

                // teleport player to isle;
                CharacterController cr = gameObject.GetComponent<CharacterController>();
                cr.velocity.Set(0, 0, 0);

                LevelManager lvlManager = LevelManager.getLevelManager();
                IsleAbstract currentIsle = lvlManager.getCurrentIsle();

                IsleAbstract targetIsle = null;
                Portal targetPortal = null;

                int direction = portal.getDirection();

                switch (direction)
                {
                    case 0:
                        targetIsle = currentIsle.getIsleUp();
                        targetPortal = targetIsle.IsleObj.PortalDown;
                        break;
                    case 1:
                        targetIsle = currentIsle.getIsleUpRight();
                        targetPortal = targetIsle.IsleObj.PortalDownLeft;
                        break;
                    case 2:
                        targetIsle = currentIsle.getIsleDownRight();
                        targetPortal = targetIsle.IsleObj.PortalUpLeft;
                        break;
                    case 3:
                        targetIsle = currentIsle.getIsleDown(); ;
                        targetPortal = targetIsle.IsleObj.PortalUp;
                        break;
                    case 4:
                        targetIsle = currentIsle.getIsleDownLeft();
                        targetPortal = targetIsle.IsleObj.PortalUpRight;
                        break;
                    case 5:
                        targetIsle = currentIsle.getIsleUpLeft();
                        targetPortal = targetIsle.IsleObj.PortalDownRight;
                        break;
                }

                Vector3 vec = targetIsle.IsleObj.transform.position - targetPortal.transform.position;
                vec.Normalize();
                vec = vec * 2;
                Vector3 startPos = new Vector3(targetPortal.transform.position.x + vec.x, targetPortal.transform.position.y + 1, targetPortal.transform.position.z + vec.z);

                transform.position = startPos;

                lvlManager.setCurrentIsle(targetIsle);

                GameMaster gm = GameMaster.getGameMaster();

                if (targetIsle.getFinishState() == false)
                {
                    gm.StartCurrentIsle();
                }

                mr.getUI().UpdateMiniMap();
            }

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

    IEnumerator ImmportalHandler(float time)
    {
        yield return new WaitForSeconds(time);

        meshRenderer.enabled = true;
        isImmportal = false;
    }
}
