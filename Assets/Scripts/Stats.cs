using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stats : MonoBehaviour
{

    //Char stats
    public float health = 10;
    public float speed = 10.0f;
    public float luck = 0.0f;

    //Shoot stats
    public Vector3 shootOffset = new Vector3(0, 1, 0);
    public float StartVerticalDegree = 45;
    public float shotSpeed = 10.0f;
    public float shotStrength = 5.0f;
    public float fireRate = 0.1f;
    public List<Classes.ShotMode> possibleShotMode;

    public Classes.ShotMode _randomShotType { get {return possibleShotMode[Mathf.RoundToInt(Random.value * (possibleShotMode.Count - 1))]; } }

    public void Awake()
    {
        possibleShotMode = new List<Classes.ShotMode>();
        possibleShotMode.Add(Classes.ShotMode.Rocket);
    }

    public void gotHit(float damage, Vector3 pushDirection, float force)
    {
        health -= damage;
        
        if (gameObject.tag == "Enemy")
        {
            gameObject.GetComponent<Enemy>().getPushed(pushDirection, force);
            if (health <= 0)
            {
                gameObject.GetComponent<Enemy>().die();
            }
        }
    }
}
