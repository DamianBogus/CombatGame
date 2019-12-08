using UnityEngine;
using CustomController;
public class CameraController : MonoBehaviour
{
    #region Camera Properties
    [Header("Camera Properties")]
    public Transform defaultOwner;
    [Range(50.0f, 200.0f), SerializeField]
    private float xAxisSensitivity = 100.0f;
    [Range(50.0f, 200.0f), SerializeField]
    private float yAxisSensitivity = 100.0f;
    #endregion

    #region Private Fields
    private PhysCharacterController owner;
    private Camera camera;
    private float xRot = 0;
    private float yRot = 0;
    private float followTime = 0.05f;
    private bool followPlayer = false;
    private Quaternion targetRotation;
    private Vector2 inputVector;
    #endregion

    void Start() 
    {
        if (defaultOwner)
            owner = defaultOwner.GetComponent<PhysCharacterController>();
       camera = Camera.main;
       targetRotation = owner.transform.rotation;
    }

    void Update() 
    {
        CheckFollow();
        GetInput();
    }
    void FixedUpdate() 
    {
        MoveCamera();
        RotateCamera();
    }

    void MoveCamera() 
    {
        Vector3 vel = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, owner.CharacterRigidbody.position, ref vel, followTime);
    }

    void GetInput() 
    {
        inputVector.x = Input.GetAxis("MouseX");
        inputVector.y = Input.GetAxis("MouseY");
    }
    void RotateCamera()
    {
        if (followPlayer && inputVector == Vector2.zero)
        {
            targetRotation = Quaternion.Lerp(transform.rotation, owner.transform.rotation, 0.5f * Time.deltaTime);
        }
        else
        {
            Vector3 rot = targetRotation.eulerAngles;
            rot.y += inputVector.x * yAxisSensitivity * Time.deltaTime;
            rot.x += inputVector.y * xAxisSensitivity * Time.deltaTime;
            targetRotation = Quaternion.Euler(rot);
        }

        
        transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y,0);

    }

    void CheckFollow() 
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.8f && Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f)
            followPlayer = true;
        else
            followPlayer = false;
    }
}   
