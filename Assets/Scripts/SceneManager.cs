using System;
using System.Collections;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private NarrativeWriter narrativeWriter;
    private ArcadeSceneFader arcadeSceneFader;

    private void OnEnable()
    {
        arcadeSceneFader = FindAnyObjectByType<ArcadeSceneFader>();
        narrativeWriter = FindAnyObjectByType<NarrativeWriter>();
    }
    
    private void Start()
    {
        Act1CutScene();
        // Act1GamePlay(Vector3.zero);
    }

    private void Act1CutScene()
    {
        arcadeSceneFader.BlackScene();

        StartCoroutine(DelayedAction(3f,
            () => narrativeWriter.TextRow1("Meanwhile", "", 3f, GameUtils.lightYellow)));
        StartCoroutine(DelayedAction(8f, () => arcadeSceneFader.FadeIn()));
    }

    public void SkipCutScene()
    {
        narrativeWriter.Skip();
        StopAllCoroutines();
        
        Act1GamePlay();
    }

    private void Act1GamePlay()
    {
    }

    private IEnumerator DelayedAction(float delay, Action method)
    {
        yield return new WaitForSeconds(delay);
        method.Invoke();
    }
}