using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class TextCommand
{
    public string command;
}

public class TextCommandFetcher : MonoBehaviour
{
    public string url = "https://siam37777773.pythonanywhere.com/get_commands"; 
    public float fetchInterval = 5.0f; 
    void Start()
    {
        StartCoroutine(FetchTextCommands());
    }

    IEnumerator FetchTextCommands()
    {
        while (true)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching text commands: " + www.error);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
                TextCommand textCommand = JsonUtility.FromJson<TextCommand>(jsonResponse);
                Debug.Log("Received command: " + textCommand.command);
            }

            yield return new WaitForSeconds(fetchInterval);
        }
    }
}
