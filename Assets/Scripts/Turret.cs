using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;
    public GameObject missile;
    public int missileSpeed;
    public float shootDelay;
    public GameObject turretPos;
    // Start is called before the first frame update
    void Start()
    {
        shootDelay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        shootDelay -= Time.deltaTime;
    }
    void FixedUpdate()
    {
        //Find enemy and look at enemy:
        transform.LookAt(target);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy1" || other.tag == "Enemy2" || other.tag == "Enemy3")
        {
            target = other.gameObject.transform;
            if (shootDelay <= 0)
            {
                shootDelay = 1;
                GameObject _projectile = Instantiate(missile, turretPos.transform.position, transform.rotation) as GameObject;
                _projectile.GetComponent<Rigidbody>().AddForce(transform.forward * missileSpeed);
            }
            
        }
    }
}
