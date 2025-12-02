using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    
    float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        timerText.text = $"time : {timer}";
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
