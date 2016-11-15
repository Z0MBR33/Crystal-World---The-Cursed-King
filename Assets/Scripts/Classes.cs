using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// DOTween

public class Classes : MonoBehaviour {

    public enum ShotMode { Bomb, Rocket };

    public GameObject HauptPanel;

    public void HalloWelt()
    {
        HauptPanel.SetActive(false);
    }

}
