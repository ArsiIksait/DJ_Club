using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Time : MonoBehaviour
{
    public Text Clock;
    public Material skybox01;
    public Material skybox02;
    public Material skybox03;
    public Material skybox04;
    public Material skybox05;

    private void Start()
    {
        InvokeRepeating("skyBox", 0, 1);
        InvokeRepeating("time", 0, 0.5f);
    }
    // Update is called once per frame
    void time()
    {
        Clock.text = "北京时间: " + DateTime.Now.ToLongTimeString().ToString();
    }
    void skyBox()
    {
        int hour = DateTime.Now.Hour;
        if (hour >= 6)
            RenderSettings.skybox = skybox01;
        if (hour >= 8)
            RenderSettings.skybox = skybox02;
        if (hour >= 12)
            RenderSettings.skybox = skybox03;
        if (hour >= 18)
            RenderSettings.skybox = skybox04;
        if (hour >= 20)
            RenderSettings.skybox = skybox05;
    }
}
