using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

    public class Inhabitant {
        public GameObject movingAgent;
        public GameObject home;
        public GameObject office;
        public int startWork;
        public int endWork;
        public bool atWork;
        public bool atHome;

        public bool moving;
        private GameObject HouseLight;
        private GameObject OfficeLight;

        public Inhabitant(GameObject home, GameObject office) {
            startWork = Random.Range(5, 10);
            endWork = Random.Range(13, 24);
            this.home = home;
            this.office = office;
            moving = false;
            
            atHome = true;
            atWork = false;

            HouseLight = home.GetComponent<Home>().GetLight();
            HouseLight.GetComponent<Light>().enabled = true;
            OfficeLight = office.GetComponent<Office>().GetLight();
        }

        public void NotVersailleHere(bool work=true) {
            if (atWork)
                OfficeLight.GetComponent<Light>().enabled = false;
            else
                HouseLight.GetComponent<Light>().enabled = false;
        }
        
        
        
        public void FiatLux(bool work=true) {
            if (atWork)
                OfficeLight.GetComponent<Light>().enabled = true;
            else
                HouseLight.GetComponent<Light>().enabled = true;
        }

        public bool Arrived() {
            Car c = movingAgent.GetComponent<Car>();
            if (c != null) {
                return c.Arrived;
            }
            
            /*Worker w = movingAgent.GetComponent<Worker>();
            if (w != null) {
                return w.Arrived();
            }*/

            return false;
        }

        public void OnDestroyListener(MonoBehaviour instance) {
            Vector3 t;
            if (atHome) t = office.transform.position;
            else t = home.transform.position;
            
            float dist = Vector3.Distance(movingAgent.transform.position, t);
            if (dist < 1) {
                if (atHome) {
                    atHome = false;
                    atWork = true;
                    home.GetComponent<Home>().WhosAtHome--;
                }
                else {
                    atHome = true;
                    atWork = false;
                    home.GetComponent<Home>().WhosAtHome++;
                }
            
                moving = false;
                FiatLux();
                MonoBehaviour.Destroy(movingAgent);
            }
        }
    }

