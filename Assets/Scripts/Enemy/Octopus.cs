using UnityEngine;
using System.Collections;

public class Octopus : Enemy {

    protected override GameObject getGhost()
    {
        return mr.getObject(ObjectPool.categorie.enemy, (int)ObjectPool.enemy.octopusGhost);
    }

}
