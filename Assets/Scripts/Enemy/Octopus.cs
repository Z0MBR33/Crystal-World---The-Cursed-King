using UnityEngine;
using System.Collections;

public class Octopus : Enemy
{

    public float walkTimeSeconds;
    public float loadTimeSeconds;
    public float shotSize;
    public Animator anim;
    public GameObject ShotPoint;

    private bool walking;

    private GameObject loadingShot;

    private Vector3 posBeforeShooting;
    private float shotScale;
    private float startTime;
    private Coroutine octopusRythm;
    Rigidbody rb;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        anim.SetBool("shoot", !walking);
        if (walking == true)
        {
            posBeforeShooting = transform.position;
        }
        else
        {
            transform.position = posBeforeShooting;
            Vector3 vec = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(vec);

            loadShot();
        }
    }

    public override void Initialize(EnemyPoint enemyPoint, Vector3 islePosition, Vector3 navMeshPosition, NavMeshTarget target)
    {
        base.Initialize(enemyPoint, islePosition, navMeshPosition, target);

        posBeforeShooting = transform.position;
        loadingShot = null;

        octopusRythm = StartCoroutine(octopusRythmHandler());
    }

    protected override GameObject getGhost()
    {
        return mr.getObject(ObjectPool.categorie.enemy, (int)ObjectPool.enemy.octopusGhost);
    }

    public override void die()
    {
        if (loadingShot != null)
        {
            mr.returnObject(loadingShot);
            loadingShot = null;
        }

        StopAllCoroutines();

        base.die();
    }

    public override void getPushed(Vector3 pushDirection, float force)
    {
        // YES! MUST BE EMPTY!
    }

    private void loadShot()
    {
        if (loadingShot == null)
        {
            loadingShot = mr.getObject(ObjectPool.categorie.shot, (int)ObjectPool.shot.loadingBall);
            shotScale = 0;
            loadingShot.transform.localScale = new Vector3(shotScale, shotScale, shotScale);
            startTime = Time.time;
        }

        loadingShot.transform.position = ShotPoint.transform.position;
        loadingShot.transform.rotation = transform.rotation;

        float timeLoading = (Time.time - startTime);
        float ratio = timeLoading / loadTimeSeconds;

        shotScale = shotSize * ratio;

        loadingShot.transform.localScale = new Vector3(shotScale, shotScale, shotScale);
    }

    private void shoot()
    {
        // shot! TODO

        if (loadingShot != null)
        {
            mr.returnObject(loadingShot);
            loadingShot = null;
        }
    }

    public IEnumerator octopusRythmHandler()
    {
        walking = true;

        while (true)
        {
            if (walking)
            {
                ghostCopy.MovedByGhost = true;

                yield return new WaitForSeconds(walkTimeSeconds);
                walking = false;
            }
            else
            {
                ghostCopy.MovedByGhost = false;

                yield return new WaitForSeconds(loadTimeSeconds);
                shoot();

                walking = true;
            }
        }
    }
}
