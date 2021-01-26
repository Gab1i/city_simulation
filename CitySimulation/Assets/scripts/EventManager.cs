using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour {
    public Car Selected;
    public Worker Selected2;
    public Camera cam;
    
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0))
            Clicked();
        if (Input.GetKey("q")) {
            Vector3 p = cam.transform.position;
            p.x -= 10;
            cam.transform.position = p;
        }
        if (Input.GetKey("d")) {
            Vector3 p = cam.transform.position;
            p.x += 10;
            cam.transform.position = p;
        }
        if (Input.GetKey("z")) {
            Vector3 p = cam.transform.position;
            p.z += 10;
            cam.transform.position = p;
        }
        if (Input.GetKey("s")) {
            Vector3 p = cam.transform.position;
            p.z -= 10;
            cam.transform.position = p;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            Vector3 p = cam.transform.position;
            p.y += 10;
            cam.transform.position = p;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            Vector3 p = cam.transform.position;
            p.y -= 10;
            cam.transform.position = p;
        }
  
            
    }

    void MouseMove() {
        
    }
    
    void Clicked() {
        Debug.Log("Click");
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
 
        RaycastHit hit = new RaycastHit();
 
        if (Physics.Raycast (ray, out hit)) {
            Debug.Log(hit.collider.gameObject.name);
            
            
            /*if (hit.collider.gameObject.name == "roadR" || hit.collider.gameObject.name == "roadL") {
                Selected.Move(hit.point);
                Debug.Log("Move");
            }*/
            
            
            Car peon = (Car) hit.collider.gameObject.GetComponent(typeof(Car));
            if(peon != null) {
                Selected = peon;
                //Debug.Log("Supposed to go to... " + Selected.Target + " from " + hit.point);
                
            }

            if (peon == null) {
                Selected.Move(hit.point);
                Debug.Log("Move to " + hit.point);
            }
       
        }
    }
}