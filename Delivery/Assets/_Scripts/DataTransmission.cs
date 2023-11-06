using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;

public class DataTransmission : MonoBehaviour
{
    uint currentUserId;
    uint currentSessionId;
    uint currentPurchaseId;

    private void OnEnable()
    {
        Simulator.OnNewPlayer += HandleNewPlayer;
        Simulator.OnNewSession += HandleNewSession;
        Simulator.OnEndSession += HandleEndSession;
        Simulator.OnBuyItem += HandleBuyItem;
    }
    private void OnDisable()
    {
        Simulator.OnNewPlayer -= HandleNewPlayer;
        Simulator.OnNewSession -= HandleNewSession;
        Simulator.OnEndSession -= HandleEndSession;
        Simulator.OnBuyItem -= HandleBuyItem;
    }

    private void HandleNewPlayer(string name, string country, DateTime date)
    {
        StartCoroutine(UploadPlayer(name, country, date));
    }
    private void HandleBuyItem(int item, DateTime date)
    {
        StartCoroutine(UploadItem(item, date));
    }
    private void HandleNewSession(DateTime date)
    {
        StartCoroutine(UploadSession(date, false));
    }
    private void HandleEndSession(DateTime date)
    {
        StartCoroutine(UploadSession(date, true));
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
            UnityEngine.Debug.Log("Player data uploaded successfully.");
            uint.TryParse(www.downloadHandler.text, out currentUserId);
            CallbackEvents.OnAddPlayerCallback.Invoke(currentUserId);
        }
    }
    IEnumerator UploadSession(DateTime date, bool sessionStart)
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www;

        if (!sessionStart)
        {
            form.AddField("User_ID", currentUserId.ToString());
            form.AddField("Start_Session", date.ToString("yyyy-MM-dd HH:mm:ss"));

            www = UnityWebRequest.Post("https://citmalumnes.upc.es/~hangx/Session_Data.php", form);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                UnityEngine.Debug.Log(www.error);
            }
            else
            {
                UnityEngine.Debug.Log("Session data uploaded successfully.");
                uint.TryParse(www.downloadHandler.text, out currentSessionId);
                CallbackEvents.OnNewSessionCallback.Invoke(currentSessionId);
            }
        }
        else
        {
            form.AddField("User_ID", currentUserId.ToString());
            form.AddField("End_Session", date.ToString("yyyy-MM-dd HH:mm:ss"));
            form.AddField("Session_ID", currentSessionId.ToString());

            www = UnityWebRequest.Post("https://citmalumnes.upc.es/~hangx/Close_Session_Data.php", form);

            yield return www.SendWebRequest();          
        }
    }
    IEnumerator UploadItem(int item, DateTime date)
    {
        WWWForm form = new WWWForm();
        form.AddField("Item", item);
        form.AddField("User_ID", currentUserId.ToString());
        form.AddField("Session_ID", currentSessionId.ToString());
        form.AddField("Buy_Date", date.ToString("yyyy-MM-dd HH:mm:ss"));

        UnityWebRequest www = UnityWebRequest.Post("https://citmalumnes.upc.es/~hangx/Purchase_Data.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.Log(www.error);
        }
        else
        {
            UnityEngine.Debug.Log("Purchase data uploaded successfully.");          
            uint.TryParse(www.downloadHandler.text, out currentPurchaseId);
            CallbackEvents.OnItemBuyCallback.Invoke();
        }
    }
}
