using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    public enum SpawnMode
    {
        LongStream,
        StutteredBurst,
        BossSolo,
        BossStream
    }

    public SpawnMode spawnMode;
    public float dedicatedPower;
    public float startDelayTime;

    public Wave(int spawnModeSelect, float dedicatedPowerSet, float startDelayTimeSet) 
    {
        spawnMode = (SpawnMode)spawnModeSelect;
        dedicatedPower = dedicatedPowerSet;
        startDelayTime = startDelayTimeSet;
    }

    public Wave(SpawnMode spawnModeSelect, float dedicatedPowerSet, float startDelayTimeSet)
    {
        spawnMode = spawnModeSelect;
        dedicatedPower = dedicatedPowerSet;
        startDelayTime = startDelayTimeSet;
    }
}
