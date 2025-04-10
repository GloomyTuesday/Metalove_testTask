using Scripts.BaseSystems;
using Scripts.ProjectSrc;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenarioFrameToLoadBuffer", menuName = "Scriptable Obj/Project src/Scenario frame to load buffer")]
public class ScenarioFrameToLoadBufferSrc : ScriptableObject, IScenarioFrameToLoadBuffer
{
    [Header("Scenario to load:")]
    [SerializeField]
    private string _scenarioFrameId; 

    public string ScenarioFrameId { get => _scenarioFrameId; set => _scenarioFrameId = value; }
}
