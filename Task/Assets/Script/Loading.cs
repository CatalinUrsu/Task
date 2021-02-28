using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] Slider ProgressSlider;

    AsyncOperation LoadingOperation;
    float Progress = 0;

    void Awake() => StartCoroutine(LoadingAsync());

    //Loading MainScene, check history and set the loading slider depend by this values----------------------------------------------------------------------------------
    IEnumerator LoadingAsync()
    {
        LoadingOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        while (!LoadingOperation.isDone)
            yield return null;

        while (Progress < 1)
        {
            Progress = (LoadingOperation.progress + GameManager.Inst.GetHistoryCheck()) / 2;
            ProgressSlider.value = Mathf.Clamp01(Progress);

            yield return null;
        }

        SceneManager.UnloadScene(0);
    }
}