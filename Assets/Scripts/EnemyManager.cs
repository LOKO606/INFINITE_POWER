using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject playerObject;
    public Transform flyenemiesTransformParentOfAllSpawns;
    private Transform[] flySpawnLocations;
    public GameObject flyEnemy;

    void Start()
    {
        flySpawnLocations = flyenemiesTransformParentOfAllSpawns.GetComponentsInChildren<Transform>();
        flySpawnLocations[0] = null;
    }

    public void CreateEnemies()
    {
        for (int i = 1; i < flySpawnLocations.Length; i++)
        {
            GameObject obj = Instantiate(flyEnemy, flySpawnLocations[i].position, Quaternion.Euler(-90, 180, 0));// makes a little animation of flying enemies pointing up then to player
            obj.GetComponent<FlyingEnemy>().target = playerObject.transform;
        }
    }

    public void DestroyEnemies()
    {
        FlyingEnemy[] obj = FindObjectsOfType<FlyingEnemy>();

        if (obj.Length > 0)
        {
            for (int i = 0; i < obj.Length; i++)
            {
                Destroy(obj[i].gameObject);
            }
        }
    }
}
