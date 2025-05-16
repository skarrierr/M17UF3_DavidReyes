using UnityEngine;
using UnityEngine.UI;

public class RelayUI : MonoBehaviour
{
    [Header("References")]
    public RelayManager relayManager;
    public Button createButton;
    public Button joinButton;
    public InputField joinInput;


    [Header("Hotkeys")]
    [Tooltip("Key to press for creating a relay session")]
    public KeyCode createKey;
    [Tooltip("Key to press for joining a relay session")]
    public KeyCode joinKey;

    void Start()
    {
        createButton.onClick.AddListener(() => relayManager.CreateRelay());
        joinButton.onClick.AddListener(TryJoin);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        // Hotkey for creating a relay session
        if (Input.GetKeyDown(createKey))
        {
            relayManager.CreateRelay();
            Debug.Log($"[RelayUI] Hotkey '{createKey}' pressed: CreateRelay called.");
        }

        // Hotkey for joining a relay session
        if (Input.GetKeyDown(joinKey))
        {
            TryJoin();
            Debug.Log($"[RelayUI] Hotkey '{joinKey}' pressed: JoinRelay called with code '{joinInput.text}'.");
        }
    }

    private void TryJoin()
    {
        string code = joinInput.text.Trim();
        if (!string.IsNullOrEmpty(code))
        {
            relayManager.JoinRelay(code);
        }
        else
        {
            Debug.LogWarning("[RelayUI] Join code is empty. Please enter a valid join code.");
        }
    }
}
