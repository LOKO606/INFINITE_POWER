using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontWarp : MonoBehaviour
{
    private WorldManager worldManager;
    void Start()
    {
        worldManager = FindObjectOfType<WorldManager>();
        worldManager.canWarp = false;
    }
}
