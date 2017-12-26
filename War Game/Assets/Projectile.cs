using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float bulletSpeed = 0.0f;
    public Vector3 myDir;
    public float speed = 30.0f; //Probably don't need a slerp for this
    private Transform target;
    private float damage;


    public void Init(Transform t, float d)
    {
        damage = d;
        target = t;
        Vector3 vectorToTarget = target.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
    }
    void Update()
    {
        //Move Projectile
        //transform.position += transform.right * bulletSpeed * Time.deltaTime;
    }
    void OnTriggerEnter(Collider other)//void OnCollisionEnter(Collision hit)
    {
        
        
        if (other.tag == "Unit")
        {
            Debug.Log("Collition " + other.name);

            Unit unit = other.GetComponent<Unit>();
            if (unit != null)
            {
                unit.TakeDamage(damage);
            }
        }

        
    }
}
