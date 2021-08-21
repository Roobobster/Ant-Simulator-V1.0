using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform antPrefab; 

    [SerializeField]
    private int spawnCount;

    [SerializeField]
    private float duration;

    [SerializeField]
    private float spawnDelay;   

    private float timeTillNextSpawn;

    private Vector3 colonyPosition;


    // Start is called before the first frame update
    void Start()
    {
        colonyPosition = GetComponent<Transform>().position;
        timeTillNextSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
       HandleSpawning();
        
    }

    private void HandleSpawning(){  
        if(duration > 0){
            timeTillNextSpawn -= Time.deltaTime;
            duration -= Time.deltaTime;
            if(timeTillNextSpawn <= 0 ){
                SpawnAnts();
                timeTillNextSpawn = spawnDelay;
            }
        }
    }

    private void SpawnAnts()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Transform ant = Instantiate(antPrefab, colonyPosition, Quaternion.identity);
            ant.SetParent(this.transform);
            AntController antControllerScript = ant.GetComponent<AntController>();
            antControllerScript.setHome(this.transform.position);
            
        }
    }
}
