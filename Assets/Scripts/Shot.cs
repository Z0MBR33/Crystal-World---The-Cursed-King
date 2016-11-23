using UnityEngine;
using System.Collections;


public class Shot : MonoBehaviour
{

    private Rigidbody rb;
    private Classes.ShotMode ModeOfShot;
    private Vector3 Direction;
    private float Speed;
    public ParticleSystem explosion;

    private ObjectPool mr;

    public void Initialize(Classes.ShotMode shotMode, Vector3 pos, Vector3 direction, Quaternion rotation, float speed, float scale)
    {

        mr = ObjectPool.getObjectPool();
        rb = GetComponent<Rigidbody>();

        this.ModeOfShot = shotMode;
        transform.position = pos;

        direction.Normalize();
        this.Direction = direction;
        transform.rotation = rotation;
        this.Speed = speed;
        this.transform.localScale = new Vector3(scale, scale, scale);

        StartCoroutine(timerHandler());

        rb.velocity = Vector3.zero;
    }


    void FixedUpdate()
    {
        if (ModeOfShot == Classes.ShotMode.Rocket)
        {
            rb.velocity = this.Direction * this.Speed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor" || collision.collider.tag == "Enemy")
        {
            ExplosionScript expl = mr.getExplosion(0).GetComponent<ExplosionScript>();
            expl.gameObject.SetActive(true);
            expl.Initialize(transform.position, new Quaternion());

            // check for enemy hit
            if (collision.collider.tag == "Enemy")
            {
                Enemy enemy = collision.collider.GetComponent<Enemy>();
                enemy.TakeDamage(1, rb.velocity, 5);
            }

            StopCoroutine(timerHandler());
            mr.returnShot(gameObject, 0);
        }
    }

    private IEnumerator timerHandler()
    {
        yield return new WaitForSeconds(3);

        StopCoroutine(timerHandler());
        mr.returnShot(gameObject, 0);

    }

}
