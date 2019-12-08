using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomController;

public class RotationControl : MonoBehaviour
{
    PhysCharacterController controller;
    
    void Start() 
    {
        controller = GetComponent<PhysCharacterController>();
    }

    void Update() 
    {
        Vector3 dir = controller.Velocity;
        dir.y = 0;

        if (dir.magnitude < 0.1f)
            dir = transform.forward;

        transform.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
    }
}
