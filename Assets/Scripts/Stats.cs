using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    public float shotSpeed = 10.0f;
    public float shotStrength = 5.0f;
    public float fireRate = 0.1f;
    public List<Classes.ShotMode> possibleShotMode;

    public Classes.ShotMode _randomShotType { get { return possibleShotMode[Mathf.RoundToInt(Random.value * (possibleShotMode.Count - 1))]; } }

    public void Awake()
    {
        resetStats();
        possibleShotMode = new List<Classes.ShotMode>();
        possibleShotMode.Add(Classes.ShotMode.Rocket);
    }

    public void resetStats()
    {
        health = maxHealth;
    }

    public void gotHit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (gameObject.tag == "Enemy")
            {
                gameObject.GetComponent<Enemy>().die();
            }
            if (gameObject.tag == "Player")
            {
                SceneManager.LoadScene("Scenes/Main_Menue");
            }
        }
    }
}
