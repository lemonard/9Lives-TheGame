using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullingWhipRenderer : MonoBehaviour {

	public GameObject whipStartingPointRight;
	public GameObject whipStartingPointLeft;

	public float lineWidth;

	public bool isActive;
	public bool toTheRight;

	private GameObject target;

	public Vector3[] points;

	public Material whipMaterial;

	private LineRenderer currentLineRenderer;

	// Use this for initialization
	void Start () {
		points = new Vector3[2];
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			if (toTheRight) {
				points [0] = new Vector3 (whipStartingPointRight.transform.position.x, whipStartingPointRight.transform.position.y, whipStartingPointRight.transform.position.z);
				points [1] = new Vector3 (target.transform.position.x, target.transform.position.y, target.transform.position.z);
				currentLineRenderer.SetPositions (points);
			} else {
				points [0] = new Vector3 (whipStartingPointLeft.transform.position.x, whipStartingPointLeft.transform.position.y, whipStartingPointLeft.transform.position.z);
				points [1] = new Vector3 (target.transform.position.x, target.transform.position.y, target.transform.position.z);
				currentLineRenderer.SetPositions (points);
			}
		}
	}

	public void RenderWhip(bool isRight, GameObject whipTarget){
		target = whipTarget;
		toTheRight = isRight;

		isActive = true;

		currentLineRenderer = gameObject.AddComponent<LineRenderer> ();
		currentLineRenderer.material = whipMaterial;
		currentLineRenderer.positionCount = 2;
		currentLineRenderer.widthMultiplier = lineWidth;
		currentLineRenderer.sortingOrder = -3;

		if (isRight) {
			points[0] = new Vector3(whipStartingPointRight.transform.position.x, whipStartingPointRight.transform.position.y, whipStartingPointRight.transform.position.z);
			points[1] = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
			currentLineRenderer.SetPositions (points);
		} else {
			points[0] = new Vector3(whipStartingPointLeft.transform.position.x, whipStartingPointLeft.transform.position.y, whipStartingPointLeft.transform.position.z);
			points[1] = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
			currentLineRenderer.SetPositions (points);
		}
	}

	public void DestroyWhip(){
		isActive = false;
		Destroy (currentLineRenderer);
		currentLineRenderer = null;
	}
}
