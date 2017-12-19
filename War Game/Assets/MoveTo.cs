using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour {

	public Transform Goal;

	// Use this for initialization
	void Start () {
		NavMeshAgent agent = GetComponent<NavMeshAgent>();
		agent.destination = Goal.position;
	}
}
