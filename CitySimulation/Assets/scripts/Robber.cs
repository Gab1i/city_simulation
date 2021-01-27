using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robber : MonoBehaviour {
    public NavMeshAgent navAgent;
    private GameObject[] potentialTargets;
    private bool stealing;
    private GameObject currentHouseToSteal;
    
    // Start is called before the first frame update
    void Start() {
        potentialTargets = GameObject.FindGameObjectsWithTag("house");
        stealing = false;
        Move();
    }

    // Update is called once per frame
    void Update() {
        if (navAgent.remainingDistance < 0.05) {
            if (stealing) {
                currentHouseToSteal.GetComponent<Home>().broken = true;
                var r = currentHouseToSteal.GetComponentsInChildren<Renderer>();
                r[0].material.color = Color.yellow;
                
                Debug.Log("On vole cette maison !");
                stealing = false;
            }
            
            Move();
        }

        GameObject nearest = Nearest();
        if (nearest.GetComponent<Home>().WhosAtHome == 0 && !nearest.GetComponent<Home>().broken) {
            LetsSteal(nearest);
        }
    }

    void LetsSteal(GameObject house) {
        navAgent.SetDestination(house.transform.position);
        stealing = true;
        currentHouseToSteal = house;
    }

    GameObject Nearest() {
        float dist = Vector3.Distance(potentialTargets[0].transform.position, transform.position);
        GameObject nearest = potentialTargets[0];
        
        foreach (GameObject target in potentialTargets) {
            float d = Vector3.Distance(target.transform.position, transform.position);
            if (d < dist) {
                dist = d;
                nearest = target;
            }
        }

        return nearest;
    }
    
    public static Vector3 GetRandomPoint(Vector3 center, float maxDistance) {
        Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);

        return hit.position;
    }

    private void Move() {
        Vector3 point = GetRandomPoint(transform.position, 100f);
        navAgent.SetDestination(point);
    }
    
    private void OnDestroy() {
        Debug.Log("Oh nooon, j'ai été arrêté !");
        if(this.OnDestroyEvnt != null) this.OnDestroyEvnt(this);
    }

 
    public event OnDestroyDelegate OnDestroyEvnt;
    public delegate void OnDestroyDelegate(MonoBehaviour instance);
}
