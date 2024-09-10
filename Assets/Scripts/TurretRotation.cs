using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation : MonoBehaviour
{
    public Transform target;

    //Update is called once per frame:
    void FixedUpdate()
    {
        //Find enemy and look at enemy:
        target = GameObject.FindGameObjectWithTag("Enemy1").transform;
        transform.LookAt(target);
    }
}
