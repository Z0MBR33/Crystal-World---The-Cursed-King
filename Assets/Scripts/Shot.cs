using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Shot : MonoBehaviour
{
    //Just needs to be initialized once.
    private ObjectPool mr;
    private Rigidbody rb;
    private Vector3 normalScale;

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
        set { }
    }

    private float shotStrength;
    public float _shotStrength
    {
        get { return shotStrength; }
        set {
            transform.localScale = normalScale * (value / shootedFrom.GetComponent<Stats>()._startDmg);
            shotStrength = value;

        }
    }

    private float timeOutInSec = 3;
    private Coroutine timer;

    private bool canPlop = false;
    private Coroutine plopTimer;

    public void Awake()
    {
        mr = ObjectPool.getObjectPool();
        rb = GetComponent<Rigidbody>();
        normalScale = transform.localScale;
        effectsToExecute = new List<ShotEffect>();
        effectsToExecute.Add(new basic());
    }

    public void reset(GameObject shootedFrom, Vector3 horizontalDirection, Quaternion rotation)
    {

        Vector3 startPosition = shootedFrom.transform.position + shootedFrom.GetComponent<Stats>().shootOffset;
        Vector3 startDirection = shootedFrom.GetComponent<CharacterController>().velocity;
        startDirection += (horizontalDirection.normalized + new Vector3(0, 0.2f, 0)) * shootedFrom.GetComponent<Stats>().shotSpeed;

        List<ShotEffect> listToCopy = shootedFrom.GetComponent<Stats>().possibleShotEffects;

        List<ShotEffect> copyOfEffectList = new List<ShotEffect>();
        ShotEffect[] pufferarray = new ShotEffect[listToCopy.Count];
        listToCopy.CopyTo(pufferarray);
        copyOfEffectList.AddRange(pufferarray);
        reset(shootedFrom, startPosition, startDirection, rotation, copyOfEffectList);
    }

    public void reset(GameObject shootedFrom, Vector3 startPosition, Vector3 startDirection, Quaternion rotation, List<ShotEffect> effects)
    {
        this.shootedFrom = shootedFrom;
        Stats shooterData = shootedFrom.GetComponent<Stats>();
        this.tag = shootedFrom.tag;
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = startPosition;
        this.startDirection = startDirection;
        effectsToExecute = effects;
        transform.rotation = rotation;

        timer = StartCoroutine(timerHandler());
        canPlop = false;
        plopTimer = StartCoroutine(plopTimerHandler());
        shotStrength = shooterData.shotStrength;
        float shotDmgTpScaleMultiplier = shotStrength / shooterData._startDmg;
        transform.localScale = normalScale * shotDmgTpScaleMultiplier;

        for (int i = 0; i < effectsToExecute.Count; i++)
        {
            effectsToExecute[i].triggerStart(gameObject);
        }
    }

    public void Update()
    {
        for (int i = 0; i < effectsToExecute.Count; i++)
        {
            effectsToExecute[i].triggerUpdate(gameObject);
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < effectsToExecute.Count; i++)
        {
            effectsToExecute[i].triggerFixUpdate(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        executeCollison(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        //executeCollison(collision);
    }

    void executeCollison(Collision collision)
    {
        if (canPlop)
        {
            if (collision.collider.tag == "Enemy")
            {
                for (int i = 0; i < effectsToExecute.Count; i++)
                {
                    effectsToExecute[i].triggerHitEnemy(gameObject, collision.collider.gameObject);
                }
            }
            else if (collision.collider.tag == "Player")
            {
                for (int i = 0; i < effectsToExecute.Count; i++)
                {
                    effectsToExecute[i].triggerHitPlayer(gameObject);
                }
            }
            else
            {
                for (int i = 0; i < effectsToExecute.Count; i++)
                {
                    effectsToExecute[i].triggerHitStructure(gameObject);
                }
            }
            StopAllCoroutines();
            mr.returnObject(gameObject);
        }
    }

    private IEnumerator timerHandler()
    {
        yield return new WaitForSeconds(timeOutInSec);
        for (int i = 0; i < effectsToExecute.Count; i++)
        {
            effectsToExecute[i].triggerHitStructure(gameObject);
        }
        StopAllCoroutines();
        mr.returnObject(gameObject);
    }

    private IEnumerator plopTimerHandler()
    {
        yield return new WaitForSeconds(0.1f);
        StopCoroutine(plopTimer);
        canPlop = true;
    }

    public List<ShotEffect> getActivShotEffects()
    {
        return effectsToExecute;
    }

    public void removeActivShotEffect(ShotEffect effectToRemove)
    {
        effectsToExecute.Remove(effectToRemove);
    }
}
