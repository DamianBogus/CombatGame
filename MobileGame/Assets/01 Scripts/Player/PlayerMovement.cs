using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private string moveInput = "Horizontal";
    [SerializeField] private string jumpInput = "Jump";
    private float moveSpeed = 5.0f;
    private float jumpForce = 5.0f;
    [SerializeField] private PhysicMaterial physicsMaterial;
    
    private Rigidbody rBody;

    //Movement
    private float inputVal = 0.0f;
    private float stopFriction = 1.0f;
    private float moveFriction = 0.0f;
    
    //jump
    private bool jumpPressed = false;
    private bool isGrounded = false;
    private int layerMask = 0;
    private float hitDistance;

    void Start()
    {
        if(rBody == null)
            rBody = GetComponent<Rigidbody>();

        layerMask = LayerMask.GetMask("Ground");
    }

    void Update() 
    {
        inputVal = Input.GetAxis(moveInput);
        jumpPressed = Input.GetButtonDown(jumpInput);
    }

    void FixedUpdate()
    {
        CheckGrounded();

        //Update Friction
        if (physicsMaterial)
            physicsMaterial.dynamicFriction = (Mathf.Abs(inputVal) < 0.1f) ? stopFriction : moveFriction;

        //Force Motion
        if(isGrounded)
            rBody.AddForce(Vector3.right * moveSpeed * inputVal);

        //Jump
        if (jumpPressed && isGrounded)
            rBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void CheckGrounded() 
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.5f, -Vector3.up, out hit,
            0.52f))
        {
            isGrounded = true;
            hitDistance = hit.distance;
        }
        else
        {
            isGrounded = false;
        }

        rBody.useGravity = !isGrounded;
    }



    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position, transform.position + -Vector3.up * hitDistance);
        Gizmos.DrawWireSphere(transform.position + -Vector3.up * hitDistance, 0.5f);
    }
}
