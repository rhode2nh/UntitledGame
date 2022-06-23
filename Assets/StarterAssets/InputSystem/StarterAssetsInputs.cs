using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool checkInteractable;

		[Header("Movement Settings")]
		public bool analogMovement;

		private PlayerInput playerInput;
		private InventoryUIController inventoryUIController;
		private FirstPersonController firstPersonController;
		public DeveloperConsoleBehavior developerConsole;
        private EquipmentContainer equipmentContainer;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif
		
        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            inventoryUIController = GetComponent<InventoryUIController>();
			firstPersonController = GetComponent<FirstPersonController>();
            equipmentContainer = GetComponent<EquipmentContainer>();
        }

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnInteract(InputValue value)
        {
			InteractInput(value.isPressed);
        }

		public void OnOpenInventory(InputValue value)
        {
			OpenInventoryInput(value.isPressed);
        }

		public void OnOpenConsole(InputValue value)
        {
			OpenConsoleInput();
        }

        #region Inventory Action Map Functions
        public void OnCloseInventory(InputValue value)
        {
			CloseInventoryInput(value.isPressed);
        }
        #endregion

        #region Console Action Map Functions
		public void OnCloseConsole(InputValue value)
        {
			CloseConsoleInput();
        }

		public void OnPreviousCommand(InputValue value)
        {
			PreviousCommandInput();
        }

		public void OnNextCommand(InputValue value)
        {
			NextCommandInput();	
        }

        public void OnSwitchEquipment(InputValue value)
        {
            SwitchEquipment(0);
        }

        public void OnSwitchEquipment1(InputValue value)
        {
            SwitchEquipment(1);
        }

        public void OnSwitchEquipment2(InputValue value)
        {
            SwitchEquipment(2);
        }

        public void OnSwitchEquipment3(InputValue value)
        {
            SwitchEquipment(3);
        }

        public void OnAttack(InputValue value)
        {
            Attack(value.isPressed);
        }
        #endregion
#else
		// old input sys if we do decide to have it (most likely wont)...
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void InteractInput(bool newInteractableState)
        {
			firstPersonController.HandleInteractable();
        }

		public void OpenInventoryInput(bool openInventoryState)
        {
			if (openInventoryState)
            {
				playerInput.currentActionMap = playerInput.actions.FindActionMap("Inventory");
				inventoryUIController.OpenInventory();
            }
        }

		public void CloseInventoryInput(bool closeInventoryState)
        {
			if (closeInventoryState)
            {
				playerInput.currentActionMap = playerInput.actions.FindActionMap("Player");
				inventoryUIController.CloseInventory();
            }
        }

		public void OpenConsoleInput()
        {
			Time.timeScale = 0.0f;
			playerInput.currentActionMap = playerInput.actions.FindActionMap("Console");
			developerConsole.Toggle();
        }

		public void CloseConsoleInput()
        {
			Time.timeScale = 1.0f;
			playerInput.currentActionMap = playerInput.actions.FindActionMap("Player");
			developerConsole.Toggle();
        }

		public void NextCommandInput()
        {
			developerConsole.NextCommand();
        }

		public void PreviousCommandInput()
        {
			developerConsole.PreviousCommand();
        }

        public void SwitchEquipment(int index)
        {
            equipmentContainer.SwitchEquipment(index);
        }

        public void Attack(bool isPressed)
        {
            equipmentContainer.setIsAttacking(isPressed);
        }

#if !UNITY_IOS || !UNITY_ANDROID

        private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif

	}
	
}
