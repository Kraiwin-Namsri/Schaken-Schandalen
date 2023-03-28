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
using System.Xml.Linq;

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

        BestMoveToCoordinates(parsed.best_move);
    }

    public static void BestMoveToCoordinates(string response)
    {
        int xCordStart = 0;
        int xCordEnd = 0;
        int yCordStart = 0;
        int yCordEnd = 0;
        bool startCordPassed = false;

        Char[] letters = response.ToCharArray();

        foreach(char letter in letters)
        {
            switch (letter.ToString())
            {
                case "a":
                    if (startCordPassed == false)
                    {
                        xCordStart = 1;
                    }
                    else
                    {
                        xCordEnd= 1;
                    }
                    break;
                case "b":
                    if (startCordPassed == false)
                    {
                        xCordStart = 2;
                    }
                    else
                    {
                        xCordEnd = 2;
                    }
                    break;
                case "c":
                    if (startCordPassed == false)
                    {
                        xCordStart = 3;
                    }
                    else
                    {
                        xCordEnd = 3;
                    }
                    break;
                case "d":
                    if (startCordPassed == false)
                    {
                        xCordStart = 4;
                    }
                    else
                    {
                        xCordEnd = 4;
                    }
                    break;
                case "e":
                    if (startCordPassed == false)
                    {
                        xCordStart = 5;
                    }
                    else
                    {
                        xCordEnd = 5;
                    }
                    break;
                case "f":
                    if (startCordPassed == false)
                    {
                        xCordStart = 6;
                    }
                    else
                    {
                        xCordEnd = 6;
                    }
                    break;
                case "g":
                    if (startCordPassed == false)
                    {
                        xCordStart = 7;
                    }
                    else
                    {
                        xCordEnd = 7;
                    }
                    break;
                case "h":
                    if (startCordPassed == false)
                    {
                        xCordStart = 8;
                    }
                    else
                    {
                        xCordEnd = 8;
                    }
                    break;
                default:
                    if (startCordPassed == false)
                    {
                        yCordStart = letter - '0';
                    }
                    else
                    {
                        yCordEnd = letter - '0';
                    }
                        break;
            }
            startCordPassed = true;
            Vector2 startCoordinates = new Vector2(xCordStart, yCordStart);
            Vector2 endCoordinates = new Vector2(xCordEnd, yCordEnd);
        }
    }
}
