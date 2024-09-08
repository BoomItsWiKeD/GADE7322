using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
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

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy1" && shootDelay <= 0)
        {
            shootDelay = 1;
            GameObject _projectile = Instantiate(missile, turretPos.transform.position, transform.rotation) as GameObject;
            _projectile.GetComponent<Rigidbody>().AddForce(transform.forward * missileSpeed);
        }
    }
}
