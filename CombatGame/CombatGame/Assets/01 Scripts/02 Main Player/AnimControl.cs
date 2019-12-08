using UnityEngine;
using CustomController;

public class AnimControl : MonoBehaviour
{
    private Animator animator;
    private PhysCharacterController controller;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(controller)
            animator.SetFloat("Speed", controller.Velocity.magnitude / 6);
    }

    public void SetController(MonoBehaviour control) 
    {
        controller = (PhysCharacterController)control;
    }
}
