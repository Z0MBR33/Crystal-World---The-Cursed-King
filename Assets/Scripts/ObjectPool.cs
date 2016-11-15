using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Diese Klasse stellt einen ObjectPool für andere Scripte bereit.
/// Sie wird einmal auf ein Objekt gezogen! 
/// Andere Skripte holen sich die Referenz per ObjectPool.getObjectPool();
/// Die Prefabs werden in dieser Version per drag and drop im Editor gesetzt.
/// Die Listen muessen von NULL Werten frei bleiben.
/// Die Listen fungieren Stackaehnlich.
/// Per get/return Methoden können sich andere Skripte Objekte holen/wiederbringen.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool objectPool;


    public GameObject playerPrefab;                     //Spieler PreFab
    public GameObject cameraPrefab;                     //Kamera PreFab
    //Was soll gespawnt werden.
    //Wie viele sollen gespawnt werden. 

    //Gegner
    public List<GameObject> enemiesPrefabs; 
    public List<int> enemiesCount; 

    //Strukturen
    public List<GameObject> structuresPrefabs;
    public List<int> structuresCount;


    private GameObject playerObject;                    //Spieler Objekt
    private GameObject cameraObject;                    //Kamera Objekt

    private List<Stack<GameObject>> enemiesObjectPool;   //instanztizierte Objekte
    private List<Stack<GameObject>> structuresObjectPool;

    private List<List<GameObject>> activeEnemies;       //aktiv vergebene Objekte
    private List<List<GameObject>> activeStructures;


    public static ObjectPool getObjectPool()
    {
        return objectPool;
    }

    void Awake()
    {
        objectPool = this;
    }
    void Start()
    {
        playerObject = Instantiate(playerPrefab);
        playerObject.SetActive(false);

        cameraObject = Instantiate(cameraPrefab);
        cameraObject.SetActive(false);

        enemiesObjectPool = createInstances(enemiesPrefabs, enemiesCount);
        activeEnemies = createEmptyLists(enemiesPrefabs.Count);

        structuresObjectPool = createInstances(structuresPrefabs, structuresCount);
        activeStructures = createEmptyLists(structuresPrefabs.Count);
    }

    private List<List<GameObject>> createEmptyLists(int quantity)
    {
        List<List<GameObject>> toReturn = new List<List<GameObject>>();
        for(int i = 0; i < quantity; i++)
        {
            toReturn.Add(new List<GameObject>());
        }
        return toReturn;
    }

    private List<Stack<GameObject>> createInstances(List<GameObject> toSpawn, List<int> amount)
    {
        Stack<GameObject> spawned;
        GameObject spawning;
        List<Stack<GameObject>> toReturn = new List<Stack<GameObject>>();

        for (int i = 0; i < toSpawn.Count; i++)
        {

            spawned = new Stack<GameObject>();
            for (int j = 0; j < amount[i]; j++)
            {
                spawning = Instantiate(toSpawn[i]);
                spawning.SetActive(false);
                spawned.Push(spawning); 
            }
            toReturn.Add(spawned);
        }

        return toReturn;
    }

    //Sicherung für was wenn mehr rausgenommen werden soll? Methoden für Stack umschreiben.
    public GameObject getEnemie(int id)
    {
        if (enemiesObjectPool[id].Count == 0)
        {
            enemiesObjectPool[id].Push(Instantiate(structuresPrefabs[id]));
        }
        GameObject toReturn = enemiesObjectPool[id].Pop();
        activeEnemies[id].Add(toReturn);
        return toReturn;
    }

    public void returnEnemie(GameObject objectToGiveBack, int id)
    {
        objectToGiveBack.SetActive(false);
        activeEnemies[id].Remove(objectToGiveBack);
        enemiesObjectPool[id].Push(objectToGiveBack);
    }

    public GameObject getStructure(int id)
    {
        if (structuresObjectPool[id].Count == 0)
        {
            structuresObjectPool[id].Push(Instantiate(structuresPrefabs[id]));
        }
        GameObject toReturn = structuresObjectPool[id].Pop();
        activeStructures[id].Add(toReturn);
        return toReturn;
    }

    public void returnStructure(GameObject objectToGiveBack, int id)
    {
        objectToGiveBack.SetActive(false);
        activeStructures[id].Remove(objectToGiveBack);
        structuresObjectPool[id].Push(objectToGiveBack);
    }

    public GameObject getPlayer()
    {
        return playerObject;
    }
    public GameObject getCamera()
    {
        return cameraObject;
    }
}
