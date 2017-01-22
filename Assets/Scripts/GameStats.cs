using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameStats {

    public static bool LoadLevelSettings = false;

    public static int Level;

    public static int LvlWorldWidht;
    public static int LvlWorldHeight;
    public static float LvlIsleDensity;

    //Char stats
    public static bool LoadCharStats = false;

    public static float maxHealth;
    public static float health;
    public static float speed;
    public static float luck;
    public static float strength;

    //Shoot stats
    public static Vector3 shootOffset = new Vector3(0, 1, 0);
    public static float StartVerticalDegree = 45;
    public static float shotSpeed = 1.0f;
    public static float shotStrength = 5.0f;
    public static float fireRate = 0.1f;
    public static float fireRateDifference = 0;
    public static List<ShotEffect> possibleShotEffects;

    public static void UpdateLevelSettings(int level)
    {
        Level = level;

        switch(level)
        {
            case 1: LvlWorldWidht = 3;
                    LvlWorldHeight = 3;
                    LvlIsleDensity = 1;
                    break;
            case 2: LvlWorldWidht = 4;
                    LvlWorldHeight = 4;
                    LvlIsleDensity = 0.5f;
                    break;
            case 3: LvlWorldWidht = 5;
                    LvlWorldHeight = 5;
                    LvlIsleDensity = 0.5f;
                    break;
        }
    }
}
