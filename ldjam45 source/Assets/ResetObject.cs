using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObject : MonoBehaviour
{
    public GameScriptableObject g;
    void Start()
    {
        g.isKnockedDown = false;
        g.restarted = false;
        g.count = 10;
        g.MainCount = 10;
        g.Fame = 0;
        g.enemiesKilled = 0;
        g.keyPresses = 0;
    }
}
