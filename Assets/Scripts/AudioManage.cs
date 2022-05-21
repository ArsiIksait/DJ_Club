using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using MusicAndAudios;

public class AudioManage : MonoBehaviour
{
    public AudioSource L_Audio;
    public AudioSource R_Audio;
    public Text MusicName;
    public GameObject Fogs;
    public GameObject Fires;
    public GameObject Fire_Boxs;
    public List<AudioClip> Musics;
    public List<string> MusicNames;
    // Start is called before the first frame update
    void Start()
    {
        Audios.L_Audio = L_Audio;
        Audios.R_Audio = R_Audio;
        Audios.MusicName = MusicName;
        Audios.MusicNames = MusicNames;
        Audios.Musics = Musics;
        string aaa="";
        int i = 0;
        foreach (var item in Musics)
        {
            Audios.MusicNames.Add($"{item.name}");
            aaa = $"{aaa}\r\n{item.name} : {i}";
            i++;
        }
        //Debug.Log(aaa);
        InvokeRepeating("RandomPlay", 0, 2);
        //InvokeRepeating("MakeFogs", 0, 15);
        //InvokeRepeating("MakeFire", 0, 25);
        //InvokeRepeating("MakeFireBox", 0, 60);
        //InvokeRepeating("MakeAll", 0, 120);
    }
    void Update()
    {
        if (Audios.state != -1)
        {
            Audios.PlayMusic(Audios.state);
            Audios.state = -1;
        }
        if (Audios.r1state)
        {
            Audios.CutMusic();
            Audios.r1state = false;
        }
        if (Audios.r2state)
        {
            //MakeFogs();
            Audios.r2state = false;
        }
        if (Audios.r3state)
        {
            //MakeFire();
            Audios.r3state = false;
        }
        if (Audios.r4state)
        {
            //MakeFireBox();
            Audios.r4state = false;
        }
        if (Audios.r5state)
        {
            //MakeAll();
            Audios.r5state = false;
        }
    }

    void RandomPlay()
    {
        Audios.RandomPlay();
    }

    void MakeFogs()
    {
        Fogs.SetActive(false);
        Fogs.SetActive(true);
    }
    void MakeFire()
    {
        Fires.SetActive(false);
        Fires.SetActive(true);
    }
    void MakeFireBox()
    {
        Fire_Boxs.SetActive(false);
        Fire_Boxs.SetActive(true);
    }
    void MakeAll()
    {
        Fogs.SetActive(false);
        Fogs.SetActive(true);
        Fires.SetActive(false);
        Fires.SetActive(true);
        Fire_Boxs.SetActive(false);
        Fire_Boxs.SetActive(true);
    }
}
