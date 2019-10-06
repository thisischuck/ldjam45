using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class GameScriptableObject : ScriptableObject
{
    public bool isKnockedDown;
    public bool restarted;

    public int Fame;

    public int count = 10;
    public int MainCount = 10;


    public int enemiesKilled;

    public int keyPresses;
}
