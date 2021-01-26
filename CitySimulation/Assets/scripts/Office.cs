using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Office : MonoBehaviour {
    public int nbWorkers;
    public int workers;
    public Vector3 position;
    public List<GameObject> lights;
    public GameObject prefab_light;
    
    void Awake() {
        SetUpLights();
    }
    
    // Start is called before the first frame update
    void Start() {
        nbWorkers = 120;
        workers = 0;
    }
    
    public GameObject GetLight() {
        int idx = Random.Range(0, lights.Count-1);
        GameObject l = lights[idx];
        lights.Remove(l);
        return l;
    }

    // Update is called once per frame
    void Update() {
        
    }

    void SetUpLights() {
        lights = new List<GameObject>();
        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < 12; j++) {
                GameObject light = Instantiate(prefab_light, transform);
                Vector3 p = new Vector3(light.transform.localPosition.x, j, i);
                light.transform.localPosition = p;
                light.GetComponent<Light>().enabled = false;
                lights.Add(light);
            }
        }
    }
}
