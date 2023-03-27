using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEditor.PackageManager.UI;
using Unity.VisualScripting;
using Newtonsoft.Json;

public class StockFish : MonoBehaviour
{
    class Response
    {
        public string platform { get; set; }
        public string best_move { get; set; }
        public string fen { get; set; }
    }

    public static IEnumerator GetBestMove(string forsythEdwardsNotationString)
    {
        yield return new WaitForSeconds(0);
        var request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:5000");

        var postData = $"{{\n\"fen\": \"{forsythEdwardsNotationString}\"\n}}";
        var data = Encoding.ASCII.GetBytes(postData);

        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        var response = (HttpWebResponse)request.GetResponse();
        var json = new StreamReader(response.GetResponseStream()).ReadToEnd();

        Response parsed = JsonConvert.DeserializeObject<Response>(json);
    }
}
