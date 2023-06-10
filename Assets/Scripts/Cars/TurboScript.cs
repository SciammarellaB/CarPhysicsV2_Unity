using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboScript : MonoBehaviour {

	public float turboBoost;
	public float maxTurboBoost;

	public CarBaseScript carScript;

	// Use this for initialization
	void Start () 
	{
		carScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CarBaseScript>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		carScript.gearTorque = carScript.gearTorque * turboBoost;
	}

	void Turbo()
	{

	}
}
