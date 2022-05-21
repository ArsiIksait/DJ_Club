using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    public int s = 10;
    private int ss = 10;
    public bool loaded = false;
    public Text progress;
    private float progressValue;
    [Tooltip("下个场景的名字")]
    public string nextSceneName;
    private AsyncOperation async = null;
    // Start is called before the first frame update
    void Start()
    {
        ss = s;
        StartCoroutine("LoadScene");
        InvokeRepeating("loadeds", 0, 1);
    }
    void loadeds()
    {
        if (loaded)
            ss--;
    }
    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(nextSceneName);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            if (async.progress < 0.9f)
                progressValue = async.progress;
else
                progressValue = 1.0f;

            progress.text = $"正在加载场景中 请稍后... {(int)(progressValue * 100)}%";

            if (progressValue >= 0.9)
            {
                loaded = true;
                progress.text = $"加载完毕 {s}秒后跳转到场景 ({ss}s)";
                if (ss == 0)
                {
                    async.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
