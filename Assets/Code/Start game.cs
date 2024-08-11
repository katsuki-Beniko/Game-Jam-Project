using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Startgame : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction startAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component is missing!");
            return;
        }

        startAction = playerInput.actions["Start game"];
        if (startAction == null)
        {
            Debug.LogError("Start game action not found!");
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

    private void OnStartAction(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("First Cutscene");
    }
}
