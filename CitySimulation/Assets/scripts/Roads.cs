using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Delaunay;
using Delaunay.Geo;


public class Roads : MonoBehaviour {
    public NavMeshSurface roadSurface;
    public NavMeshSurface walkwaySurface;
    public GameObject prefab_road;
    public GameObject prefab_highway;
    public GameObject prefab_house;
    public GameObject prefab_building;
    public GameObject prefab_car;
    public GameObject prefab_walker;
    public GameObject prefab_office;
    public GameObject prefab_pedestrian_cross;
    public Material land;
    public Camera mainCam;
    public List<GameObject> habitations;
    private List<Vector2> m_points;
    public List<GameObject> offices;
    public List<GameObject> houses;
    
    const int WIDTH = 200;
    const int HEIGHT = 200;
    const int NPOINTS = 20;

    public int population = 0;
    public int works = 0;
    
    private List<LineSegment> m_edges = null;
    private List<LineSegment> m_spanningTree;
    private List<LineSegment> m_delaunayTriangulation;
    private Texture2D tx;
    public float freqx = 0.021f, freqy = 0.017f, offsetx = 0.43f, offsety = 0.22f;

    private void DrawRiver(Color[] pixels, float[,] map) {
        Vector2 p1;
        Vector2 p2;
        if (Random.Range(0, 10) > 5) {
            p1 = new Vector2(Random.Range(0, WIDTH), 0);
            p2 = new Vector2(Random.Range(0, WIDTH), HEIGHT-1);
        }
        else {
            p1 = new Vector2(0, Random.Range(0, HEIGHT));
            p2 = new Vector2(WIDTH - 1, Random.Range(0, HEIGHT));
        }
        
        DrawLine(pixels, map, p1, p2);
    }
    
    private float [,] createMap() {
        float [,] map = new float[WIDTH, HEIGHT];
        for (int i = 0; i < WIDTH; i++)
            for (int j = 0; j < HEIGHT; j++)
                map[i, j] = Mathf.PerlinNoise(freqx * i + offsetx, freqy * j + offsety);
        return map;
    }
    
    private Color[] createPixelMap(float[,] map) {
        Color[] pixels = new Color[WIDTH * HEIGHT];
        for (int i = 0; i < WIDTH; i++)
            for (int j = 0; j < HEIGHT; j++)
                pixels[i * WIDTH + j] = Color.Lerp(Color.white, Color.black, 1 - map[i, j]);
            
        return pixels;
    }
    
    private void DrawPoint (Color[] pixels, Vector2 p, Color c) {
        if (p.x < WIDTH && p.x >= 0 && p.y < HEIGHT && p.y >= 0)
            pixels[(int)p.y * WIDTH + (int)p.x] = c;
    }
    
