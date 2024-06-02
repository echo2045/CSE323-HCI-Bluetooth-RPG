using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TextReceiver : MonoBehaviour
{
    private string url = "https://speechtotext.free.beeceptor.com/";

    void Start()
    {
        StartCoroutine(GetTextFromWebhook());
    }

    IEnumerator GetTextFromWebhook()
    {
        while (true)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                // Extract the text data from the response
                string textData = www.downloadHandler.text;
                Debug.Log("Received text: " + textData);
            }

            // Wait for a short period before polling again
            yield return new WaitForSeconds(1f); // Adjust the polling interval as needed
        }
    }
}
