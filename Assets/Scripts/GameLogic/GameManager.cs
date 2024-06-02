using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int chestsOpened = 0;
    public int totalChests = 3;
    public Text chestCounterText;

    private void Awake()
    {
        // Ensure only one instance of the GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChestOpened()
    {
        chestsOpened++;
        UpdateChestCounterUI();
    }

    private void UpdateChestCounterUI()
    {
        chestCounterText.text = $"Chests: {chestsOpened}/{totalChests}";
    }
}
