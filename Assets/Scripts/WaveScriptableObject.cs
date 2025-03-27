using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave", order = 2)]
public class WaveScriptableObject : ScriptableObject
{
    public float waveWaitTime;
    public int enemyCount;
    public int waveIndex;
    public Enemy enemyTypePrefab;
}
