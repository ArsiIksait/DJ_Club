using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace MusicAndAudios
{
    public class Audios
    {
        public static AudioSource L_Audio;
        public static AudioSource R_Audio;
        public static List<string> MusicNames;
        public static List<AudioClip> Musics;
        public static Text MusicName;
        public static int state = -1;
        public static bool r1state = false;
        public static bool r2state = false;
        public static bool r3state = false;
        public static bool r4state = false;
        public static bool r5state = false;

        public static void CutMusic()
        {
            L_Audio.Stop();
            R_Audio.Stop();
            RandomPlay();
        }
        public static void RandomPlay()
        {
            try
            {
                int MusicNum = Musics.Count;
                if (MusicNum > 0)
                {
                    if (!L_Audio.isPlaying)
                    {
                        if (!R_Audio.isPlaying)
                        {
                            PlayMusic(UnityEngine.Random.Range(0, MusicNum));
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("随机播放音乐时出错! 没有任何可以播放的音乐");
                }
            }
            catch (Exception ex) {
                Debug.LogError("随机播放音乐失败: "+ex);
            }
        }
        public static void PlayMusic(int Index)
        {
            try
            {
                AudioClip Music = GetMusic(Musics, Index);
                if (Music != null)
                {
                    L_Audio.clip = Music;
                    R_Audio.clip = Music;
                    L_Audio.Play();
                    R_Audio.Play();
                    MusicName.text = $"正在播放: {Music.name} (歌曲ID: {Index})";
                }
            }
            catch (Exception ex) {
                Debug.LogError("音乐播放错误: "+ex);
            }
        }

        public static AudioClip GetMusic(List<AudioClip> Musics, int Index)
        {
            AudioClip Music = null;
            int Count = 0;
            foreach (var item in Musics)
            {
                if (Count == Index)
                    Music = item;
                Count++;
            }
            return Music;
        }
    }
}
