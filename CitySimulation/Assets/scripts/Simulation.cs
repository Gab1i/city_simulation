using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Simulation : MonoBehaviour {
    /* ==========   UI   ========== */
    public Text labelDay;
    public Text labelTime;
    public Text labelPopulation;
    public Text labelEmployment;
    private List<Inhabitant> population;

    public GameObject prefab_car;
    public GameObject prefab_john;
    public GameObject what_a_point;
    public GameObject sun;
    public GameObject police;
    public GameObject robber;

    private int thievesStop;
    private int currentThievesNb;
    
    private int TotalTime;
    public int deltaHours;

    public GameObject city;
    private static System.Random _rnd;

    void Start() {
        _rnd = new System.Random();
        TotalTime = 0;
        deltaHours = 240;

        TotalTime = 5 * deltaHours;

        population = new List<Inhabitant>();

        thievesStop = 0;
        currentThievesNb = 0;

        assignHouseAndOffice();
        Instantiate(police, population[0].office.transform.position, Quaternion.identity);
        Instantiate(police, population[0].office.transform.position, Quaternion.identity);
    }
    
    void Update() {
        TotalTime++;
        
        int Day = (int) (TotalTime/deltaHours) / 24;
        int Time = (int) (TotalTime/deltaHours) % 24;
        labelDay.text = Day.ToString();
        labelTime.text = Time.ToString();
        labelEmployment.text = city.GetComponent<Roads>().works.ToString();
        labelPopulation.text = city.GetComponent<Roads>().population.ToString();

        CheckMoves(Time);
        
        // sun
        sun.transform.localRotation = Quaternion.Euler(Time, -30, 0);

        if (Random.Range(0, 1000) > 995 && currentThievesNb <= 5) {
            GameObject mechant = Instantiate(robber, population[2].office.transform.position, Quaternion.identity);
            mechant.GetComponent<Robber>().OnDestroyEvnt += OnDestroyListener;
            currentThievesNb++;
        }
    }

    public void OnDestroyListener(MonoBehaviour instance) {
        currentThievesNb--;
    }

    void CheckMoves(int time) {
        foreach (var john in population) {
            
            if (john.startWork == time && !john.moving && john.atHome) {
                john.moving = true;

                Vector3 homePosition = john.home.transform.position;
                Vector3 officePosition = john.office.transform.position;
                officePosition.y = 1.0f;
                //Debug.Log("home: " + homePosition.ToString() + " | office: " +officePosition.ToString());
                
                
                if (_rnd.Next(10) > 8) {
                    GameObject car = Instantiate(prefab_john, new Vector3(homePosition.x, 1, homePosition.z),
                        Quaternion.identity);
                    car.GetComponent<Worker>().Move(officePosition);
                    john.movingAgent = car;
                    
                    john.movingAgent.GetComponent<Worker>().OnDestroyEvnt += john.OnDestroyListener;
                }
                else {
                    GameObject car = Instantiate(prefab_car, new Vector3(homePosition.x, 1, homePosition.z),
                        Quaternion.identity);
                    car.GetComponent<Car>().Move(officePosition);
                    john.movingAgent = car;
                    
                    john.movingAgent.GetComponent<Car>().OnDestroyEvnt += john.OnDestroyListener;
                }

                john.NotVersailleHere();
            }
            
            if (john.endWork == time && !john.moving && john.atWork) {
                john.moving = true;

                Vector3 homePosition = john.home.transform.position;
                Vector3 officePosition = john.office.transform.position;
                //Debug.Log("home: " + homePosition.ToString() + " | office: " +officePosition.ToString());
                
                
                if (_rnd.Next(10) > 8) {
                    GameObject car = Instantiate(prefab_john, new Vector3(officePosition.x, 1, officePosition.z),
                        Quaternion.identity);
                    car.GetComponent<Worker>().Move(homePosition);
                    john.movingAgent = car;
                    
                    john.movingAgent.GetComponent<Worker>().OnDestroyEvnt += john.OnDestroyListener;
                }
                else {
                    GameObject car = Instantiate(prefab_car, new Vector3(officePosition.x, 1, officePosition.z),
                        Quaternion.identity);
                    car.GetComponent<Car>().Move(homePosition);
                    john.movingAgent = car;
                    
                    john.movingAgent.GetComponent<Car>().OnDestroyEvnt += john.OnDestroyListener;
                }

                john.NotVersailleHere();
            }

            /*if (john.moving) {
                if (john.Arrived()) {
                    //john.movingAgent.GetComponent<Material>().color = Color.green;
                    Debug.Log("Arrived");
                    //john.FiatLux(true);
                    //Destroy(john.movingAgent);
                    //john.moving = false;
                }
            } */
        }
    }
    
    void assignHouseAndOffice() {
        List<GameObject> offices = new List<GameObject>(city.GetComponent<Roads>().offices);
        List<GameObject> houses = new List<GameObject>(city.GetComponent<Roads>().houses);

        for (int i = 0; i < city.GetComponent<Roads>().population; i++) {
            int idxOffice = Random.Range(0, offices.Count-1);
            int idxHome = Random.Range(0, houses.Count-1);
            
            GameObject johnsHome = houses[idxHome];
            GameObject johnsWork = offices[idxOffice];

            Inhabitant johnDo = new Inhabitant(johnsHome, johnsWork);
            population.Add(johnDo);

            johnsHome.GetComponent<Home>().inhabitants++;
            johnsHome.GetComponent<Home>().WhosAtHome++;
            johnsWork.GetComponent<Office>().workers++;

            if (johnsHome.GetComponent<Home>().inhabitants >= johnsHome.GetComponent<Home>().nInhabitants) {
                houses.Remove(johnsHome);
            }

            if (johnsWork.GetComponent<Office>().workers >= johnsWork.GetComponent<Office>().nbWorkers) {
                offices.Remove(johnsWork);
            }
        }
    }
}
