using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebhookTextFetcher : MonoBehaviour
{
    private string url = "https://webhook.site/c91483c0-05cc-40f0-8b8c-68285d0de81a"; // Webhook URL

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
                // Get the entire HTML content
                string htmlContent = www.downloadHandler.text;

                // Extract the text data
                string data = ExtractTextData(htmlContent);
                Debug.Log("Received data: " + data);
            }

            // Wait for a short period before polling again
            yield return new WaitForSeconds(1f); // Adjust the polling interval as needed
        }
    }

    string ExtractTextData(string htmlContent)
    {
        // Manually extract the text data from HTML content
        // You may need to adjust this based on the actual structure of the HTML content
        // For example, if the text is wrapped in a specific tag, you would need to extract it accordingly
        string textStartTag = "<h1>"; // Example: Assuming the text is wrapped in an <h1> tag
        string textEndTag = "</h1>";   // Example: Assuming the text is wrapped in an <h1> tag

        int startIndex = htmlContent.IndexOf(textStartTag);
        int endIndex = htmlContent.IndexOf(textEndTag);

        if (startIndex != -1 && endIndex != -1)
        {
            // Extract the text between the start and end index
            string extractedText = htmlContent.Substring(startIndex + textStartTag.Length, endIndex - startIndex - textStartTag.Length);
            return extractedText;
        }
        else
        {
            Debug.LogWarning("Failed to extract text data from HTML.");
            return string.Empty;
        }
    }
}
