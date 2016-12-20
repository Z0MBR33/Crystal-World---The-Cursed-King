using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_Canvas : MonoBehaviour {

    public GameObject MiniMap;
    public Text LiveField;
    public UI_Isle IsleImage;
    public UI_Connection ConnectionImage;

    public int Fieldwidth;

    private LevelManager levelManager;

    private List<UI_Isle> listIsles;
    private List<UI_Connection> listConnections;

    public void ShowMiniMap()
    {
        if (listIsles != null)
        {
            for(int i = 0; i < listIsles.Count; i++)
            {
                Destroy(listIsles[i].gameObject);
            }
            listIsles.Clear();
        }

        if (listConnections != null)
        {
            for (int i = 0; i < listConnections.Count; i++)
            {
                Destroy(listConnections[i].gameObject);
            }
            listConnections.Clear();
        }

        // draw isles

        listIsles = new List<UI_Isle>();
        listConnections = new List<UI_Connection>();

        levelManager = LevelManager.getLevelManager();
        IsleAbstract[,] world = levelManager.getWorld();

        int mapWidth = world.GetLength(0) * Fieldwidth;
        int mapHeight = world.GetLength(1) * Fieldwidth;

        for (int x = 0; x < world.GetLength(0); x++)
        {
            for(int y = 0; y <world.GetLength(1); y++)
            {
                if (world[x, y] == null)
                {
                    continue;
                }

               IsleAbstract isle = world[x, y];

                int offset = 0;
                if (x % 2 == 1)
                {
                    offset = Fieldwidth / 2;
                }

                Vector3 pos = new Vector3(isle.Index.x * Fieldwidth, (isle.Index.y * Fieldwidth) + offset, -1);
                pos = pos - new Vector3(mapWidth, mapHeight, 0);

                UI_Isle ui_Isle = Instantiate(IsleImage);
                ui_Isle.GetComponent<RawImage>().texture = ui_Isle.Normal;

                isle.setUIIsle(ui_Isle);
                ui_Isle.setIsle(isle);

                ui_Isle.transform.SetParent(MiniMap.transform, false);
                ui_Isle.transform.position = ui_Isle.transform.position + pos;

                listIsles.Add(ui_Isle);

            }
        }

        // draw connections

        List<ConnectionAbstract> connections = levelManager.getConnections();
        for (int i = 0; i < connections.Count; i++)
        {
            float x1 = connections[i].Portal1.isleAbstract.Index.x;
            float y1 = connections[i].Portal1.isleAbstract.Index.y;

            int offset = 0;
            if (x1 % 2 == 1)
            {
                offset = Fieldwidth / 2;
            }

            Vector3 posIsle1 = new Vector3(x1 * Fieldwidth, (y1 * Fieldwidth) + offset, 0);
            posIsle1 = posIsle1 - new Vector3(mapWidth, mapHeight, 0);

            float x2 = connections[i].Portal2.isleAbstract.Index.x;
            float y2 = connections[i].Portal2.isleAbstract.Index.y;

            offset = 0;
            if (x2 % 2 == 1)
            {
                offset = Fieldwidth / 2;
            }

            Vector3 posIsle2 = new Vector3(x2 * Fieldwidth, (y2 * Fieldwidth) + offset, 0);
            posIsle2 = posIsle2 - new Vector3(mapWidth, mapHeight, 0);

            Vector3 distanceVec = posIsle2 - posIsle1;
            float distanceLength = distanceVec.magnitude;

            float angle = Mathf.Atan2(distanceVec.x, distanceVec.y) * Mathf.Rad2Deg;
            angle = 360 - angle;

            UI_Connection ui_Conn = Instantiate(ConnectionImage);
            ui_Conn.transform.SetParent(MiniMap.transform, false);
            ui_Conn.transform.position = ui_Conn.transform.position + posIsle1;
            ui_Conn.GetComponent<RectTransform>().sizeDelta = new Vector2(4, distanceLength);
            ui_Conn.transform.Rotate(0f, 0f, angle);
            ui_Conn.transform.SetAsFirstSibling();

            listConnections.Add(ui_Conn);
        }

        UpdateMiniMap();

    }

    public void UpdateMiniMap()
    {

        List<IsleAbstract> list = levelManager.getAllNeighbours();

        list.Add(levelManager.getCurrentIsle());

        UI_Isle isle;

        for(int i = 0; i < list.Count; i++)
        {
            isle = list[i].ui_Isle;

            // Todo: Set active in GameMaster or Player (or somewhere else)
            isle.getIsleAbstract().discovered = true;

            // draw background of isle
            if (isle.getIsleAbstract() == levelManager.getCurrentIsle())
            {
                isle.GetComponent<RawImage>().texture = isle.Current;
            }
            else if (isle.getIsleAbstract().getFinishState() == true)
            {
                isle.GetComponent<RawImage>().texture = isle.Finished;
            }   
            else if (isle.getIsleAbstract().discovered == true)
            {
                isle.GetComponent<RawImage>().texture = isle.Discovered;
            }
            else
            {
                isle.GetComponent<RawImage>().texture = isle.Normal;
            }

            // draw icon
            if (isle.getIsleAbstract().discovered == true)
            {
                if (isle.getIsleAbstract().isleObjectType == IsleAbstract.IsleObjectType.boss)
                {
                    isle.setIcon(isle.IconBoss);
                }
                else if (isle.getIsleAbstract().isleObjectType == IsleAbstract.IsleObjectType.key)
                {
                    isle.setIcon(isle.IconKey);
                }
            }
        }

    }

    public void UpdateLive(float live)
    {
        //LiveField.text = live.ToString();  // TODO
    }
}
