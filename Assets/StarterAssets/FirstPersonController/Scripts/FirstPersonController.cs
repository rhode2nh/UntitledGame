using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : LifeEntity, IHittable
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float accelerationRate = 10.0f;
        [Tooltip("Tilt speed and angle of the character")]
		public float friction = 10.0f;
		public float airResistance = 1.0f;
        public float tiltSpeed = 7f;
        public float tiltAngle = 5f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		public bool hitCeiling = false;
		bool resetVerticalVelocity = false;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		public float ceilingOffset = 1.0f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;
		public LayerMask CeilingLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTargetParent;
		public GameObject CinemachineCameraTarget;
		public CinemachineVirtualCamera virtualCamera;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _verticalKnockback;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;
		public float velocityFOVScaleFactor;
		public float fovChangeRate;
		private float fov;

		private const float _threshold = 0.01f;

		//--------------------user variables--------------------
		[Tooltip("Show debug info for first person controller function")]
		public bool isDebug = false;
        public InputRaycast _inputRaycast;
		public PlayerStats playerStats;

		private Vector3 _oldPos;
		private float _totalDisance = 0.0f;
        private Quaternion _tiltRotation = Quaternion.identity;
        private Quaternion _initialRotation;
		float lastHorizontalSpeed = 0.0f;
		Vector3 lastInputDirBeforeJump = new Vector3();
		Vector3 lastLookDirBeforeJump = new Vector3();
		Vector3 lastLookDirBeforeShoot = new Vector3();
		Vector3 lastForwardDirBeforeShoot = new Vector3();
		Vector3 inputDirection = new Vector3();
		Vector3 lastInputDir = new Vector3();
		Transform lastTransform;
		Vector3 lastRightDir = new Vector3();
		Vector3 lastForwardDir = new Vector3();
		private bool captureLastInputDir = true;
		Vector3 lerpedInputDir = new Vector3();
		private float dotScalar = 0.0f;
		private float knockback = 0.0f;
		private Vector3 _horizontalKnockbackDir = new Vector3();
		private float lastMoveSpeed = 0.0f;

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
				fov = virtualCamera.m_Lens.FieldOfView;
			}
		}

		private void Start()
		{
            GameEvents.current.onIsPlayerDead += IsPlayerDead;
            GameEvents.current.onHurtPlayer += HurtPlayer;
			GameEvents.current.onSetMouseSense += SetMouseSense;
			GameEvents.current.onGetMouseSense += GetMouseSense;
			GameEvents.current.onGetPlayerHealth += GetPlayerHealth;
			GameEvents.current.onRecoilKnockback += ApplyRecoilKnockback;
            _initialRotation = CinemachineCameraTarget.transform.localRotation;
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;

			// Get start position to calculate distance travelled
			_oldPos = transform.position;
			lastTransform = transform;
		}

		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			CeilingCheck();
			Move();
			CalculateDistanceTravelled();
		}

		private void LateUpdate()
		{
			CameraRotation();
		}

		public void Consume(Item item)
        {
            GameEvents.current.Consume(item);
        }

		public void HandleInteractable()
        {
            if (_inputRaycast.isHitting)
            {
                var interactable = _inputRaycast.hit.transform.gameObject;
                switch (interactable.tag)
                {
                    case Constants.WORLD_ITEM:
                        PickUpItem(interactable.GetComponent<WorldItem>());
                        break;
                    case Constants.CHEST:
                        break;
                    case Constants.BUTTON:
                        interactable.GetComponent<WButton>().Execute();;
                        break;
                    default:
                        Debug.Log("I don't know what to do with this: " + _inputRaycast.hit.transform.gameObject.tag);
                        break;
                }
            }
        }

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}

		private void CeilingCheck() {
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + ceilingOffset, transform.position.z);
			hitCeiling = Physics.CheckSphere(spherePosition, GroundedRadius, CeilingLayers, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				_cinemachineTargetPitch += _input.look.y * RotationSpeed * 0.01f;
				_rotationVelocity = _input.look.x * RotationSpeed * 0.01f;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTargetParent.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private void Move()
		{
			inputDirection = MoveGround();
			_horizontalKnockbackDir = -MoveKnockback() * Time.deltaTime;
			// move the player
			_controller.Move(Vector3.ClampMagnitude(inputDirection, 1f) * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime + _horizontalKnockbackDir);
            float maxSpeed = 100.0f;
            float normalizedSpeed = _controller.velocity.magnitude / maxSpeed;
			virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, fov + fov * Mathf.Pow(normalizedSpeed, 2) * velocityFOVScaleFactor, Time.deltaTime * fovChangeRate), 0.0f, 120.0f);

            Quaternion _targetRotation;
            if (_input.move.x == -1) {
                _targetRotation = Quaternion.Euler(0, 0, tiltAngle) * _initialRotation;
            } else if (_input.move.x == 1) {
                _targetRotation = Quaternion.Euler(0, 0, -tiltAngle) * _initialRotation;
            } else {
                _targetRotation = _initialRotation;
            }
            CinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(CinemachineCameraTarget.transform.localRotation, _targetRotation, tiltSpeed * Time.deltaTime);
		}

		private Vector3 MoveAir() {
			float targetSpeed = 0.0f;
			float decelType = airResistance;
			Vector3 dir = new Vector3();

			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
			_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * airResistance);

			dir = lastLookDirBeforeJump;
			lastHorizontalSpeed = currentHorizontalSpeed;

			return dir;
		}

		private Vector3 MoveKnockback() {
			float frictionType = Grounded ? friction : airResistance;
			lastLookDirBeforeShoot = Vector3.Lerp(lastLookDirBeforeShoot, new Vector3(), Time.deltaTime * frictionType);
			return lastLookDirBeforeShoot;
		}
		
		private Vector3 MoveGround() {
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = Grounded ? (_input.sprint ? SprintSpeed : MoveSpeed) : lastMoveSpeed;
			Vector3 inputDir = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
			float frictionType = Grounded ? friction : airResistance;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move == Vector2.zero && captureLastInputDir) {
				lastRightDir = transform.right;
				lastForwardDir = transform.forward;
				inputDir = lastRightDir * inputDir.x + lastForwardDir * inputDir.z;
				captureLastInputDir = false;
			} else if (_input.move != Vector2.zero) {
				inputDir = transform.right * inputDir.x + transform.forward * inputDir.z;
				captureLastInputDir = true;
				lastInputDir = inputDir;
			} 

			lerpedInputDir = Vector3.Lerp(lerpedInputDir, inputDir, frictionType * Time.deltaTime);

			// a reference to the players current horizontal velocity
			_speed = targetSpeed;
			return lerpedInputDir;
		}

		private void OnControllerColliderHit(ControllerColliderHit controllerColliderHit) {
			lerpedInputDir -= controllerColliderHit.normal * Vector3.Dot(lerpedInputDir, controllerColliderHit.normal);
			lastLookDirBeforeShoot -= controllerColliderHit.normal * Vector3.Dot(lastLookDirBeforeShoot, controllerColliderHit.normal);
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				resetVerticalVelocity = false;
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = 0f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt((playerStats.buffedStats.jumpHeight + JumpHeight) * -2f * Gravity);
					lastLookDirBeforeJump = (transform.right * Vector3.Dot(_controller.velocity, transform.right) + transform.forward * Vector3.Dot(_controller.velocity, transform.forward)).normalized;
					lastMoveSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				if (hitCeiling && !resetVerticalVelocity) {
					_verticalVelocity = 0f;
					resetVerticalVelocity = true;
				} 
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
			if (hitCeiling) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + ceilingOffset, transform.position.z), GroundedRadius);
			Gizmos.color = Color.magenta;
			Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), inputDirection);
			Gizmos.color = Color.yellow;
			Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), _horizontalKnockbackDir);
		}

        public void PickUpItem(WorldItem item)
        {
            Slot invItem = new Slot(item.item, item.count, item.properties);
			if (invItem.item is IWeapon) {
				Debug.Log(((List<Slot>)item.properties[Constants.P_W_MODIFIERS_LIST])[0].item);
			}
            GameEvents.current.AddItemToPlayerInventory(invItem);
            if (isDebug)
            {
                Debug.Log(item.id);
            }
            Destroy(_inputRaycast.hit.transform.gameObject);
        }

		private void CalculateDistanceTravelled()
        {
			Vector3 distanceVector = transform.position - _oldPos;
			float distanceThisFrame = distanceVector.magnitude;
			_totalDisance += distanceThisFrame;
			_oldPos = transform.position;
			playerStats.DistanceTraveled = _totalDisance;
        }

        private void OnApplicationQuit()
        {
            // TODO Rework when save/load system is implemented
            // GameEvents.current.ClearInventory();
        }

        public bool IsPlayerDead()
        {
            return health <= 0 ? true: false;
        }

        public void HurtPlayer(int damage)
        {
            health -= damage;
        }

		public float GetPlayerHealth() { 
			return health;
		}

		public void SetMouseSense(float mouseSens) {
			RotationSpeed = mouseSens;
		}

		public float GetMouseSense() {
			return RotationSpeed;
		}

		public void ModifyHealth(float damage) {
			if (!isInvincible) {
				health -= damage;
				GameEvents.current.UpdateHealth(health);
			}
			if (health <= 0) {
				GameEvents.current.ClearInventory();
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}

		public void ApplyRecoilKnockback(float knockback, Vector3 direction) {
			_verticalVelocity += Vector3.Dot(CinemachineCameraTarget.transform.forward, -transform.up) * knockback;
			resetVerticalVelocity = false;
			var forward = CinemachineCameraTarget.transform.forward;
			lastForwardDirBeforeShoot = transform.forward;
			this.knockback += Vector3.Dot(-lastLookDirBeforeShoot, -lastForwardDirBeforeShoot) * knockback;
			lastLookDirBeforeShoot += new Vector3(forward.x, 0.0f, forward.z) * knockback;
		}
	}
}
