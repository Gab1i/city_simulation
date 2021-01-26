using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Home : MonoBehaviour {
    public int nInhabitants;
    public List<GameObject> lights;
    public GameObject prefab_light;
    public bool FlatHouse;

    public int inhabitants;

    void Awake() {
        SetUpLights(nInhabitants);
    }
    
    // Start is called before the first frame update
    void Start() {
        inhabitants = 0;
        //SetUpLights(nInhabitants);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public GameObject GetLight() {
        int idx = Random.Range(0, lights.Count-1);
        GameObject l = lights[idx];
        lights.Remove(l);
        return l;
    }

    void SetUpLights(int nb) {
        lights = new List<GameObject>();
        if (FlatHouse) {
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    GameObject light = Instantiate(prefab_light, transform);
                    Vector3 p = new Vector3( i, light.transform.localPosition.y, j);
                    light.transform.localPosition = p;
                    lights.Add(light);
                }
            }
        }
        else {
            for (int i = 0; i < 6; i++) {
                for (int j = 0; j < 12; j++) {
                    GameObject light = Instantiate(prefab_light, transform);
                    Vector3 p = new Vector3( light.transform.localPosition.x, j, i);
                    light.transform.localPosition = p;
                    lights.Add(light);
                }
            }
        }
    }
}
