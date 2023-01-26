using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattery : MonoBehaviour
{
    public WorldManager worldManager;
    public Battery battery;
    bool done = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !done)
        {
            done = true;
            battery.chargeRate = -0.0015f;
            worldManager.batteryDecayRate = -0.0015f;
        }
    }
}
