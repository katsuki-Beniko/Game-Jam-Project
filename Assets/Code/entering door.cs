using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class EnteringDoor : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction startAction;
    private bool isAtDoor = false;
    public string sceneToLoad;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component is missing!");
            return;
        }

        startAction = playerInput.actions["interact"];
        if (startAction == null)
        {
            Debug.LogError("Interact action not found!");
        }
    }

    private void OnEnable()
    {
        if (startAction != null)
        {
            startAction.performed += OnStartAction;
        }
    }

    private void OnDisable()
    {
        if (startAction != null)
        {
            startAction.performed -= OnStartAction;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("door"))
        {
            isAtDoor = true; // The player is at the door
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("door"))
        {
            isAtDoor = false; // The player is no longer at the door
        }
    }

    private void OnStartAction(InputAction.CallbackContext context)
    {
        if (isAtDoor)
        {
            SceneManager.LoadScene(sceneToLoad); // Load the specified scene
        }
    }
}

