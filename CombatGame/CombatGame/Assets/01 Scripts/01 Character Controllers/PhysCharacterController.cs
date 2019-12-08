using UnityEngine;
using UnityEngine.UI; //TEMPORARy

namespace CustomController
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    class PhysCharacterController : MonoBehaviour
    {
        #region TEMPORARY
        public Text velText;
        #endregion

        #region Fields
        [SerializeField]
        private float slopeLimit = 0.2f;
        [SerializeField]
        private float stepOffset = 0.22f;
        [SerializeField]
        private float groundedOffset = 0.01f;
        [SerializeField]
        private float acceleration = 0.75f;
        [SerializeField]
        private float walkSpeed = 2.5f;
        [SerializeField]
        private float runSpeed = 6.0f;
        [SerializeField]
        private float rotationSpeed = 0.05f;

        private float currentSpeed = 0.0f;
        private float desiredSpeed = 0.0f;
        private float deceleration = 1.0f;
        /// <summary>
        /// Used to Calculate Velocity
        /// </summary>
        private Vector3 lastPosition;
        #endregion

        #region Properties
        public bool IsGrounded { get; private set; }
        public float SlopeLimit { get; private set; }
        public float StepOffset { get; private set; }
        public float GroundedOffset { get; private set; }
        public CapsuleCollider CharacterCollider { get; private set; }
        public Rigidbody CharacterRigidbody { get; private set; }
        public Vector3 InputVector { get; private set; }
        public Vector3 DesiredDirection { get; private set; }
        public Vector3 DesiredVelocity { get; private set; }
        public Vector3 Velocity { get; private set; }
        public float MoveSpeed { get; private set; }
        #endregion

        #region Ground Checks
        private RaycastHit groundHit;
        public float GroundAngle { get; private set; }
        #endregion


        void Awake()
        {
            SlopeLimit = slopeLimit;
            StepOffset = stepOffset;
            GroundedOffset = groundedOffset;
            CharacterCollider = GetComponent<CapsuleCollider>();
            CharacterRigidbody = GetComponent<Rigidbody>();
            MoveSpeed = walkSpeed;
        }

        void FixedUpdate()
        {
            CheckGrounded();
            ApplyGravity();
            GetGroundAngle();
            GetDirectionVector();
            PhysicsMove();
            GetVelocity();

#if (UNITY_EDITOR)

            DrawDebugLines();
#endif
        }

        #region Ground Functions
        private void CheckGrounded()
        {
            IsGrounded = (Physics.Raycast(transform.position, -Vector3.up, out groundHit, CharacterCollider.height / 2 + 0.1f));

#if(UNITY_EDITOR)
            Color c = (IsGrounded) ? Color.green : Color.red;
            Debug.DrawRay(transform.position, -Vector3.up, c);
#endif
        }
        private void GetGroundAngle()
        {
            if (!IsGrounded)
                GroundAngle = 90;
            else
                GroundAngle = Vector3.Angle(groundHit.normal, transform.forward);
        }
        private void GetDirectionVector()
        {
            /*
            //I was using the ForwardVector (direction) as my magnitude. thats wrong
            use forward vector as normalized =1. for desired direction
            use input vector to determine desired move speed
            use a derived float to accelerate to that velocity ie speed * direction;
             
             
             
             */

            if (!IsGrounded)
                DesiredDirection = transform.forward;
            else if (InputVector == Vector3.zero)
                DesiredDirection = transform.forward;
            else
                DesiredDirection = Vector3.Cross(groundHit.normal, Vector3.Cross(InputVector.normalized, groundHit.normal));
        }
        private void ApplyGravity()
        {
            if (!IsGrounded)
                CharacterRigidbody.AddForce(Vector3.up * -9.8f, ForceMode.Acceleration);
        }

        #endregion
        
        #region Movement Functions
        public void ToggleRun(bool sprint) 
        {
            MoveSpeed = (sprint) ? runSpeed : walkSpeed;           
        }
        private void PhysicsMove()
        {
            desiredSpeed = MoveSpeed * InputVector.magnitude;

            if (desiredSpeed < currentSpeed)
                deceleration = (desiredSpeed < 0.3f) ? deceleration = 2.0f : deceleration = 0.8f;
            else
                deceleration = 1.0f;

            currentSpeed = Mathf.Lerp(currentSpeed, desiredSpeed, acceleration * deceleration *Time.deltaTime);

            Vector3 v = Vector3.zero;
            DesiredVelocity = Vector3.SmoothDamp(DesiredVelocity, DesiredDirection, ref v, rotationSpeed);
            CharacterRigidbody.MovePosition(transform.position + DesiredVelocity * currentSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Physics Based Movement.
        /// </summary>
        /// <param name="direction"></param>
        public void Move(Vector3 direction)
        {
            if (direction.sqrMagnitude > 1)
                direction = direction.normalized;

            InputVector = direction;
        }

        public void Jump(float force) 
        {
            CharacterRigidbody.AddForce(force * Vector3.up, ForceMode.VelocityChange);
        }

        private void GetVelocity()
        {
            Velocity = (CharacterRigidbody.position - lastPosition) * 50;
            lastPosition = CharacterRigidbody.position;
        }

        #endregion

        #region Debug
        void DrawDebugLines()
        {
            if(DesiredDirection != Vector3.zero)
                DebugTools.DrawArrow.ForDebug(transform.position, DesiredDirection, Color.blue);
            if (DesiredVelocity != Vector3.zero)
                DebugTools.DrawArrow.ForDebug(transform.position, DesiredVelocity, Color.yellow);
            if (Velocity != Vector3.zero)
                DebugTools.DrawArrow.ForDebug(transform.position, Velocity, Color.green);
            if (transform.forward != Vector3.zero)
                DebugTools.DrawArrow.ForDebug(transform.position, transform.forward, Color.red);
        }
        #endregion

    }
}
