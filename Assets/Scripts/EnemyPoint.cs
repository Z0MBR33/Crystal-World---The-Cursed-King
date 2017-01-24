using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPoint : MonoBehaviour {

    [Range(0, 101)]
    public int ChangeForSpawn;

    public bool Slime;
    public bool Octopus;
    public bool Spider;

    private ObjectPool mr;

    [HideInInspector]
    public bool CanCreateEnemy;

    [HideInInspector]
    public Vector3 IslePosition;

    public Vector3 getPositionOnIsle()
    {
        return transform.position - IslePosition;
    }

    public bool Initialize()
    {
        mr = ObjectPool.getObjectPool();

        int tmp = mr.random.Next(0, 101);

        if (tmp <= ChangeForSpawn)
        {
            CanCreateEnemy = true;
        }else
        {
            CanCreateEnemy = false;
        }

        return CanCreateEnemy;
    }

    public GameObject createEnemy()
    {
        GameObject enemy = null;
        
        if (CanCreateEnemy == false)
        {
            return enemy;
        }

        List<int> list = new List<int>();

        if (Slime == true)
        {
            list.Add(0);
        }
        if (Octopus == true)
        {
            list.Add(1);
        }
        if (Spider == true)
        {
            list.Add(2);
        }

        if (list.Count > 0)
        {
            int tmp = mr.random.Next(0, list.Count);

            int enemyNo = list[tmp];

            switch(enemyNo)
            {
                case 0: enemy = mr.getObject(ObjectPool.categorie.enemy, (int)ObjectPool.enemy.slime);
                    break;
                case 1: enemy = mr.getObject(ObjectPool.categorie.enemy, (int)ObjectPool.enemy.octopus);
                    break;
                case 2: enemy = mr.getObject(ObjectPool.categorie.enemy, (int)ObjectPool.enemy.spider);
                    break;
            }
        }
        

        return enemy;
    }
}
