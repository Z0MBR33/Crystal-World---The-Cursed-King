using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

    private ObjectPool mr;

    public enum ExplosionType { PlayerShot, EnemyShot, EnemyDeath};
    public ExplosionType explosionType;

    public void Initialize(ExplosionType type, Vector3 position, Quaternion rotation)
    {
        mr = ObjectPool.getObjectPool();

        explosionType = type;

        transform.position = position;
        transform.rotation = rotation;

        StartCoroutine(timerHandler());
    }
	
	private IEnumerator timerHandler()
    {
        yield return new WaitForSeconds(5);

        StopCoroutine(timerHandler());

        int itemID = 0;
        
        switch (explosionType)
        {
            case ExplosionType.PlayerShot: itemID = 0;
                break;
            case ExplosionType.EnemyShot: itemID = 1;
                break;
            case ExplosionType.EnemyDeath: itemID = 2;
                break;
        }

        mr.returnExplosion(gameObject, itemID);
        
    }
}
