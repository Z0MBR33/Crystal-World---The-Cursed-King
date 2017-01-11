using UnityEngine;
using System.Collections;

public class Spider : Enemy {

    protected override GameObject getGhost()
    {
        return mr.getObject(ObjectPool.categorie.enemy, (int)ObjectPool.enemy.spiderGhost);
    }

}
