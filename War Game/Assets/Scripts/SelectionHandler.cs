using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Handles raycasting between camera and mouseclick.
/// Place on camera
/// </summary>

[RequireComponent(typeof(Camera))]
public class SelectionHandler : MonoBehaviour {

	Camera Cam;
	public List<GameObject> Selected; //Public for debugging
	
	public LayerMask groundLayer;
	public LayerMask selectableLayer;

	LayerMask mask;

	// Use this for initialization
	void Start () {
		Cam = GetComponent<Camera>();
		Selected = new List<GameObject>();

		mask = groundLayer | selectableLayer;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0))
		{
			//Left click
			//For selecting selectables

			RaycastHit hitInfo;
			bool hit = Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out hitInfo, mask);
			
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
			//Right click 
			//For moving selected units or making them interact.

			RaycastHit hitInfo;
			bool hit = Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out hitInfo, 200, mask);

			/*
			//Interactable interactable = hitInfo.collider.GetComponent<Interactable>();

			if (interactable != null)
			{

			}
			*/

			if (hit)
			{
				if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Selectable"))
				{
					foreach (var item in Selected)
					{
						Unit unit = item.GetComponent<Unit>();
						if (unit != null)
						{
							unit.SetTarget(hitInfo.transform);
						}
					}
				}
				else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
				{
					foreach (var item in Selected)
					{
						Unit unit = item.GetComponent<Unit>();
						if (unit != null)
						{
							unit.SetDestination(hitInfo.point);
						}
					}
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
