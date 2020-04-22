using System.Collections;
using UnityEngine;

public class RotateImage : MonoBehaviour {

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float minRotationSpeed;
    [SerializeField] private float distanceSpeedMulti;

    private float distance;
    public float Distance { private get { return distance; }
                            set { distance = Mathf.Clamp(value, minRotationSpeed, rotationSpeed); } }

    public void StartRotation(float rotationTarget) {
        Distance = 1;
        StartCoroutine(StartRotateImage(Quaternion.Euler(0, 0, rotationTarget)) as IEnumerator);
    }

    private IEnumerator StartRotateImage(Quaternion targetRotation) {
        while(transform.rotation != targetRotation) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * distance * distanceSpeedMulti);
            yield return null;
        }
    }

}
