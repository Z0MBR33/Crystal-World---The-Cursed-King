﻿using UnityEngine;
using System.Collections;

public class Octopus : Enemy
{

    public float walkTimeSeconds;
    public float walkTimeDifference;
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
        Stats stats = GetComponent<Stats>();

        Shot shot = mr.getObject(ObjectPool.categorie.shot, (int)ObjectPool.shot.roundEnemy).GetComponent<Shot>();
        shot.gameObject.transform.localScale = new Vector3(shotSize, shotSize, shotSize);

        Vector3 startPoint = loadingShot.GetComponent<LoadingBall>().CenterOfBall.transform.position;
        Vector3 startDirection = transform.forward + new Vector3(0, 0, 0);
        startDirection *= stats.shotSpeed;
        float timeUntilFalloff = stats.TimeUntilShotFalloff;

        shot.reset(gameObject, startPoint, startDirection, timeUntilFalloff, stats.possibleShotEffects);

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

                // calculate difference-wait-time
                float max = walkTimeDifference * 100;
                float offset = mr.random.Next(0, (int)max);
                offset = offset - (max/2);
                offset = offset / 100;

                float actualWaitTimeSeconds = walkTimeSeconds + offset;

                // wait
                yield return new WaitForSeconds(actualWaitTimeSeconds);
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
