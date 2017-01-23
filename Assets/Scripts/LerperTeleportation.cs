using UnityEngine;
using System.Collections;

public class LerperTeleportation : MonoBehaviour
{

    [HideInInspector]
    public bool Lerping = false;

    private Vector3 StartPos;
    private Vector3 TargetPos;
    private float Speed;

    private float Distance;
    private float StartTime;

    private Controll conObject;
    private GameObject teleportationBall;
    private bool lookedATarget;

    public void StartLerp(Vector3 startPos, Vector3 targetPos, float speed)
    {
        StartPos = startPos;
        TargetPos = targetPos;
        Speed = speed;

        Distance = Vector3.Distance(startPos, targetPos);
        StartTime = Time.time;

        conObject = GameMaster.getGameMaster().GetComponent<Controll>();

        conObject.startTeleporting(targetPos - startPos);
     
        Lerping = true;

        //ObjectPool.getObjectPool().getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.camera).GetComponent<justLook>().objectToLookAt = gameObject;

        gameObject.GetComponent<Player>().MeshRenderer.enabled = false;
        teleportationBall = ObjectPool.getObjectPool().getObject(ObjectPool.categorie.explosion, (int)ObjectPool.explosion.heroTeleportBall);
        lookedATarget = false;
}       

public void Update()
    {
        if (Lerping == true)
        {
            float disCovered = (Time.time - StartTime) * Speed;
            float ratio = disCovered / Distance;

            transform.position = Vector3.Lerp(StartPos, TargetPos, ratio);
            teleportationBall.transform.position = transform.position;


            conObject.updateTeleportProgress(ratio);

            if (lookedATarget == false)
            {
                teleportationBall.transform.LookAt(TargetPos);
                lookedATarget = true;
            }

            if (ratio >= 1)
            {
                Lerping = false;

                transform.position = TargetPos;

                conObject.endTeleporting();

                gameObject.GetComponent<Player>().MeshRenderer.enabled = true;
                ObjectPool.getObjectPool().returnObject(teleportationBall);

                //TODO Bug für cam zurücksetzen zwischen meheren Lerps beheben...
                //ObjectPool.getObjectPool().getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.camera).GetComponent<justLook>().objectToLookAt = null;

            }
        }

    }
}
