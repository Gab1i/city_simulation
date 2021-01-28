using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour {
    public Car Selected;
    public Worker Selected2;
    public Camera cam;
    public float camSpeed;
    public float rotSpeed;
    
    // Start is called before the first frame update
    void Start() {
        camSpeed = 5;
        rotSpeed = 5;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0))
            Clicked();
        if (Input.GetKey("q")) {
            Vector3 p = cam.transform.rotation.eulerAngles;
            p.y -= rotSpeed;
            cam.transform.rotation = Quaternion.Euler(p);
        }
        if (Input.GetKey("d")) {
            Vector3 p = cam.transform.rotation.eulerAngles;
            p.y += rotSpeed;
            cam.transform.rotation = Quaternion.Euler(p);
        }
        if (Input.GetKey("o")) {
            Vector3 p = cam.transform.rotation.eulerAngles;
            p.x -= rotSpeed;
            cam.transform.rotation = Quaternion.Euler(p);
        }
        if (Input.GetKey("l")) {
            Vector3 p = cam.transform.rotation.eulerAngles;
            p.x += rotSpeed;
            cam.transform.rotation = Quaternion.Euler(p);
        }
        if (Input.GetKey("z")) {
            Vector3 p = cam.transform.position;
            p.z += camSpeed;
            cam.transform.position = p;
        }
        if (Input.GetKey("s")) {
            Vector3 p = cam.transform.position;
            p.z -= camSpeed;
            cam.transform.position = p;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            Vector3 p = cam.transform.position;
            p.y += camSpeed;
            cam.transform.position = p;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            Vector3 p = cam.transform.position;
            p.y -= camSpeed;
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