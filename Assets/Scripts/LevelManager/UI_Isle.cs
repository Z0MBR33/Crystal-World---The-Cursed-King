using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Isle : MonoBehaviour {

    public Texture Normal;
    public Texture Current;
    public Texture Finished;

    private IsleAbstract isleAbstract;

    public IsleAbstract getIsleAbstract()
    {
        return isleAbstract;
    }

    public void setIsle(IsleAbstract isle)
    {
        isleAbstract = isle;
    }

}
