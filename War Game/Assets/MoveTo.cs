﻿using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour {

	//Simplest example for making a NavMeshAgent move to a postion

	public Transform Goal;

	// Use this for initialization
	void Start () {
		NavMeshAgent agent = GetComponent<NavMeshAgent>();
		agent.destination = Goal.position;
	}
}
