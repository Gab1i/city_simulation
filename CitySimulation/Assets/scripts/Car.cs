using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
   
    public float movementSpeed;
    public UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private bool isOnTheWay;

    public bool Arrived;

    public void Awake() {
        //isOnTheWay = false;
        Arrived = false;
    }

    // Start is called before the first frame update
    void Start() {
        movementSpeed = 1.2f;
        _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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