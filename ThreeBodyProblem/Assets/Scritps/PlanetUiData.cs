using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUiData : MonoBehaviour {

    [Header("Prefab")]
    [SerializeField] private GameObject planetPrefab;

    [Header("Data values")]
    public Color trailColor;
    public Color planetColor;
    public string planetName;
    public float mass;
    public float radius;
    public float trailWidth;
    public float positionx;
    public float positiony;
    public float positionz;
    public float forcex;
    public float forcey;
    public float forcez;

    [Header("Ui settings")]
    [SerializeField] private float forceTrailWidthStart;
    [SerializeField] private float forceTrailWidthEnd;
    [SerializeField] private Color forceArrowColor;

    [Header("Ui elements")]
    [SerializeField] private InputField nameInput;
    [SerializeField] private InputField massInput;
    [SerializeField] private InputField radiusInput;
    [SerializeField] private InputField trailWidthInput;
    [SerializeField] private Dropdown trailColorInput;
    [SerializeField] private Dropdown planetColorInput;
    [SerializeField] private InputField positionxInput;
    [SerializeField] private InputField positionyInput;
    [SerializeField] private InputField positionzInput;
    [SerializeField] private InputField forcexInput;
    [SerializeField] private InputField forceyInput;
    [SerializeField] private InputField forcezInput;

    private Dictionary<int, Color> colorMap;
    public GameObject instance { get; set; }
    private LineRenderer planetForceLine;
    private TrailRenderer planetTrail;

    private void Awake() {
        //get references
        instance = Instantiate(planetPrefab, new Vector3(positionx, positiony, positionz), Quaternion.identity);
        planetForceLine = instance.GetComponent<LineRenderer>();
        planetTrail = instance.GetComponent<TrailRenderer>();
        planetTrail.enabled = false;

        //set permanet color for force line
        planetForceLine.startColor = Color.white;
        planetForceLine.endColor = Color.white;

        //color map options in map dropdown
        colorMap = new Dictionary<int, Color>();
        AddItemsToDropdown();

        //set basic settings
        nameInput.text = planetName;
        massInput.text = mass.ToString();
        radiusInput.text = radius.ToString();

        //trail
        trailWidthInput.text = trailWidth.ToString();
        trailColorInput.value = 7;
        UpdateColor();
        planetTrail.startColor = trailColor;
        planetTrail.endColor = trailColor;

        //planet color
        planetColorInput.value = 4;
        UpdatePlanetColor();

        //force arrow
        planetForceLine.startWidth = forceTrailWidthStart;
        planetForceLine.endWidth = forceTrailWidthEnd;
        planetForceLine.startColor = forceArrowColor;
        planetForceLine.endColor = forceArrowColor;

        //set position
        positionxInput.text = positionx.ToString();
        positionyInput.text = positiony.ToString();
        positionzInput.text = positionz.ToString();

        //set force
        forcexInput.text = forcex.ToString();
        forceyInput.text = forcey.ToString();
        forcezInput.text = forcez.ToString();
    }

    private void AddItemsToDropdown() {
        colorMap.Add(0, Color.black);
        colorMap.Add(1, Color.blue);
        colorMap.Add(2, Color.cyan);
        colorMap.Add(3, Color.gray);
        colorMap.Add(4, Color.green);
        colorMap.Add(5, Color.magenta);
        colorMap.Add(6, Color.red);
        colorMap.Add(7, Color.white);
        colorMap.Add(8, Color.yellow);

        List<string> optrions = new List<string>() { "Black", "Blue", "Cyan", "Gray", "Green", "Magenta", "Red", "White", "Yellow" };

        trailColorInput.ClearOptions();
        planetColorInput.ClearOptions();

        trailColorInput.AddOptions(optrions);
        planetColorInput.AddOptions(optrions);
    }

    public void UpdateName() {
        planetName = nameInput.text;
    }

    public void UpdateMass() {
        float outMass;
        if (float.TryParse(massInput.text, out outMass))
            mass = outMass;
        else {
            mass = 1;
            massInput.text = mass.ToString();
        }
    }

    public void UpdateRadius() {
        float outRadius;
        if (float.TryParse(radiusInput.text, out outRadius))
            radius = outRadius;
        else {
            radius = 1;
            radiusInput.text = mass.ToString();
        }

        instance.transform.localScale = new Vector3(radius, radius, radius);
        planetForceLine.startWidth = forceTrailWidthStart * radius;
        planetForceLine.endWidth = forceTrailWidthEnd * radius;

    }

    public void UpdateTrailWidth() {
        float outTrail;
        if (float.TryParse(trailWidthInput.text, out outTrail))
            trailWidth = outTrail;
        else {
            trailWidth = 0.1f;
            trailWidthInput.text = mass.ToString();
        }
    }

    public void UpdateColor() {
        trailColor = colorMap[trailColorInput.value];
        planetTrail.startColor = trailColor;
        planetTrail.endColor = trailColor;
    }

    public void UpdatePlanetColor() {
        planetColor = colorMap[planetColorInput.value];
        instance.GetComponent<Renderer>().material.SetColor("_Color", planetColor);
    }

    public void UpdatePosition() {
        float outX;
        float outY;
        float outZ;

        if (float.TryParse(positionxInput.text, out outX))
            positionx = outX;
        else {
            positionx = 0;
            positionxInput.text = positionx.ToString();
        }

        if (float.TryParse(positionyInput.text, out outY))
            positiony = outY;
        else {
            positiony = 0;
            positionyInput.text = positiony.ToString();
        }

        if (float.TryParse(positionzInput.text, out outZ))
            positionz = outZ;
        else {
            positionz = 0;
            positionzInput.text = positionz.ToString();
        }

        instance.transform.position = new Vector3(positionx, positiony, positionz);

    }

    public void UpdateForce() {
        float outX;
        float outY;
        float outZ;

        if (float.TryParse(forcexInput.text, out outX))
            forcex = outX;
        else {
            forcex = 0;
            forcexInput.text = forcex.ToString();
        }

        if (float.TryParse(forceyInput.text, out outY))
            forcey = outY;
        else {
            forcey = 0;
            forceyInput.text = forcey.ToString();
        }

        if (float.TryParse(forcezInput.text, out outZ))
            forcez = outZ;
        else {
            forcez = 0;
            forcezInput.text = positionz.ToString();
        }

        Vector3[] pos = new Vector3[2];
        Vector3 force = new Vector3(forcex, forcey, forcez);

        pos[0] = instance.transform.position + force.normalized * radius * 0.5f;
        pos[1] = instance.transform.position + force.normalized * radius * 0.5f + force;

        planetForceLine.SetPositions(pos);

    }

    public void UpdatePosition(Vector3 position) {
        positionxInput.text = position.x.ToString();
        positionyInput.text = position.y.ToString();
        positionzInput.text = position.z.ToString();
        UpdatePosition();
    }
}
