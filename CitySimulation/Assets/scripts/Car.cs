using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car : MonoBehaviour
{
   
    public float movementSpeed;
    public UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private bool isOnTheWay;

    public bool Arrived;

    public void Awake() {
        //isOnTheWay = false;
        _navMeshAgent.speed = 5;
        Arrived = false;
    }

    // Start is called before the first frame update
    void Start() {
        movementSpeed = 1.2f;
        _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
    
    int GetClosestRoad() {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity)) {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            if (hit.transform.gameObject.name == "roadHL" || hit.transform.gameObject.name == "roadHR") {
                if(_navMeshAgent.speed == 5) _navMeshAgent.speed = 10;
            }
            else {
                if(_navMeshAgent.speed == 10) _navMeshAgent.speed = 5;
            }
        }

        return 0;
    }
    
    // Update is called once per frame
    void Update() {
        if (!EqualityComparer<bool>.Default.Equals(isOnTheWay,default) && _navMeshAgent.remainingDistance < 0.5) {
            Arrived = true;
            this.OnDestroyEvnt(this);
            //Destroy(gameObject);
        }
        else {
            Arrived = false;
        }

        GetClosestRoad();

    }
    
    private void OnDestroy() {
        if(this.OnDestroyEvnt != null) this.OnDestroyEvnt(this);
    }

 
    public event OnDestroyDelegate OnDestroyEvnt;
    public delegate void OnDestroyDelegate(MonoBehaviour instance);

    public void Move(Vector3 coord) {
        _navMeshAgent.isStopped = false;
        isOnTheWay = true;
        _navMeshAgent.SetDestination(coord);
    }
}