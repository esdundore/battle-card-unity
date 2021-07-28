using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Net;
using System;

public class RESTTemplate {

    public static IEnumerator POSTRequestRoutine(string url, string json)
    {
        var www = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return www.SendWebRequest();
        yield return www.downloadHandler.text;
        Debug.Log(www.downloadHandler.text);
    }

    public static IEnumerator GETRequestRoutine(string url)
    {
        var www = new UnityWebRequest(url, "GET");
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        //Send the request then wait here until it returns
        yield return www.SendWebRequest();
        yield return www.downloadHandler.text;
        Debug.Log(www.downloadHandler.text);
    }

    public static string POSTRequest(string url, string requestBody)
    {
        // Create a request using a URL that can receive a post.
        WebRequest request = WebRequest.Create(url);
        // Set the Method property of the request to POST.
        request.Method = "POST";

        // Create POST data and convert it to a byte array.
        byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);

        // Set the ContentType property of the WebRequest.
        request.ContentType = "application/json";
        // Set the ContentLength property of the WebRequest.
        request.ContentLength = byteArray.Length;

        // Get the request stream.
        Stream dataStream = request.GetRequestStream();
        // Write the data to the request stream.
        dataStream.Write(byteArray, 0, byteArray.Length);
        // Close the Stream object.
        dataStream.Close();

        // Get the response.
        WebResponse response = request.GetResponse();
        // Display the status.
        Console.WriteLine(((HttpWebResponse)response).StatusDescription);

        // Get the stream containing content returned by the server.
        // The using block ensures the stream is automatically closed.
        string responseFromServer = null;
        using (dataStream = response.GetResponseStream())
        {
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            responseFromServer = reader.ReadToEnd();
            // Display the content.
            Debug.Log(responseFromServer);
        }

        // Close the response.
        response.Close();
        return responseFromServer;
    }

}
