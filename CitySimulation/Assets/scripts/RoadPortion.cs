using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPortion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        //Put this above all the other code so that you know it's getting called correctly.
 
        Debug.Log(other.gameObject.name);
 
    }
}
