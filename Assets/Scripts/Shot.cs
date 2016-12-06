using UnityEngine;
using System.Collections;


public class Shot : MonoBehaviour
{
    //Just needs to be initialized once.
    private ObjectPool mr;
    private Rigidbody rb;

    //Needs a reset every time it comes back from the ObjectPool
    private GameObject shootedFrom;
    private Classes.ShotMode ModeOfShot;
    private Vector3 Direction;
    private float Speed;
    public ParticleSystem explosion;
    private Coroutine timer;

    public void Awake()
    {
        mr = ObjectPool.getObjectPool();
        rb = GetComponent<Rigidbody>();
    }

    public void reset(GameObject shootedFrom, Vector3 horizontalDirection, Quaternion rotation)
    {
        this.shootedFrom = shootedFrom;
        transform.position = shootedFrom.transform.position + shootedFrom.GetComponent<Stats>().shootOffset;
        Direction = horizontalDirection.normalized;
        transform.rotation = rotation;
        //implement vertical Direction!
        this.tag = shootedFrom.tag;
        ModeOfShot = shootedFrom.GetComponent<Stats>()._randomShotType;
        Speed = shootedFrom.GetComponent<Stats>().shotSpeed;
        timer = StartCoroutine(timerHandler());
        this.GetComponent<AudioSource>().Play();
    }


    void FixedUpdate()
    {
        if (ModeOfShot == Classes.ShotMode.Rocket)
        {
            rb.velocity = Direction * Speed;
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
                collision.collider.GetComponent<Enemy>().getPushed(rb.velocity, 5);
                Stats targetStats = collision.collider.GetComponent<Stats>();
                targetStats.gotHit(shootedFrom.GetComponent<Stats>().shotStrength);
            }

            StopCoroutine(timer);
            mr.returnShot(gameObject, 0);
        }
    }

    private IEnumerator timerHandler()
    {
        yield return new WaitForSeconds(3);
        StopCoroutine(timer);
        mr.returnShot(gameObject, 0);
    }

}
