using UnityEngine;
using System.Collections;

public class Lerper : MonoBehaviour {

    [HideInInspector]
    public bool Lerping = false;
    [HideInInspector]
    public float Ratio;

    private Vector3 StartPos;
    private Vector3 TargetPos;
    private float Speed;

    private float Distance;
    private float StartTime;

    //private Controll conObject;

	public void StartLerp(Vector3 startPos, Vector3 targetPos, float speed)
    {
        StartPos = startPos;
        TargetPos = targetPos;
        Speed = speed;

        Distance = Vector3.Distance(startPos, targetPos);
        StartTime = Time.time;

        /*conObject = GameMaster.getGameMaster().GetComponent<Controll>();

        if (gameObject.tag == "Player")
        {
            conObject.startTeleporting(targetPos - startPos);
        } */

        Lerping = true;

        // ObjectPool.getObjectPool().getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.camera).GetComponent<justLook>().objectToLookAt = gameObject;
    }

    public void Update()
    {
        if (Lerping == true)
        {
            float disCovered = (Time.time - StartTime) * Speed;
            Ratio = disCovered / Distance;

            transform.position = Vector3.Lerp(StartPos, TargetPos, Ratio);
            /*if (gameObject.tag == "Player")
            {
                conObject.updateTeleportProgress(ratio);
            }*/

            if (Ratio >= 1)
            {
                Lerping = false;
                /*if (gameObject.tag == "Player")
                {
                    conObject.endTeleporting();
                }
                //TODO Bug für cam zurücksetzen zwischen meheren Lerps beheben...
                ObjectPool.getObjectPool().getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.camera).GetComponent<justLook>().objectToLookAt = null;
                */
            }
        }
  
    }
}
