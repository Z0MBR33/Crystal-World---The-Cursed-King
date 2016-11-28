using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    public Classes.ShotMode ModeOfShots;

    public Vector3 Direction;
    public float Speed;

    private ObjectPool mr;

    // Use this for initialization
    void Start()
    {
        mr = ObjectPool.getObjectPool();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            Shot shot = mr.getShot(0).GetComponent<Shot>();
            shot.gameObject.SetActive(true);

            if (ModeOfShots == Classes.ShotMode.Rocket)
            {
                shot.Initialize(ModeOfShots, transform.position + new Vector3(0, 1, 0), Direction, transform.rotation, 50, 1);

            }
            else if (ModeOfShots == Classes.ShotMode.Bomb)
            {
                shot.Initialize(ModeOfShots, transform.position + new Vector3(0, 1, 0), Direction, transform.rotation, 0, 1);
                Rigidbody rb = shot.gameObject.GetComponent<Rigidbody>();

                rb.AddForce(Direction.normalized * Speed, ForceMode.Impulse);

            }

        }

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Portal")
        {
            Portal portal = hit.gameObject.GetComponent<Portal>();
            
            if (portal.PortalActivated == true)
            { 
        
                // teleport player to isle;
                CharacterController cr = GetComponent<CharacterController>();
                cr.velocity.Set(0,0,0);

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
                vec = vec * 5;
                Vector3 startPos = new Vector3(targetPortal.transform.position.x + vec.x, targetPortal.transform.position.y, targetPortal.transform.position.z + vec.z);

                transform.position = startPos;

                lvlManager.setCurrentIsle(targetIsle);

                mr.getUI().UpdateMiniMap();
            }

        }
    }

}