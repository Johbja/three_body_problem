using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHidePanel : MonoBehaviour {

    [SerializeField] private float animationSpeed;
    [SerializeField] private float distanceSpeedMulti;
    [SerializeField] private float distanceTolorance;
	[SerializeField] private Vector3 hidePos;

    private RotateImage imageRotator;
	RectTransform rTransfrom;
    private bool visability;
	private Vector3 startPos;

    private void Start() {
        imageRotator = GetComponentInChildren<RotateImage>();
        visability = true;
		rTransfrom = GetComponent<RectTransform>();
		startPos = rTransfrom.anchoredPosition;
    }

    public void ChangePanelVisability() {
        visability = !visability;

        if(visability) {
            StopAllRunningCorutiones();
            imageRotator.StartRotation(0);
            StartCoroutine(AnimateWindow(startPos) as IEnumerator);
        } else {
            StopAllRunningCorutiones();
            imageRotator.StartRotation(180);
            StartCoroutine(AnimateWindow(hidePos) as IEnumerator);
        }
    }

    private IEnumerator AnimateWindow(Vector3 tragetPosition) {
        float distance = Vector3.Distance(rTransfrom.anchoredPosition, tragetPosition);

        while(distance > distanceTolorance) {
            rTransfrom.anchoredPosition = Vector3.MoveTowards(rTransfrom.anchoredPosition, tragetPosition, animationSpeed * distance * distanceSpeedMulti);
            distance = Vector3.Distance(rTransfrom.anchoredPosition, tragetPosition);
            imageRotator.Distance = distance;
            yield return null;
        }
    }

    private void StopAllRunningCorutiones() {
        StopAllCoroutines();
        imageRotator.StopAllCoroutines();
    }
}
