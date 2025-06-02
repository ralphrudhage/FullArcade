using System;
using System.Collections;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private GameObject arcadeCanvas;
    private NarrativeWriter narrativeWriter;
    private ArcadeSceneFader arcadeSceneFader;

    private void OnEnable()
    {
        arcadeCanvas.SetActive(true);
        arcadeSceneFader = FindAnyObjectByType<ArcadeSceneFader>();
        narrativeWriter = FindAnyObjectByType<NarrativeWriter>();
    }
    
    private void Start()
    {
        JujimufuScene();
    }

    private void JujimufuScene()
    {
        arcadeSceneFader.BlackScene();

        StartCoroutine(DelayedAction(3f,
            () => narrativeWriter.TextRow1("Meanwhile", "", 3f, GameUtils.lightYellow)));
        StartCoroutine(DelayedAction(8f, () => arcadeSceneFader.FadeIn()));
        StartCoroutine(DelayedAction(8f, () => SoundManager.Instance.PlayMusic()));
    }
    
    private IEnumerator DelayedAction(float delay, Action method)
    {
        yield return new WaitForSeconds(delay);
        method.Invoke();
    }
}