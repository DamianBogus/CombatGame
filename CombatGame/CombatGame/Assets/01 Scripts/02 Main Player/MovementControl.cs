using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomController;
public class MovementControl : MonoBehaviour
{
    private Transform cameraTransform;
    private PhysCharacterController controller;

    [SerializeField]
    private float moveSpeed = 3.0f;

    private float gravity = 9.8f;

    private Vector3 inputVector;

    public AnimControl animController;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        controller = GetComponent<PhysCharacterController>();

        if (animController)
            animController.SetController(controller);
    }

    void Update()
    {
        Move();

        controller.ToggleRun(Input.GetButton("Sprint"));
            
    }

    void Move() 
    {
        Vector3 forward = cameraTransform.forward * Input.GetAxis("Vertical");  
        Vector3 right = cameraTransform.right * Input.GetAxis("Horizontal"); 
        inputVector = (forward + right);
        inputVector.y = 0;
        controller.Move(inputVector);
    }
    
}
