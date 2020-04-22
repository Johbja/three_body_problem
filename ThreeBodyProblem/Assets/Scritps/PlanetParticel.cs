using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetParticel : MonoBehaviour {

    public float mass { get; private set; }

    private TrailRenderer trail;
    private Vector3 velocity;

    public void InitializePlanet(Vector3 startForce, float _mass, float trailWidth, Color trailColor) {
        mass = _mass;
        velocity = CalculateAcceleraton(startForce);

        trail = GetComponent<TrailRenderer>();
        trail.startColor = trailColor;
        trail.endColor = trailColor;
        trail.startWidth = trailWidth;
        trail.endWidth = trailWidth;
    }

    public void UpdatePlanet() {
        transform.position += velocity * Time.fixedDeltaTime;
    }

    public void AddForce(Vector3 force) {
        velocity += CalculateAcceleraton(force);
    }

    private Vector3 CalculateAcceleraton(Vector3 force) {
        return force * (1 / mass);
    }
}
