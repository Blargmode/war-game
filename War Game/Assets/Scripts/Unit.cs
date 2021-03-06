﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour {

	private bool selected = false;
	public UnitType unitType = UnitType.Melee;
	public float range = 1f;
	public float health = 100f;
	public float damage = 10f;
	public float interactionCooldown = 1f;
	float interactionTimer = 0f;
	public GameObject projectile;
	public float projectileDespawnDelay = 5f;

	private float startHealth;
	
	private Transform target;
	private Unit targetUnit;
	NavMeshAgent agent;

	public GameObject healthbar;
	public float healthbarOffset = 2f;
	Canvas healthbarCanvas;
	Image healthbarValue;
	private float healthbarSize = 0f;

	// Use this for initialization
	void Start () {
		startHealth = health;
		agent = transform.GetComponent<NavMeshAgent>();
		healthbarCanvas = GameObject.Find("Healthbars Canvas").GetComponent<Canvas>();
		healthbar = Instantiate(healthbar, healthbarCanvas.transform);
		healthbar.SetActive(false);
		healthbarSize = Camera.main.GetComponent<CameraController>().minY;
		healthbarValue = healthbar.transform.GetChild(0).GetComponent<Image>();
	}

	void LateUpdate()
	{
		if (selected)
		{
			float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
			healthbar.transform.localScale = new Vector3((healthbarSize / distance), (healthbarSize / distance), healthbar.transform.localScale.z);
			healthbar.transform.position = Camera.main.WorldToScreenPoint((Vector3.up * healthbarOffset) + transform.position);
		}
	}

	
	// PERFORMANCE NOTE
	// In fixed update for now. Maybe reduce how often this happens even more.
	// Maybe something like how I reduce load per tick in Space Engineers.
	void FixedUpdate () {
		if(target != null)
		{
			float distance = Vector3.Distance(transform.position, target.position);
			if(distance <= range)
			{
				//In range
				Interact();
			}
			else
			{
				//Too far away
				agent.destination = target.position;
			}
		}
	}

	//Can be called from the selection handler
	public void SetDestination(Vector3 position)
	{
		target = null;
		agent.stoppingDistance = 0f; //Has to be here. Have not tested other values. Without this it can't move again after attacking. Don't ask me why.
		agent.destination = position;
	}

	//Can be called from the selection handler
	public void SetTarget(Transform newTarget)
	{
		target = newTarget;
		agent.stoppingDistance = range - 1f;
		targetUnit = target.gameObject.GetComponent<Unit>();
	}

	private void Interact()
	{
		interactionTimer -= Time.deltaTime;
		if (interactionTimer <= 0)
		{
			interactionTimer = interactionCooldown;

			if (unitType == UnitType.Melee)
			{
				if (!targetUnit.TakeDamage(damage))
				{
					//If false, target can't take anymore damage a.k.a. dead
					target = null;
				}
			}
			else if (unitType == UnitType.Ranged)
				Fire();
		}
	}

	//Returns true if damage is taken
	public bool TakeDamage(float amount)
	{
		health -= amount; //Mathf.Clamp(amount, 0, float.MaxValue); //prevents healing -- = +

		healthbarValue.fillAmount = health / startHealth;

		Debug.Log("Health: " + healthbarValue.fillAmount);

		if (health <= 0)
		{
			Die();
			return false;
		}
		else if(health > startHealth)
		{
			//For healing
			health = startHealth;
			return false;
		}
		return true;
	}

	private void Die()
	{
		Destroy(healthbar);
		Destroy(gameObject);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, range);
	}

	void Fire()
	{
		// Create the Bullet from the Bullet Prefab
		if (projectile != null)
		{
			Vector3 spawnPos = transform.position + transform.forward * 1;

			var newProjectile = Instantiate(
				projectile,
				spawnPos,
				transform.rotation);

			newProjectile.GetComponent<Projectile>().Init(target, damage);

			// Add velocity to the bullet
			newProjectile.GetComponent<Rigidbody>().velocity = newProjectile.transform.forward * 10;

			// Destroy the bullet after x seconds
			Destroy(newProjectile, projectileDespawnDelay);
		}
	}

	public void SetSelected(bool yes)
	{
		healthbar.SetActive(yes);
		selected = yes;
	}
}

public enum UnitType
{
	Melee,
	Ranged
}
