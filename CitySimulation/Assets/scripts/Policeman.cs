using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Policeman : MonoBehaviour {
    private int counter;
    public NavMeshAgent navAgent;
    private GameObject[] potentialTargets;
    private int switchSpeed;
    private bool YaUnMalandrin;
    private GameObject LeMalotru;

    private bool blue;
    // Start is called before the first frame update
    void Start() {
        counter = 0;
        blue = true;
        potentialTargets = GameObject.FindGameObjectsWithTag("robber");
        switchSpeed = 50;
        YaUnMalandrin = false;
        Move();
    }

    // Update is called once per frame
    void Update() {
        counter++;
        if (counter % switchSpeed == 0) {
            SwitchColor();
        }

        if (navAgent.remainingDistance < 0.05) {
            if (YaUnMalandrin) {
                Destroy(LeMalotru);
                YaUnMalandrin = false;
            }
            else {
                Move();
            }
        }
        
        GameObject nearest = Nearest();
        if (nearest != null && Vector3.Distance(nearest.transform.position, transform.position) < 50) {
            switchSpeed = 15;
            navAgent.SetDestination(nearest.transform.position);
            Debug.Log("Police ! Freeeze !");
            YaUnMalandrin = true;
            LeMalotru = nearest;
        }
        else {
            
        }
    }
    
    GameObject Nearest() {
        potentialTargets = GameObject.FindGameObjectsWithTag("robber");
        GameObject nearest;
        float dist = 0;
        
        if (potentialTargets.Length == 0) {
            return null;
        }
        
        nearest = potentialTargets[0];
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
        Vector3 point = GetRandomPoint(transform.position, 300f);
        navAgent.SetDestination(point);
    }

    private void SwitchColor() {
        if (blue)
            GetComponent<Renderer>().material.color = Color.red;
        else
            GetComponent<Renderer>().material.color = Color.blue;
        
        blue = !blue;
    }
}
