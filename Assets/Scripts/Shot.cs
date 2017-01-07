using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Shot : MonoBehaviour
{
    //Just needs to be initialized once.
    private ObjectPool mr;
    private Rigidbody rb;

    //Needs a reset every time it comes back from the ObjectPool
    private GameObject shootedFrom;
    public GameObject _shootedFrom
    {
        get { return shootedFrom; }
        private set { }
    }

    private List<ShotEffect> effectsToExecute;

    private Vector3 startDirection;
    public Vector3 _startDirection
    {
        get { return startDirection; }
        private set { }
    }

    private float timeOutInSec = 3;
    private Coroutine timer;

    public void Awake()
    {
        mr = ObjectPool.getObjectPool();
        rb = GetComponent<Rigidbody>();
        effectsToExecute = new List<ShotEffect>();
        effectsToExecute.Add(new basic());
    }

    public void reset(GameObject shootedFrom, Vector3 horizontalDirection, Quaternion rotation)
    {
        reset(shootedFrom, horizontalDirection, rotation, shootedFrom.GetComponent<Stats>().possibleShotEffects);
    }

    public void reset(GameObject shootedFrom, Vector3 horizontalDirection, Quaternion rotation, List<ShotEffect> Effects)
    {
        this.shootedFrom = shootedFrom;
        this.tag = shootedFrom.tag;
        rb.velocity = new Vector3(0, 0, 0);

        transform.position = shootedFrom.transform.position + shootedFrom.GetComponent<Stats>().shootOffset;
        startDirection = shootedFrom.GetComponent<CharacterController>().velocity;
        //startDirection = (horizontalDirection.normalized + new Vector3(0,0.5f,0)) * shootedFrom.GetComponent<Stats>().shotSpeed * shootedFrom.GetComponent<Stats>().speed;   // TODO
        startDirection = (horizontalDirection.normalized + new Vector3(0, 0.2f, 0)) * shootedFrom.GetComponent<Stats>().shotSpeed;

        transform.rotation = rotation;
        timer = StartCoroutine(timerHandler());
        this.GetComponent<AudioSource>().Play();

        for (int i = 0; i < effectsToExecute.Count; i++)
        {
            effectsToExecute[i].triggerStart(gameObject);
        }
    }

    void FixedUpdate()
    {
        for(int i = 0; i < effectsToExecute.Count; i++)
        {
            effectsToExecute[i].triggerFixUpate(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor")
        {
            for (int i = 0; i < effectsToExecute.Count; i++)
            {
                effectsToExecute[i].triggerHitStructure(gameObject);
            }
        }
        if (collision.collider.tag == "Enemy")
        {
            for (int i = 0; i < effectsToExecute.Count; i++)
            {
                effectsToExecute[i].triggerHitEnemy( gameObject, collision.collider.gameObject);
            }
        }
        if (collision.collider.tag == "Floor" || collision.collider.tag == "Enemy")
        {
            StopCoroutine(timer);
            mr.returnObject(gameObject);
        }
    }

    private IEnumerator timerHandler()
    {
        yield return new WaitForSeconds(timeOutInSec);
        StopCoroutine(timer);
        mr.returnObject(gameObject);
    }

}
