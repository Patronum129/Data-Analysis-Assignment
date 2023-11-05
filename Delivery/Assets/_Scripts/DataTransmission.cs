using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;

public class DataTransmission : MonoBehaviour
{
    uint userId;
    uint sessionId;
    uint purchaseId;

    private void OnEnable()
    {
        Simulator.OnNewPlayer += SendPlayer;
        Simulator.OnNewSession += SendSessionStart;
        Simulator.OnEndSession += SendSessionClosing;
        Simulator.OnBuyItem += SendBuyItem;
    }
    private void OnDisable()
    {
        Simulator.OnNewPlayer -= SendPlayer;
        Simulator.OnNewSession -= SendSessionStart;
        Simulator.OnEndSession -= SendSessionClosing;
        Simulator.OnBuyItem -= SendBuyItem;
    }

    private void SendPlayer(string name, string country, DateTime date)
    {
        StartCoroutine(UploadPlayer(name, country, date));
    }
    private void SendBuyItem(int item, DateTime date)
    {
        StartCoroutine(UploadItem(item, date));
    }
    private void SendSessionStart(DateTime date)
    {
        StartCoroutine(UploadSession(date, false));
    }
    private void SendSessionClosing(DateTime date)
    {
        StartCoroutine(UploadSession(date, true));
    }

    void Start()
    {

    }

    private void Update()
    {

    }

    IEnumerator UploadPlayer(string name, string country, DateTime date)
    {
        WWWForm form = new WWWForm();

        form.AddField("Name", name);
        form.AddField("Country", country);
        form.AddField("Date", date.ToString("yyyy-MM-dd HH:mm:ss"));


        UnityWebRequest www = UnityWebRequest.Post("https://citmalumnes.upc.es/~hangx/Player_Data.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.Log(www.error);
        }
        else
        {
            UnityEngine.Debug.Log("Player form upload complete!");
            //Debug.Log(www.downloadHandler.text);


            uint.TryParse(www.downloadHandler.text, out userId);
            CallbackEvents.OnAddPlayerCallback.Invoke(userId);

        }
    }
    IEnumerator UploadSession(DateTime uploadedDate, bool sessionInProgress)
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www;


        if (!sessionInProgress)
        {
            form.AddField("User_ID", userId.ToString());
            form.AddField("Start_Session", uploadedDate.ToString("yyyy-MM-dd HH:mm:ss"));



            www = UnityWebRequest.Post("https://citmalumnes.upc.es/~hangx/Session_Data.php", form);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                UnityEngine.Debug.Log(www.error);
            }
            else
            {
                UnityEngine.Debug.Log("Sessions form upload complete!");
                //Debug.Log(www.downloadHandler.text);
                uint.TryParse(www.downloadHandler.text, out sessionId);

                CallbackEvents.OnNewSessionCallback.Invoke(sessionId);

            }
        }
        else
        {
            form.AddField("User_ID", userId.ToString());
            form.AddField("End_Session", uploadedDate.ToString("yyyy-MM-dd HH:mm:ss"));
            form.AddField("Session_ID", sessionId.ToString());


            www = UnityWebRequest.Post("https://citmalumnes.upc.es/~hangx/Close_Session_Data.php", form);



            yield return www.SendWebRequest();

            //Debug.Log(www.downloadHandler.text);
        }







    }
    IEnumerator UploadItem(int item, DateTime date)
    {
        WWWForm form = new WWWForm();
        form.AddField("Item", item);
        form.AddField("User_ID", userId.ToString());
        form.AddField("Session_ID", sessionId.ToString());
        form.AddField("Buy_Date", date.ToString("yyyy-MM-dd HH:mm:ss"));


        UnityWebRequest www = UnityWebRequest.Post("https://citmalumnes.upc.es/~hangx/Purchase_Data.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.Log(www.error);
        }
        else
        {
            UnityEngine.Debug.Log("Buy form upload complete!");
            //Debug.Log(www.downloadHandler.text);

            uint.TryParse(www.downloadHandler.text, out purchaseId);

            CallbackEvents.OnItemBuyCallback.Invoke();

        }
    }
}
