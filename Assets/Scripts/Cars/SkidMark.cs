using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidMark : MonoBehaviour {

	public WheelCollider wheelCollider;
	public GameObject skidMarkPrefab;
	public TrailRenderer tR;

	// Use this for initialization
	void Start ()
	{
		skidMarkPrefab.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		RaycastHit hit;

		Vector3 colliderCenterPoint = wheelCollider.transform.TransformPoint(wheelCollider.center);

		if(Physics.Raycast(colliderCenterPoint,-wheelCollider.transform.up,out hit,wheelCollider.suspensionDistance))
		{
			transform.position = hit.point + (wheelCollider.transform.up * wheelCollider.radius);
		}

		else
		{
			transform.position = colliderCenterPoint - (wheelCollider.transform.up * wheelCollider.suspensionDistance);
		}

		WheelHit groundHit;

		wheelCollider.GetGroundHit(out groundHit);

		if(Mathf.Abs(groundHit.sidewaysSlip) > 0.2f)
		{
			skidMarkPrefab.SetActive(true);
		}

		else if (Mathf.Abs(groundHit.sidewaysSlip) <= 0.75f)
		{
			skidMarkPrefab.SetActive(false);
		}


	}
}
