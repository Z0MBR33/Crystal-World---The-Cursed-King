using UnityEngine;
using System.Collections;
using System.Timers;
using System;

public class Shot : MonoBehaviour {

    private Rigidbody rb;
    private Classes.ShotMode ModeOfShot;
    private Vector3 Direction;
    private float Speed;
    public ParticleSystem explosion;

    public static GameObject Create(GameObject prefab, Classes.ShotMode shotMode, Vector3 pos, Vector3 direction, Quaternion rotation, float speed, float scale)
    {
        GameObject obj = Instantiate(prefab, pos, rotation) as GameObject;

        Shot shot = obj.GetComponent<Shot>();

        shot.ModeOfShot = shotMode;
        direction.Normalize();
        shot.Direction = direction;
        shot.Speed = speed;
        shot.transform.localScale = new Vector3(scale, scale, scale);

        return obj;
    }

   
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();

        StartCoroutine(timerHandler());
	}
	
	void FixedUpdate () {
        if (ModeOfShot == Classes.ShotMode.Rocket)
        {
            rb.velocity = this.Direction * this.Speed;
        }    
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor" || collision.collider.tag ==  "Enemy")
        {
            Instantiate(explosion, transform.position, new Quaternion());

            Destroy(gameObject);
        }
    }

    private IEnumerator timerHandler()
    {
        yield return new WaitForSeconds(3);

        Destroy(gameObject);
    }

}
