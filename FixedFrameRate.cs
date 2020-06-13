using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class FixedFrameRate : MonoBehaviour
{
    public float frameRate = 60.0f; //目標フレームレート
    float currentFrameTime;

    static FixedFrameRate _instance;
    public static FixedFrameRate Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gameObject = new GameObject("ForceFrameRate");
                _instance = gameObject.AddComponent<FixedFrameRate>();
                gameObject.AddComponent<FPSCounter>(); //必要なければこの行は削除
                DontDestroyOnLoad(gameObject);
            }
            return _instance;
        }
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadInstance()
    {
        _instance = Instance;
    }

    void Awake() 
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 9999;
        currentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine("WaitForNextFrame");
    }


    IEnumerator WaitForNextFrame() 
    {
        while (true) 
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / frameRate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0) Thread.Sleep((int)(sleepTime * 1000));
            while (t < currentFrameTime) t = Time.realtimeSinceStartup;
        }
    }
}
