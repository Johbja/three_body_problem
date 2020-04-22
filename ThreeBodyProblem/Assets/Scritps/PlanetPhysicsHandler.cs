using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetPhysicsHandler : MonoBehaviour {

    [Header("Physics settings")]
    [SerializeField] private GameObject PlanetPrefab;
    [SerializeField] private float gravitationalConstant;

    [Header("Ui")]
    [SerializeField] private InputField gravityText;
    [SerializeField] private GameObject uiSettingsPrefab;
    [SerializeField] private Transform planetSettingsParent;
    [SerializeField] private Text pauseButtonText;

    private List<PlanetParticel> planets;
    public List<PlanetUiData> uiSettings { get; private set; }
    private bool isAdding;

    private void Start() {
        planets = new List<PlanetParticel>();
        uiSettings = new List<PlanetUiData>();

        isAdding = false;

        gravityText.text = gravitationalConstant.ToString();

        StableOrbit1();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.R)) {
            GenerateNewPlanets();
        }
    }

    private void FixedUpdate() {

        if(!isAdding) {
            // add force to all planets based on newton's laws of gravitation
            foreach(var planet in planets) {
                foreach(var otherPlanet in planets) {
                    if(planet != otherPlanet) {
                        Vector3 force = CalculateForce(planet.mass, otherPlanet.mass, gravitationalConstant, planet.transform.position, otherPlanet.transform.position);
                        planet.AddForce(force);
                    }
                }
            }

            //update all the planets postitions based on there velocity
            foreach(var planet in planets) {
                planet.UpdatePlanet();
            }
        }
    }

    private Vector3 CalculateForce(float m1, float m2, float gConst, Vector3 planetPos1, Vector3 planetPos2) {
        Vector3 direction = (planetPos2 - planetPos1);
        float distance = direction.magnitude;
        float force = gConst * m1 * m2 * (1 / (distance * distance));
        return force * direction.normalized;
    }

    public void AddPlanet(Vector3 position, Vector3 force, float mass, float radius, float trailWidth, Color trailColor, Color planetColor, string name) {
        //set scripts etc on object
        GameObject instance = Instantiate(PlanetPrefab, position, Quaternion.identity, transform);
        instance.transform.localScale = new Vector3(radius, radius, radius);
        instance.name = name;
        instance.GetComponent<LineRenderer>().enabled = false;
        instance.GetComponent<Renderer>().material.SetColor("_Color", planetColor);

        //set planet settings
        PlanetParticel planet = instance.GetComponent<PlanetParticel>();
        planet.InitializePlanet(force, mass, trailWidth, trailColor);
        planets.Add(planet);
    }

    public Vector3 CalculateCenterOfMass() {

        Vector3 vectorSum = new Vector3(0, 0, 0);
        float scalerSum = 0;

        //count all planets in simulation
        foreach(var planet in planets) {
            vectorSum += planet.transform.position * planet.mass;
            scalerSum += planet.mass;
        }

        //count with the temporary planets that are not added to simulation yet
        foreach(var planet in uiSettings) {
            vectorSum += planet.instance.transform.position * planet.mass;
            scalerSum += planet.mass;
        }

        if(scalerSum == 0)
            return vectorSum;

        return vectorSum / scalerSum;
    }

    public void UpdateGravityText() {
        float result;

        if(float.TryParse(gravityText.text, out result))
            gravitationalConstant = result;
        else {
            gravitationalConstant = 0.1f;
            gravityText.text = gravitationalConstant.ToString();
        }
    }

    public void AddPlanetSettings() {
        isAdding = true;
        GameObject instance = Instantiate(uiSettingsPrefab, CalculateCenterOfMass(), Quaternion.identity, planetSettingsParent);
        PlanetUiData uiData = instance.GetComponent<PlanetUiData>();
        uiData.UpdatePosition(instance.transform.position);
        
        uiSettings.Add(uiData);
    }

    public void AddPlanetsToSimulation() {
        
        foreach(var setting in uiSettings) {
            Vector3 pos = new Vector3(setting.positionx, setting.positiony, setting.positionz);
            Vector3 force = new Vector3(setting.forcex, setting.forcey, setting.forcez);
            
            AddPlanet(pos, force, setting.mass, setting.radius, setting.trailWidth, setting.trailColor, setting.planetColor, setting.planetName);

            Destroy(setting.instance);
            Destroy(setting.gameObject);

        }

        uiSettings.Clear();

        isAdding = false;
    }

    public void SetSimulation() {
        isAdding = !isAdding;

        if(!isAdding)
            pauseButtonText.text = "Pause simulation";
        else
            pauseButtonText.text = "Resume simulation";
    }

    private void StableOrbit1() {
        Vector3 forceMoon = new Vector3(3, 2, 1);
        Vector3 forceMars = new Vector3(1, 2, 3);
        Vector3 forceEarth = forceMars - forceMoon;

        //sort of stable version
        AddPlanet(Vector3.right * 20, forceMoon, 1, 1, 0.1f, Color.blue, Color.blue, "Moon");
        AddPlanet(Vector3.zero, forceEarth, 81, 1, 0.1f, Color.red, Color.red, "Earth");
        AddPlanet(Vector3.left * 20, forceMars, 1, 1, 0.1f, Color.green, Color.green, "Mars");
    }

    private void StableOrbit2() {
        Vector3 force1 = new Vector3(-1, 1, 0);
        Vector3 force2 = new Vector3(1, -1, 0);
        Vector3 force3 = force2 - force1;

        //sort of stable version
        AddPlanet(Vector3.right * 20, force1, 1, 1, 0.1f, Color.blue, Color.blue, "Moon");
        AddPlanet(Vector3.zero, force3, 1, 1, 0.1f, Color.red, Color.red, "Earth");
        AddPlanet(Vector3.left * 20, force2, 1, 1, 0.1f, Color.green, Color.green, "Mars");
    }

    private void GenerateNewPlanets() {
        //sort of stable version

        foreach(var planet in planets)
            Destroy(planet.gameObject);

        planets.Clear();

        Vector3 randomOffset1 = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        Vector3 randomOffset2 = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));

        Vector3 randomForce1 = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        Vector3 randomForce2 = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        Vector3 randomForce3 = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));

        AddPlanet(Vector3.zero, randomForce1, Random.Range(1, 100), 1, 0.1f, Color.blue, Color.blue, "planet 1");
        AddPlanet(randomOffset1, randomForce2, Random.Range(1, 100), 1, 0.1f, Color.red, Color.red, "planet 2");
        AddPlanet(randomOffset2 * 20, randomForce3, Random.Range(1, 100), 1, 0.1f, Color.green, Color.green, "planet 3");
    }

}
