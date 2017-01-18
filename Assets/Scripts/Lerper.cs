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

        Lerping = true;

    }

    public void Update()
    {
        if (Lerping == true)
        {
            float disCovered = (Time.time - StartTime) * Speed;
            Ratio = disCovered / Distance;

            transform.position = Vector3.Lerp(StartPos, TargetPos, Ratio);

            if (Ratio >= 1)
            {
                transform.position = TargetPos;

                Lerping = false;
            }
        }
  
    }
}
