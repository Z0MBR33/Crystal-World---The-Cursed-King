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
        transform.rotation = rotation;

        this.tag = shootedFrom.tag;
        ModeOfShot = shootedFrom.GetComponent<Stats>()._randomShotType;
        
        Direction = horizontalDirection;
        //implement vertical Direction!
        
        Speed = shootedFrom.GetComponent<Stats>().shotSpeed;

        timer = StartCoroutine(timerHandler());

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
                Stats targetStats = collision.collider.GetComponent<Stats>();
                targetStats.TakeDamage(1, rb.velocity, 5);
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
