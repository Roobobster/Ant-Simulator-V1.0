using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalDisperse : MonoBehaviour
{
    [SerializeField]
    private float timeTillDisperse;

    private float timePassed;
    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timeTillDisperse < timePassed) {
            Destroy(gameObject);
        }
    }
}
