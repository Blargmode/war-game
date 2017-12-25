using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitSelection : MonoBehaviour {

	Camera Cam;
	public List<GameObject> Selected; //Public for debugging

	// Use this for initialization
	void Start () {
		Cam = GetComponent<Camera>();
		Selected = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0))
		{
			RaycastHit hitInfo;
			bool hit = Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out hitInfo);

			if (hit && HasTag(hitInfo.transform.tag, "Unit"))
			{
				if (Input.GetKey(KeyCode.LeftControl))
				{
					Select(hitInfo.transform.gameObject);
				}
				else
				{
					Deselect();
					Select(hitInfo.transform.gameObject);
				}
				
			}
			else
			{
				Deselect();
			}
		}
		if (Input.GetMouseButtonUp(1) && Selected.Count > 0)
		{
			RaycastHit hitInfo;
			bool hit = Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out hitInfo);

			if (hit)
			{
				foreach (var item in Selected)
				{
					NavMeshAgent agent = item.GetComponent<NavMeshAgent>();
					agent.destination = hitInfo.point;
				}
			}
		}
	}


	void Deselect()
	{
		if (Selected.Count > 0) Selected.Clear();
	}

	void Select(GameObject item)
	{
		if (!Selected.Contains(item))
		{
			Selected.Add(item);
		}
	}

	// Checks if the tag is any of the entered tags
	bool HasTag(string tag, params string[] tags)
	{
		foreach (var item in tags)
		{
			if (tag == item) return true;
		}
		return false;
	}
}
