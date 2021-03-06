﻿
using UnityEngine;

public class CameraController : MonoBehaviour {

    public bool enableMousePanning = true;
	public float panSpeed = 40f;
	public float panBorderThickness = 10f;
	public Vector2 panLimits;
	public float scrollSpeed = 20f;
	public float minY = 10;
	public float maxY = 80;
	public float smoothing = 0.125f;
	
	// Update is called once per frame
	void Update () {

		Vector3 pos = transform.position;

		//Pan
		if (Input.GetKey("w") || (enableMousePanning && Input.mousePosition.y >= Screen.height - panBorderThickness))
		{
			pos.z += panSpeed * Time.deltaTime;
		}
		else if (Input.GetKey("s") || (enableMousePanning && Input.mousePosition.y <= panBorderThickness))
		{
			pos.z -= panSpeed * Time.deltaTime;
		}
		else if(Input.GetKey("d") || (enableMousePanning && Input.mousePosition.x >= Screen.width - panBorderThickness))
		{
			pos.x += panSpeed * Time.deltaTime;
		}
		else if(Input.GetKey("a") || (enableMousePanning && Input.mousePosition.x <= panBorderThickness))
		{
			pos.x -= panSpeed * Time.deltaTime;
		}

		//Zoom
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		pos = pos + transform.forward * scroll * scrollSpeed * 100f * Time.deltaTime;

		//Limits
		pos.x = Mathf.Clamp(pos.x, -panLimits.x, panLimits.x);
		pos.y = Mathf.Clamp(pos.y, minY, maxY);
		pos.z = Mathf.Clamp(pos.z, -panLimits.y, panLimits.y);
		
		//Apply changes
		transform.position = pos;
	}
}
