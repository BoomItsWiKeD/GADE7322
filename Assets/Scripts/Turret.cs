using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;
    
    public GameObject missile1;
    public GameObject missile2;
    public GameObject missile3;
    
    public int missileSpeed;
    private float shootDelay;
    public GameObject turretPos;

    private string nameOfTurretObject;

    public static float health1;
    public static float health2;
    public static float health3;

    public static float shootDelay1;
    public static float shootDelay2;
    public static float shootDelay3;
    
    // Start is called before the first frame update
    void Start()
    {
        //Changing the shoot delay and health for each tower:
        if (this.gameObject.tag == "Tower1") //Normal tower
        {
            shootDelay = shootDelay1;
        }
        if (this.gameObject.tag == "Tower2") //Fast tower
        {
            shootDelay = shootDelay2;
        }
        if (this.gameObject.tag == "Tower3") //Heavy tower
        {
            shootDelay = shootDelay3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Find enemy and look at enemy:
        transform.LookAt(target);
        shootDelay -= Time.deltaTime;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy1" || other.tag == "Enemy2" || other.tag == "Enemy3")
        {
            target = other.gameObject.transform;
            transform.LookAt(target);
            
            if (shootDelay <= 0)
            {
                if (this.gameObject.tag == "Tower1") //Normal tower
                {
                    shootDelay = shootDelay1;
                    GameObject _projectile = Instantiate(missile1, turretPos.transform.position, transform.rotation) as GameObject;
                    _projectile.GetComponent<Rigidbody>().AddForce(transform.forward * missileSpeed);
                }
                if (this.gameObject.tag == "Tower2") //Fast tower
                {
                    shootDelay = shootDelay2;
                    GameObject _projectile = Instantiate(missile2, turretPos.transform.position, transform.rotation) as GameObject;
                    _projectile.GetComponent<Rigidbody>().AddForce(transform.forward * missileSpeed);
                }
                if (this.gameObject.tag == "Tower3") //Heavy tower
                {
                    shootDelay = shootDelay3;
                    GameObject _projectile = Instantiate(missile3, turretPos.transform.position, transform.rotation) as GameObject;
                    _projectile.GetComponent<Rigidbody>().AddForce(transform.forward * missileSpeed);
                }
            }
        }
    }
}
