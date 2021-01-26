using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public float movementSpeed;
    public UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private bool isOnTheWay;
    
    public bool Arrived() {
        return !EqualityComparer<bool>.Default.Equals(isOnTheWay, default) && _navMeshAgent.remainingDistance < 0.5;
    }

    // Start is called before the first frame update
    void Start() {
        movementSpeed = 1.2f;
        _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
    
    // Update is called once per frame
    void Update() {
        if (!EqualityComparer<bool>.Default.Equals(isOnTheWay,default) && _navMeshAgent.remainingDistance < 0.5) {
            //Destroy(gameObject);
            this.OnDestroyEvnt(this);
        }
    }
    
    private void OnDestroy() {
        if(this.OnDestroyEvnt != null) this.OnDestroyEvnt(this);
    }
 
    public event OnDestroyDelegate OnDestroyEvnt;
    public delegate void OnDestroyDelegate(MonoBehaviour instance);

    public void Move(Vector3 coord) {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(coord);
        isOnTheWay = true;
    }
}
