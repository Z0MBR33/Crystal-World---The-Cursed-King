using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stats : MonoBehaviour
{

    //Char stats
    public float maxHealth = 10;
    public float health = 10;
    public float speed = 10.0f;
    public float luck = 0.0f;
    public float strength = 10;

    //Shoot stats
    public Vector3 shootOffset = new Vector3(0, 1, 0);
    public float StartVerticalDegree = 45;
    public float shotSpeed = 1.0f;
    public float shotStrength = 5.0f;
    public float fireRate = 0.1f;
    public List<ShotEffect> possibleShotEffects;

    public void Awake()
    {
        resetStats();
        possibleShotEffects = new List<ShotEffect>();
        possibleShotEffects.Add(new basic());
        //possibleShotEffects.Add(new multiplyOnContact());
    }

    public void resetStats()
    {
        health = maxHealth;
    }

    public void gotHit(float damage)
    {

        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                if (gameObject.tag == "Enemy")
                {
                    gameObject.GetComponent<Enemy>().die();
                    return;
                }
                if (gameObject.tag == "Player")
                {
                    gameObject.GetComponent<Player>().Die();
                }
            }
        }
    }

}
