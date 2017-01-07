using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Isle : MonoBehaviour {

    public Texture Normal;
    public Texture Discovered;
    public Texture Current;
    public Texture Finished;

    public Texture IconBoss;
    public Texture IconKey;

    private UI_Isle Icon;

    [HideInInspector]
    public IsleAbstract isleAbstract;

    public void setIcon(Texture icon)
    {
        deleteIcon();

        GameObject obj = Instantiate(gameObject);
        obj.transform.SetParent(gameObject.transform);
        obj.transform.position = gameObject.transform.position;
        obj.GetComponent<RawImage>().texture = icon;
        Icon = obj.GetComponent<UI_Isle>();
    }

    public void deleteIcon()
    {
        if (Icon != null) Destroy(Icon.gameObject);
        Icon = null;
    }

}
