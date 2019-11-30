using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoveController : MonoBehaviour
{
    private Transform cameraTransform;
    private CharacterController controller;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    void Move() 
    {
        float angle = cameraTransform.eulerAngles.y;
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(Quaternion.Euler(0, 0, angle) * input);


    }
}
