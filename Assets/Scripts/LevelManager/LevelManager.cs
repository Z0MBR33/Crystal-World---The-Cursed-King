using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{

    public static LevelManager levelManager;

    public Isle IslePrefab;
    public Connection connectionPrefab;

    private IsleAbstract[,] world;
    private List<IsleAbstract> isles;
    private List<ConnectionAbstract> connections;

    public int WorldWidth;
    public int WorldHeight;

    public float IsleDensity;

    public int Fieldwidth;

    public float AddConnPerIsle;

    private List<Isle> islesObjects;
    private List<Connection> connectionObjects;

    private IsleAbstract currentIsle;

    private System.Random rnd;

    public static LevelManager getLevelManager()
    {
        return levelManager;
    }

    void Awake()
    {
        levelManager = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GenerateMap();

            ObjectPool.getObjectPool().getUI().GetComponent<UI_Canvas>().ShowMiniMap();
            /*int sum = 0;
            for (int i = 0; i < isles.Count; i++)
            {
                sum += isles[i].getConnectionCount();
            }
            float av = (float)sum / isles.Count;
            print(av);*/
        }
    }

    public void GenerateMap()
    {
        rnd = new System.Random();

        if (islesObjects != null)
        {
            for (int i = 0; i < islesObjects.Count; i++)
            {
                Destroy(islesObjects[i].gameObject);
            }
            islesObjects.Clear();
        }

        if (connectionObjects != null)
        {
            for (int i = 0; i < connectionObjects.Count; i++)
            {
                Destroy(connectionObjects[i].gameObject);
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
                Isle isleObj = Instantiate(IslePrefab, pos, new Quaternion()) as Isle;
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
            Connection connectionObj = Instantiate(connectionPrefab);
            connectionObj.connectionAbstract = connections[i];
            connections[i].connectionObj = connectionObj;

            LineRenderer lineRenderer = connectionObj.GetComponent<LineRenderer>();
            lineRenderer.SetVertexCount(2);
            lineRenderer.SetPosition(0, connections[i].Isle1.IsleObj.transform.position);
            lineRenderer.SetPosition(1, connections[i].Isle2.IsleObj.transform.position);

            connectionObjects.Add(connectionObj);
        }
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
        ConnectionAbstract connection = new ConnectionAbstract();
        connection.Isle1 = isle1;
        connection.Isle2 = isle2;
        connections.Add(connection);

        switch (directionFrom)
        {
            case 0:
                isle1.ConnectionDown = connection;
                isle2.ConnectionUp = connection;
                break;
            case 1:
                isle1.ConnectionDownLeft = connection;
                isle2.ConnectionUpRight = connection;
                break;
            case 2:
                isle1.ConnectionUpLeft = connection;
                isle2.ConnectionDownRight = connection;
                break;
            case 3:
                isle1.ConnectionUp = connection;
                isle2.ConnectionDown = connection;
                break;
            case 4:
                isle1.ConnectionUpRight = connection;
                isle2.ConnectionDownLeft = connection;
                break;
            case 5:
                isle1.ConnectionDownRight = connection;
                isle2.ConnectionUpLeft = connection; ;
                break;
        }
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

                switch (direction)
                {
                    case 0:
                        if (isle.ConnectionUp != null) continue;
                        islePartner = travelUp((int)isle.Index.x, (int)isle.Index.y);
                        break;
                    case 1:
                        if (isle.ConnectionUpRight != null) continue;
                        islePartner = travelUpRight((int)isle.Index.x, (int)isle.Index.y);
                        break;
                    case 2:
                        if (isle.ConnectionDownRight != null) continue;
                        islePartner = travelDownRight((int)isle.Index.x, (int)isle.Index.y);
                        break;
                    case 3:
                        if (isle.ConnectionDown != null) continue;
                        islePartner = travelDown((int)isle.Index.x, (int)isle.Index.y);
                        break;
                    case 4:
                        if (isle.ConnectionDownLeft != null) continue;
                        islePartner = travelDownLeft((int)isle.Index.x, (int)isle.Index.y);
                        break;
                    case 5:
                        if (isle.ConnectionUpLeft != null) continue;
                        islePartner = travelUpLeft((int)isle.Index.x, (int)isle.Index.y);
                        break;
                }

                int directionFrom = (direction + 3) % 6;

                if (islePartner != null)
                {
                    connectIsles(isle, islePartner, directionFrom);
                    break;
                }

            }

        }
    }

    public IsleAbstract getRandomIsle()
    {
        IsleAbstract isle = null;

        int tmp = rnd.Next(0, isles.Count);

        isle = isles[tmp];

        return isle;
    }

    public void setCurrentIsle(IsleAbstract isle)
    {
        currentIsle = isle;
    }

    public IsleAbstract getCurrentIsle()
    {
        return currentIsle;
    }

    public IsleAbstract[,] getWorld()
    {
        return world;
    }

    public List<ConnectionAbstract> getConnections()
    {
        return connections;
    }
}
