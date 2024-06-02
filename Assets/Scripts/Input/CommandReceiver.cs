using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CommandReceiver : MonoBehaviour
{
    private string serverUrl = "https://siam37777773.pythonanywhere.com/get_commands";  // Changed to HTTPS

    private Rigidbody2D rb;
    private Animator anim;
    public float MovementSpeed;
    private Vector2 MovementInput;
    public bool canOpen;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canOpen = false;
    }

    void Start()
    {
        // Start the coroutine to get commands from the server
        StartCoroutine(GetCommandsFromServer());
    }

    IEnumerator GetCommandsFromServer()
    {
        while (true)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(serverUrl))
            {
                // Send the request and wait for a response
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                }
                else
                {
                    // Get the JSON response
                    string jsonResponse = webRequest.downloadHandler.text;

                    // Parse the JSON response
                    CommandResponse commandResponse = JsonUtility.FromJson<CommandResponse>(jsonResponse);

                    if (commandResponse != null && !string.IsNullOrEmpty(commandResponse.command))
                    {
                        Debug.Log("Received Command: " + commandResponse.command);
                      
                        HandleCommand(commandResponse.command);
                    }
                    else
                    {
                        Debug.LogWarning("Received an empty or malformed command.");
                    }
                }
            }

            // Wait for a short time before making the next request
            yield return new WaitForSeconds(1f);
        }
    }

    private void MoveForward()
    {
        MovementInput = new Vector2(0, 1);
        Animate();
        rb.velocity = MovementInput * MovementSpeed * Time.fixedDeltaTime;
    }

    private void MoveBackward()
    {
        MovementInput = new Vector2(0, -1);
        Animate();
        rb.velocity = MovementInput * MovementSpeed * Time.fixedDeltaTime;
    }

    private void MoveLeft()
    {
        MovementInput = new Vector2(-1, 0);
        Animate();
        rb.velocity = MovementInput * MovementSpeed * Time.fixedDeltaTime;
    }

    private void MoveRight()
    {
        MovementInput = new Vector2(1, 0);
        Animate();
        rb.velocity = MovementInput * MovementSpeed * Time.fixedDeltaTime;
    }

    private void StopMoving()
    {
        MovementInput = new Vector2(0, 0);
        Animate();
        rb.velocity = MovementInput * MovementSpeed * Time.fixedDeltaTime;
    }
    private void OpenChest()
    {
        canOpen = true;
        StartCoroutine(SetCanOpenFalseAfterDelay(5f));

    }
    private IEnumerator SetCanOpenFalseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canOpen = false;
    }

    private void Animate()
    {
        anim.SetFloat("MovementX", MovementInput.x);
        anim.SetFloat("MovementY", MovementInput.y);
    }

    void HandleCommand(string command)
    {
        // Implement your command handling logic here
        // For example, move a game object based on the command
        switch (command)
        {
            case "forward":
                // Move forward logic
                //Debug.Log("Moving forward...");
                MoveForward();
                break;
            case "backward":
                // Move backward logic
                //Debug.Log("Moving backward...");
                MoveBackward();
                break;
            case "left":
                // Move left logic
                //Debug.Log("Moving left...");
                MoveLeft();
                break;
            case "right":
                // Move right logic
                MoveRight();
                //Debug.Log("Moving right...");
                break;
            case "stop":
                StopMoving();
                break;
            case "open":
                OpenChest();
                break;
            default:
                Debug.LogWarning("Unknown command: " + command);
                break;
        }
    }

    [System.Serializable]
    private class CommandResponse
    {
        public string command;
    }
}