    // Bresenham line algorithm
    private void DrawLine(Color[] pixels, float[,] map, Vector2 p0, Vector2 p1) {
        double val = 0;
        
        int x0 = (int)p0.x;
        int y0 = (int)p0.y;
        int x1 = (int)p1.x;
        int y1 = (int)p1.y;

        int dx = Mathf.Abs(x1-x0);
        int dy = Mathf.Abs(y1-y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx-dy;
        Color c;
        while (true) {
            if (x0 >= 0 && x0 < WIDTH && y0 >= 0 && y0 < HEIGHT) {
                c = map[x0, y0] < 0.7 ? Color.blue : Color.green;
                pixels[y0 * WIDTH + x0] = c;
            }

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2*err;
            if (e2 > -dy) {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx) {
                err += dx;
                y0 += sy;
            }
        }
    }
    
    // Start is called before the first frame update
    void Start() {
        float [,] map = createMap();
        Color[] pixels = createPixelMap(map);

        /* Create random points points */
        m_points = new List<Vector2> ();
        List<uint> colors = new List<uint> ();
        for (int i = 0; i < NPOINTS; i++) {
            int iter = 0;
            colors.Add((uint) 0);
            Vector2 vec;
            do {
                vec = new Vector2(Random.Range(0, WIDTH - 1), Random.Range(0, HEIGHT - 1));
            } while (map[(int) vec.x, (int) vec.y] < 0.73 && iter++ < 40);

            m_points.Add(vec);
        }
        
        /* Generate Graphs */
        Delaunay.Voronoi v = new Delaunay.Voronoi(m_points, colors, new Rect (0, 0, WIDTH, HEIGHT));
        m_edges = v.VoronoiDiagram();
        m_spanningTree = v.SpanningTree(KruskalType.MINIMUM);
        //m_delaunayTriangulation = v.DelaunayTriangulation();

        Vector2 randomOne = new Vector2(0, 0);
        Vector3 rndDir = new Vector3(0, 0, 0);
        Vector2 randomL = new Vector2(0, 0);

        //DrawRiver(pixels, map);
        
        /* Shows Voronoi diagram */
        Color color = Color.blue;
        for (int i = 0; i < m_edges.Count; i++) {
            LineSegment seg = m_edges[i];

            Vector2 left = (Vector2)seg.p0;
            Vector2 right = (Vector2)seg.p1;

            Vector2 mid = (left + right) / 2;
            float length = Vector3.Distance(left, right);
    
            Vector3 l = new Vector3(left.x * (-10) + 1000, 1, left.y * (-10) + 1000);
            Vector3 r = new Vector3(right.x * (-10) + 1000, 1, right.y * (-10) + 1000);
            Vector3 dir = l - r;
            
            GameObject prefab;
            GameObject habitation;
            
            if (map[(int) mid.x, (int) mid.y] < 0.7) {
                prefab = prefab_highway;
                habitation = prefab_house;
            }
            else {
                prefab = prefab_road;
                habitation = prefab_building;
                randomOne = mid;
                rndDir = dir;
                randomL = l;
            }

            if (works <= population) {
                // Pas assez de travail, on met des bureaux
                GameObject obj = Instantiate(prefab_office, new Vector3((mid.x)*(-10)+1000, 0.9f, mid.y*(-10)+1000), Quaternion.LookRotation(dir));
                works += obj.GetComponent<Office>().nbWorkers;
                offices.Add(obj);
            }
            else {
                // On met des habitations
                GameObject obj = Instantiate(habitation, new Vector3((mid.x)*(-10)+1000, 0.9f, mid.y*(-10)+1000), Quaternion.LookRotation(dir));
                habitations.Add(obj);

                population += obj.GetComponent<Home>().nInhabitants;
                houses.Add(obj);
            }
            prefab.transform.localScale = new Vector3(1, 1, length);
            Instantiate(prefab, new Vector3(mid.x*(-10)+1000, 1, mid.y*(-10)+1000), Quaternion.LookRotation(dir));
            Instantiate(prefab_pedestrian_cross, l, Quaternion.LookRotation(dir));
            Instantiate(prefab_pedestrian_cross, r, Quaternion.LookRotation(dir));
        }
        
        roadSurface.BuildNavMesh();
        walkwaySurface.BuildNavMesh();
        
        // Instantiate a car
        //Instantiate(prefab_car, new Vector3(randomOne.x*(-10)+1000, 1, randomOne.y*(-10)+1000), Quaternion.LookRotation(rndDir));

        //Vector2 p = new Vector2(-10, -10);
        Vector2 p = new Vector2(1000, 1000);
        Vector2 a = (randomOne*(-10.0f)+p) - (randomL*(-10)+p).normalized * 0.3f;
        Vector2 perpendicular = (new Vector2(a.y, -a.x)).normalized;
        
        //Instantiate(prefab_walker, new Vector3(randomOne.x*(-10)+1000, 1, randomOne.y*(-10)+1000), Quaternion.LookRotation(rndDir));

        //mainCam.transform.position = new Vector3(randomOne.x * (-10) + 1000, 50, randomOne.y * (-10) + 1000);
        //mainCam.transform.LookAt(new Vector3(randomOne.x*(-10)+1000, 1, randomOne.y*(-10)+1000));

        /* Apply pixels to texture */
        tx = new Texture2D(WIDTH, HEIGHT);
        land.SetTexture ("_MainTex", tx);
        tx.SetPixels(pixels);
        tx.Apply ();
    }

    // Update is called once per frame
    void Update() {
        
    }
    
    
}