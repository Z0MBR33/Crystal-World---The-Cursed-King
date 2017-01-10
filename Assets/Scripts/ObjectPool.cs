using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Diese Klasse stellt einen ObjectPool für andere Scripte bereit.
/// Sie wird einmal auf ein Objekt gezogen! 
/// Andere Skripte holen sich die Referenz per ObjectPool.getObjectPool();
/// Die Prefabs werden in dieser Version per drag and drop im Editor gesetzt.
/// Per get/return Methode können sich andere Skripte Objekte holen/wiederbringen.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool objectPool;

    private List<List<Stack<GameObject>>> pool;
    private List<GameObject> activeObjects;
    public GameObject[] essentialPrefabs;
    public GameObject[] shotPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] ilandsPrefabs;
    public GameObject[] structuresPrefabs;
    public GameObject[] explosionPrefabs;
    public GameObject[] itemPrefabs;
    public GameObject[] planePrefabs;

    public System.Random random = new System.Random();

    //Enums
    public enum categorie
    {
        essential,
        shot,
        enemy,
        islands,
        structures,
        explosion,
        items,
        planes
    }

    public enum essential
    {
        player,
        camera,
        UI
    }

    public enum shot
    {
        round
    }

    public enum enemy
    {
        slimeGhost,
        slime,
        octopusGhost,
        octopus
    }

    public enum islands
    {
        boss_portal,
        type1,
        type2,
        type3,
        type4
        
    }

    public enum structures
    {
        portal,
        connection
    }

    public enum explosion
    {
        shot,
        enemyShot,
        enemy
    }

    public enum items
    {
        key1,
        key2,
        key3,
        smallBox,
        bigBox
    }

    public enum planes
    {
        deathPlane,
        borderPlane
    }

    //StartOperationen
    public static ObjectPool getObjectPool()
    {
        return objectPool;
    }

    void Awake()
    {
        objectPool = this;
        init();
    }

    void init()
    {
        activeObjects = new List<GameObject>();  // TODO   Liste hat gefehlt

        pool = new List<List<Stack<GameObject>>>();
        pool.Add(createListOfEmptyStacks(System.Enum.GetNames(typeof(essential)).Length));
        fillCategoriesStacks(pool[pool.Count-1], essentialPrefabs,1);
        pool.Add(createListOfEmptyStacks(System.Enum.GetNames(typeof(shot)).Length));
        fillCategoriesStacks(pool[pool.Count - 1], shotPrefabs, 20);
        pool.Add(createListOfEmptyStacks(System.Enum.GetNames(typeof(enemy)).Length));
        fillCategoriesStacks(pool[pool.Count - 1], enemyPrefabs, 20);
        pool.Add(createListOfEmptyStacks(System.Enum.GetNames(typeof(islands)).Length));
        fillCategoriesStacks(pool[pool.Count - 1], ilandsPrefabs, 20);
        pool.Add(createListOfEmptyStacks(System.Enum.GetNames(typeof(structures)).Length));
        fillCategoriesStacks(pool[pool.Count - 1], structuresPrefabs, 20);
        pool.Add(createListOfEmptyStacks(System.Enum.GetNames(typeof(explosion)).Length));
        fillCategoriesStacks(pool[pool.Count - 1], explosionPrefabs, 20);
        pool.Add(createListOfEmptyStacks(System.Enum.GetNames(typeof(items)).Length));
        fillCategoriesStacks(pool[pool.Count - 1], itemPrefabs, 20);
        pool.Add(createListOfEmptyStacks(System.Enum.GetNames(typeof(planes)).Length));
        fillCategoriesStacks(pool[pool.Count - 1], planePrefabs, 5);
    }


    private List<Stack<GameObject>> createListOfEmptyStacks(int quantity)
    {
        List<Stack<GameObject>> toReturn = new List<Stack<GameObject>>();
        for (int i = 0; i < quantity; i++)
        {
            toReturn.Add(new Stack<GameObject>());
        }
        return toReturn;
    }

    private void fillCategoriesStacks(List<Stack<GameObject>> listToFill, GameObject[] prefabsToFillIn, int amount)
    {
        for(int i = 0; i < listToFill.Count; i++)
        {
            fillStackWithObjects(listToFill[i], prefabsToFillIn[i], amount);
        }
    }

    private void fillStackWithObjects(Stack<GameObject> stackToFill, GameObject prefabToFillIn, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // TODO  alle Objekte waren anfangs an aktiv (böse bei den Partikel-Effekten, führte zu starken Rucklern)
            GameObject go = Instantiate(prefabToFillIn);
            go.SetActive(false);
            stackToFill.Push(go);
            //stackToFill.Push(Instantiate(prefabToFillIn));
        }
    }

    /// <summary>
    /// This method gives you an object out of the pool. You still need to reset it's values!
    /// </summary>
    /// <param name="cat"></param>
    /// <param name="id">Cast the wanted enum into an Integer.</param>
    /// <returns>The active Object, needs an reset of it's values!</returns>
    
    public GameObject getObject(categorie cat, int id)
    {
        GameObject toReturn;
        if (cat == categorie.essential)
        {
            toReturn = pool[(int)cat][id].Pop();
            pool[(int)cat][id].Push(toReturn);
        }
        else
        {
            if (pool[(int)cat][id].Count == 0)
            {
                pool[(int)cat][id].Push(initGameobject(cat, id));
            }
            toReturn = pool[(int)cat][id].Pop();
            activeObjects.Add(toReturn);
            toReturn.AddComponent<ObjectPoolAgent>();
            toReturn.GetComponent<ObjectPoolAgent>().tag_cat = cat;
            toReturn.GetComponent<ObjectPoolAgent>().tag_id = id;
            toReturn.SetActive(true);
        }
     
        toReturn.SetActive(true);  // TODO

        return toReturn;
    }

    private GameObject initGameobject(categorie cat, int id)
    {
        GameObject toReturn = new GameObject();

        switch (cat)
        {
            case categorie.essential: toReturn = Instantiate(essentialPrefabs[id]); break;
            case categorie.shot: toReturn = Instantiate(shotPrefabs[id]); break;
            case categorie.enemy: toReturn = Instantiate(enemyPrefabs[id]); break;
            case categorie.islands: toReturn = Instantiate(ilandsPrefabs[id]); break;
            case categorie.structures: toReturn = Instantiate(structuresPrefabs[id]); break;
            case categorie.explosion: toReturn = Instantiate(explosionPrefabs[id]); break;
            case categorie.items: toReturn = Instantiate(itemPrefabs[id]); break;
            case categorie.planes: toReturn = Instantiate(planePrefabs[id]); break;
            default: toReturn = null; break;
        }
        toReturn.SetActive(false);
        return toReturn;
    }

    /// <summary>
    /// <para>With this method, you can lay back an object into the pool.</para>
    /// If the object doesn't have a ObjectPoolAgent attached to it, this method will do nothing.
    /// The same applies for the object if it's in the essential group.
    /// </summary>
    /// <param name="objectToGiveBack"></param>
    public void returnObject(GameObject objectToGiveBack)
    {
        if (objectToGiveBack.GetComponent<ObjectPoolAgent>() == null)
        {
        }
        else
        {
            if (objectToGiveBack.GetComponent<ObjectPoolAgent>().tag_cat == categorie.essential)
            {
            }
            else
            {
                objectToGiveBack.SetActive(false);
                activeObjects.Remove(objectToGiveBack);
                pool[(int)objectToGiveBack.GetComponent<ObjectPoolAgent>().tag_cat][objectToGiveBack.GetComponent<ObjectPoolAgent>().tag_id].Push(objectToGiveBack);
                Destroy(objectToGiveBack.GetComponent<ObjectPoolAgent>());
            }
        }
    }

    /// <summary>
    /// <para>With this method, you can lay back all objects into the pool.</para>
    /// </summary>
    public void returnAllObjects()
    {
        for(int i = 0; i < activeObjects.Count; i++)
        {
            returnObject(activeObjects[i]);
            i--;
        }
    }

    public void preserveObjectsOnLoad()
    {
        DontDestroyOnLoad(gameObject);


        for (int i = 0; i < activeObjects.Count; i++)
        {
            DontDestroyOnLoad(activeObjects[i]);
        }

        for (int i = 0; i < pool.Count; i++)
        {
            for(int j = 0; j < pool[i].Count; j++)
            {
                Stack<GameObject> stack = new Stack<GameObject>();
                GameObject go;
                while (pool[i][j].Count > 0)
                {
                    go = pool[i][j].Pop();
                    DontDestroyOnLoad(go);
                    stack.Push(go);
                }

                while (stack.Count > 0)
                {
                    pool[i][j].Push(stack.Pop());
                }
            }
        }
    }

    public void DestroyAllObjects()
    {
        Destroy(gameObject);

        for (int i = 0; i < activeObjects.Count; i++)
        {
            Destroy(activeObjects[i]);
        }

        for (int i = 0; i < pool.Count; i++)
        {
            for (int j = 0; j < pool[i].Count; j++)
            {
                GameObject go;
                while (pool[i][j].Count > 0)
                {
                    go = pool[i][j].Pop();
                    Destroy(go);
                   
                }
            }
        }
    }
}
