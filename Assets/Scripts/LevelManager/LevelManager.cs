using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{

    public static LevelManager levelManager;
    private ObjectPool mr;

    [HideInInspector]
    public IsleAbstract[,] world;
    private List<IsleAbstract> isles;
    [HideInInspector]
    public List<ConnectionAbstract> connections;

    public int WorldWidth;
    public int WorldHeight;

    public float IsleDensity;

    public int Fieldwidth;

    public float AddConnPerIsle;

    [Range(0, 100)]
    public int ChancesBigBoxes;

    private List<Isle> islesObjects;
    private List<Connection> connectionObjects;

    [HideInInspector]
    public IsleAbstract currentIsle;
    [HideInInspector]
    public IsleAbstract startIsle;
    [HideInInspector]
    public IsleAbstract bossIsle;

    private System.Random rnd;

    public static LevelManager getLevelManager()
    {   
        return levelManager;
    }

    void Awake()
    {
        levelManager = this;
        mr = ObjectPool.getObjectPool();
    }

    public void GenerateMap()
    {
        rnd = new System.Random();

        if (islesObjects != null)
        {
            for (int i = 0; i < islesObjects.Count; i++)
            {
                mr.returnObject(islesObjects[i].gameObject);
            }
            islesObjects.Clear();
        }

        if (connectionObjects != null)
        {
            for (int i = 0; i < connectionObjects.Count; i++)
            {
                mr.returnObject(connectionObjects[i].gameObject);
            }
            connectionObjects.Clear();
        }

        // Create World-Array
        world = new IsleAbstract[WorldWidth, WorldHeight];

        int numberFields = WorldWidth * WorldHeight;

        // create list of all fields

        List<Vector2> fields = new List<Vector2>();
        for (int x = 0; x < WorldWidth; x++)
        {
            for (int y = 0; y < WorldHeight; y++)
            {
                fields.Add(new Vector2(x, y));
            }
        }


        // Create abstract isles and give isles individual fields

        IsleDensity = System.Math.Min(IsleDensity, 1);

        int numberOfIslands = (int)Mathf.Floor(numberFields * IsleDensity);  // Warum schickt Floor einen float zurück? Das war früher nicht so!

        isles = new List<IsleAbstract>();

        for (int i = 0; i < numberOfIslands; i++)
        {
            IsleAbstract isle = new IsleAbstract();
            int tmp = rnd.Next(0, fields.Count);
            isle.Index = fields[tmp];
            isles.Add(isle);
            fields.RemoveAt(tmp);

        }


        // set isles in world-array

        for (int i = 0; i < isles.Count; i++)
        {
            int x = (int)isles[i].Index.x;
            int y = (int)isles[i].Index.y;
            world[x, y] = isles[i];
        }

        buildMinimalTree();

        insertAdditionalConnections();

        inserObjectsOnMap();

        // render world

        renderWorld();

    }

    private void renderWorld()
    {
        islesObjects = new List<Isle>();
        connectionObjects = new List<Connection>();

        Fieldwidth = System.Math.Max(Fieldwidth, 10);

        IsleAbstract isle = null;
        for (int x = 0; x < world.GetLength(0); x++)
        {

            for (int y = 0; y < world.GetLength(1); y++)
            {
                if (world[x, y] == null)
                {
                    continue;
                }

                isle = world[x, y];

                int offset = 0;
                if (x % 2 == 1)
                {
                    offset = Fieldwidth / 2;
                }

                int isleHeight = rnd.Next(-50, 51);
                //int isleHeight = 0;

                Vector3 pos = new Vector3(isle.Index.x * Fieldwidth, isleHeight, (isle.Index.y * Fieldwidth) + offset);
                Isle isleObj;
                if (isle.isleObjectType != IsleAbstract.IsleObjectType.boss)
                {
                    int type = mr.random.Next(1, 4);
                    switch(type)
                    {
                        case 1: isleObj = mr.getObject(ObjectPool.categorie.islands, (int)ObjectPool.islands.type1).GetComponent<Isle>();
                            break;
                        case 2: isleObj = mr.getObject(ObjectPool.categorie.islands, (int)ObjectPool.islands.type2).GetComponent<Isle>();
                            break;
                        case 3: isleObj = mr.getObject(ObjectPool.categorie.islands, (int)ObjectPool.islands.type3).GetComponent<Isle>();
                            break;
                        case 4: isleObj = mr.getObject(ObjectPool.categorie.islands, (int)ObjectPool.islands.type4).GetComponent<Isle>();
                            break;
                        default: isleObj = mr.getObject(ObjectPool.categorie.islands, (int)ObjectPool.islands.type1).GetComponent<Isle>();
                            break;
                    }
                    
                } else
                {
                    isleObj = mr.getObject(ObjectPool.categorie.islands, (int)ObjectPool.islands.boss_portal).GetComponent<Isle>();
                }
                isleObj.transform.position = pos;
                isleObj.Initialize(isle);
                isle.IsleObj = isleObj;


                if (isle.Connected == true)
                {
                    //isleObj.gameObject.GetComponent<Renderer>().material.color = new Color(255, 0, 0);   // TODO
                }

                islesObjects.Add(isleObj);
            }
        }

        // render connections
        for (int i = 0; i < connections.Count; i++)
        {
            Connection connectionObj = mr.getObject(ObjectPool.categorie.structures, (int)ObjectPool.structures.connection).GetComponent<Connection>();
            connectionObj.connectionAbstract = connections[i];
            connections[i].connectionObj = connectionObj;

            LineRenderer lineRenderer = connectionObj.GetComponent<LineRenderer>();
            lineRenderer.SetVertexCount(2);
            lineRenderer.SetPosition(0, connections[i].Portal1.portalObj.transform.position + new Vector3(0, 1,0));
            lineRenderer.SetPosition(1, connections[i].Portal2.portalObj.transform.position + new Vector3(0, 1, 0));

            connectionObjects.Add(connectionObj);
        }

        // add death Place

        GameObject deathPlane = mr.getObject(ObjectPool.categorie.planes, (int)ObjectPool.planes.deathPlane);
        deathPlane.transform.position = new Vector3(0, -100, 0);
    }

    private void buildMinimalTree()
    {
        if (isles == null) return;

        connections = new List<ConnectionAbstract>();

        int tmp = rnd.Next(0, isles.Count);
        IsleAbstract startIsle = isles[tmp];

        searchNeighbour(startIsle, null, -1);
    }

    private void searchNeighbour(IsleAbstract current, IsleAbstract last, int directionFrom)
    {

        if (current.Connected == true)
        {
            return;
        }

        // Connect isles
        current.Connected = true;
        if (last != null)
        {
            connectIsles(last, current, directionFrom);
        }


        List<int> directions = new List<int>();

        for (int i = 0; i < 6; i++)
        {
            directions.Add(i);
        }

        // remove direction which is comes from
        int directionsToCheck = 6;
        if (directionFrom != -1)
        {
            directions.Remove(directionFrom);
            directionsToCheck--;
        }

        for (int i = 0; i < directionsToCheck; i++)
        {
            int tmp = rnd.Next(0, directions.Count);
            int direction = directions[tmp];
            directions.RemoveAt(tmp);

            IsleAbstract isle = null;

            switch (direction)
            {
                case 0:
                    isle = travelUp((int)current.Index.x, (int)current.Index.y);
                    break;
                case 1:
                    isle = travelUpRight((int)current.Index.x, (int)current.Index.y);
                    break;
                case 2:
                    isle = travelDownRight((int)current.Index.x, (int)current.Index.y);
                    break;
                case 3:
                    isle = travelDown((int)current.Index.x, (int)current.Index.y);
                    break;
                case 4:
                    isle = travelDownLeft((int)current.Index.x, (int)current.Index.y);
                    break;
                case 5:
                    isle = travelUpLeft((int)current.Index.x, (int)current.Index.y);
                    break;
            }

            if (isle != null)
            {
                direction = (direction + 3) % 6;
                searchNeighbour(isle, current, direction);

            }
        }

    }

    private void connectIsles(IsleAbstract isle1, IsleAbstract isle2, int directionFrom)
    {
        PortalAbstract portal1 = new PortalAbstract();
        PortalAbstract portal2 = new PortalAbstract();
        ConnectionAbstract connection = new ConnectionAbstract();

        portal1.isleAbstract = isle1;
        portal1.ConnectedPortal = portal2;
        portal1.Connection = connection;
        
        portal2.isleAbstract = isle2;
        portal2.ConnectedPortal = portal1;
        portal2.Connection = connection;
        
        connection.Portal1 = portal1;
        connection.Portal2 = portal2;
        connections.Add(connection);

        isle1.Portals[(directionFrom + 3) % 6] = portal1;
        isle2.Portals[directionFrom] = portal2;
    }

    public IsleAbstract travelInDirection(int startX, int startY, int direction)
    {
        IsleAbstract endIsle = null;

        switch(direction)
        {
            case 0: endIsle = travelUp(startX, startY);
                break;
            case 1:
                endIsle = travelUpRight(startX, startY);
                break;
            case 2:
                endIsle = travelDownRight(startX, startY);
                break;
            case 3:
                endIsle = travelDown(startX, startY);
                break;
            case 4:
                endIsle = travelDownLeft(startX, startY);
                break;
            case 5:
                endIsle = travelUpLeft(startX, startY);
                break;
        }

        return endIsle;
    }

    public IsleAbstract travelUp(int startX, int startY)
    {
        IsleAbstract endIsle = null;

        for (int y = startY + 1; y < world.GetLength(1); y++)
        {
            if (world[startX, y] != null)
            {
                endIsle = world[startX, y];
                break;
            }
        }

        return endIsle;
    }

    public IsleAbstract travelDown(int startX, int startY)
    {
        IsleAbstract endIsle = null;

        for (int y = startY - 1; y >= 0; y--)
        {
            if (world[startX, y] != null)
            {
                endIsle = world[startX, y];
                break;
            }
        }

        return endIsle;
    }

    public IsleAbstract travelUpRight(int startX, int startY)
    {
        IsleAbstract endIsle = null;

        int x = startX;
        int y = startY;

        while (true)
        {

            if (x % 2 == 1)
            {
                y++;
            }

            x++;

            if (x >= world.GetLength(0) || y >= world.GetLength(1))
            {
                return null;
            }

            if (world[x, y] != null)
            {
                endIsle = world[x, y];
                break;
            }

        }

        return endIsle;
    }

    public IsleAbstract travelDownRight(int startX, int startY)
    {
        IsleAbstract endIsle = null;

        int x = startX;
        int y = startY;

        while (true)
        {

            if (x % 2 == 0)
            {
                y--;
            }

            x++;

            if (x >= world.GetLength(0) || y < 0)
            {
                return null;
            }

            if (world[x, y] != null)
            {
                endIsle = world[x, y];
                break;
            }

        }

        return endIsle;
    }

    public IsleAbstract travelUpLeft(int startX, int startY)
    {
        IsleAbstract endIsle = null;

        int x = startX;
        int y = startY;

        while (true)
        {

            if (x % 2 == 1)
            {
                y++;
            }

            x--;

            if (x < 0 || y >= world.GetLength(1))
            {
                return null;
            }

            if (world[x, y] != null)
            {
                endIsle = world[x, y];
                break;
            }

        }

        return endIsle;
    }

    public IsleAbstract travelDownLeft(int startX, int startY)
    {
        IsleAbstract endIsle = null;

        int x = startX;
        int y = startY;

        while (true)
        {

            if (x % 2 == 0)
            {
                y--;
            }

            x--;

            if (x < 0 || y < 0)
            {
                return null;
            }

            if (world[x, y] != null)
            {
                endIsle = world[x, y];
                break;
            }

        }

        return endIsle;
    }

    private void insertAdditionalConnections()
    {
        //AddConnPerIsle = System.Math.Min(AddConnPerIsle, 4);

        IsleAbstract isle;

        int connectionsToAdd = (int)System.Math.Floor(connections.Count * AddConnPerIsle);

        for (int i = 0; i < connectionsToAdd; i++)
        {
            int tmp = rnd.Next(0, isles.Count);

            isle = isles[tmp];

            List<int> directions = new List<int>();

            for (int j = 0; j < 6; j++)
            {
                directions.Add(j);
            }

            for (int j = 0; j < 6; j++)
            {
                tmp = rnd.Next(0, directions.Count);
                int direction = directions[tmp];
                directions.RemoveAt(tmp);

                IsleAbstract islePartner = null;

                if (isle.Portals[direction] != null)
                {
                    continue;
                }

                islePartner = travelInDirection((int)isle.Index.x, (int)isle.Index.y, direction);

                int directionFrom = (direction + 3) % 6;

                if (islePartner != null)
                {
                    connectIsles(isle, islePartner, directionFrom);
                    break;
                }

            }

        }
    }

    public void inserObjectsOnMap()
    {
        List<IsleAbstract> tmpList = new List<IsleAbstract>(this.isles);

        // start Isle
        int tmp = rnd.Next(0, tmpList.Count);
        startIsle = tmpList[tmp];
        startIsle.isleObjectType = IsleAbstract.IsleObjectType.start;
        tmpList.RemoveAt(tmp);

        // keys
        for (int i = 0; i < 3; i++)
        {
            tmp = rnd.Next(0, tmpList.Count);
            tmpList[tmp].isleObjectType = IsleAbstract.IsleObjectType.key;
            tmpList[tmp].keyNumber = i + 1;
            tmpList.RemoveAt(tmp);
        }

        // boss
        tmp = rnd.Next(0, tmpList.Count);
        tmpList[tmp].isleObjectType = IsleAbstract.IsleObjectType.boss;
        bossIsle = tmpList[tmp];
        tmpList.RemoveAt(tmp);

    }

    public List<IsleAbstract> getAllNeighbours()
    {
        List<IsleAbstract> tmpList = new List<IsleAbstract>();

        for(int i = 0; i < 6; i++)
        {
            if (currentIsle.Portals[i] != null)
            {
                tmpList.Add(currentIsle.Portals[i].ConnectedPortal.isleAbstract);
            }
        }

        return tmpList;
    }

    public IsleAbstract getRandomIsle()
    {
        IsleAbstract isle = null;

        int tmp = rnd.Next(0, isles.Count);

        isle = isles[tmp];

        return isle;
    }

}
