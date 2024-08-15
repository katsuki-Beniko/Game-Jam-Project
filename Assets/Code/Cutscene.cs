using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;  // Required for RawImage and legacy Text
//using UnityEditor;  // Required for AssetDatabase usage
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    [Header("Input Settings")]
    public InputActionReference startAction;  // Reference to the input action for progressing the cutscene

    [Header("Cutscene Settings")]
    public int[] textsPerPicture;             // Number of texts needed to pass before switching to each picture
    public string[] dialogTexts;              // Array of all dialog texts in the cutscene
    public string scenetogo;

    [Header("Effect Targets")]
    public RawImage targetPictureObject;      // The specific RawImage to apply picture effects to
    public Text targetTextObject;             // The specific legacy Text to apply text effects to

    [SerializeField] private Texture2D[] pictures;             // Array to hold the loaded pictures
    private int currentPictureIndex = 0;      // Index of the current picture being displayed
    private int currentTextIndex = 0;         // Index of the current dialog text being displayed
    private int textsPassed = 0;              // Number of texts passed for the current picture

    private void Awake()
    {
        //LoadPictures();

        if (startAction != null)
        {
            startAction.action.Enable();
            startAction.action.performed += OnStartAction;
        }

        UpdateCutscene();  // Initialize the cutscene
    }

    private void OnDestroy()
    {
        if (startAction != null)
        {
            startAction.action.performed -= OnStartAction;
        }
    }

    private void LoadPictures()
    {
        pictures = new Texture2D[pictures.Length];

        for (int i = 0; i < pictures.Length; i++)
        {
            // Load the texture from the "Assets/frame" folder using AssetDatabase
            string assetPath = "/frame/" + pictures[i];
            //Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            Texture2D texture = Resources.Load<Texture2D>(assetPath);

            if (texture == null)
            {
                Debug.LogError("Could not find picture: " + pictures[i]);
                continue;
            }

            pictures[i] = texture;
        }
    }

    private void OnStartAction(InputAction.CallbackContext context)
    {
        // Progress to the next text
        textsPassed++;
        currentTextIndex++;

        // Check if all text has been displayed
        if (currentTextIndex >= dialogTexts.Length)
        {
            EndCutscene();
            return;
        }

        // Check if it's time to switch to the next picture
        if (textsPassed >= textsPerPicture[currentPictureIndex])
        {
            textsPassed = 0;  // Reset the text count for the next picture
            currentPictureIndex++;

            // Ensure we don't go out of bounds for pictures
            if (currentPictureIndex >= pictures.Length)
            {
                currentPictureIndex = pictures.Length - 1;  // Stay on the last picture
            }
        }

        // Update the cutscene visuals and text
        UpdateCutscene();
    }



    private void UpdateCutscene()
    {
        // Display the current picture in the RawImage
        if (currentPictureIndex < pictures.Length && targetPictureObject != null)
        {
            targetPictureObject.texture = pictures[currentPictureIndex];
            ApplyPictureEffect(targetPictureObject);
        }

        // Display the current dialog text
        if (currentTextIndex < dialogTexts.Length && targetTextObject != null)
        {
            targetTextObject.text = dialogTexts[currentTextIndex];
            ApplyTextEffect(targetTextObject);
        }
    }

    private void ApplyPictureEffect(RawImage pictureObject)
    {
        // Implement your picture effect logic here
        Debug.Log($"Applying effect to picture object: {pictureObject.name}");
    }

    private void ApplyTextEffect(Text textObject)
    {
        // Implement your text effect logic here
        Debug.Log($"Applying effect to text object: {textObject.name}");
    }

    private void EndCutscene()
    {
        Debug.Log("Cutscene Ended");
        SceneManager.LoadScene(scenetogo);
    }

}
