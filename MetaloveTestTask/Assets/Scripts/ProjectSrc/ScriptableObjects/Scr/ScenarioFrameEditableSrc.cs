using Scripts.BaseSystems;
using Scripts.ProjectSrc;
using System;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenarioFrameEditable", menuName = "Scriptable Obj/Project src/Scenario frame editable")]
public class ScenarioFrameEditableSrc : ScriptableObject, IScenarioFrameEditor
{
    public const string EXTENSION = ".json";
    public const string EXTENSION_META = ".json.meta";

    [SerializeField]
    [Uneditable]
    private string _path = default;
    public string Path
    {
        get => _path; 
        set => _path = value;
    }

    [SerializeField]
    [Uneditable]
    private string _appVersion = default;                              
    public string AppVersion 
    { 
        get => _appVersion; 
        set
        {
            _appVersion = value;
            ScenarioFrame._appVersion = value;
            OnDataUpdated?.Invoke(); 
        }
    }

    [SerializeField]
    private string _scenarioFrameId = default;
 //   private string _previousScenarioFrameId = default;
    public string ScenarioFrameId
    {
        get => _scenarioFrameId;
        set
        {
            _scenarioFrameId = value;
            ScenarioFrame._scenarioFrameId = value;
            Rename(_scenarioFrameId); 
            OnDataUpdated?.Invoke();
        }
    }

    [Space(15)]
    [Header("The field won't be used if choicess exists.")]
    [SerializeField]
    private string _nextScenarioFrameId = default;
    public string NextScenarioFrameId
    {
        get => _nextScenarioFrameId;
        set
        {
            _nextScenarioFrameId = value;
            ScenarioFrame._nextScenarioFrameId = value;
            OnDataUpdated?.Invoke();
        }
    }

    [SerializeField]
    private string _sceneId = default;
    public string SceneId
    {
        get => _sceneId;
        set
        {
            _sceneId = value;
            ScenarioFrame._sceneId = value;
            OnDataUpdated?.Invoke();
        }
    }

    [SerializeField]
    [TextAreaAttribute]
    private string _text;
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            ScenarioFrame._text = value;
            OnDataUpdated?.Invoke();
        }
    }

    [Space(15)]
    [SerializeField]
    private ChoiceData[] _choiceOption;

    [Space(15)]
    [SerializeField]
    private string _characterId = default;
    public string CharacterId
    {
        get => _characterId;
        set
        {
            _characterId = value;
            ScenarioFrame._characterId = value;
            OnDataUpdated?.Invoke();
        }
    }

    [SerializeField]
    public AlignmentId _characterAlignment;
    public AlignmentId CharacterAlignment
    {
        get => _characterAlignment;
        set
        {
            _characterAlignment = value;
            ScenarioFrame._characterAlignment = value;
            OnDataUpdated?.Invoke();
        }
    }

    public bool IsScenarioFrameAvailable => _scenarioFrame != null; 

    private ScenarioFrameModel _scenarioFrame;

    /// <summary>
    ///     Automaticaly create ScenarioFrameModel object if its null
    /// </summary>
    public ScenarioFrameModel ScenarioFrame
    {
        get
        {
            if(_scenarioFrame == null )
                _scenarioFrame = new ScenarioFrameModel(Application.version);

            return _scenarioFrame; 
        }

        private set
        {
            _scenarioFrame = value;
        }
    }

    public event Action OnDataUpdated;

    private void OnValidate()
    {

        if(!string.IsNullOrEmpty(Path) && !string.IsNullOrEmpty(_scenarioFrameId))
        {
            var fileName = System.IO.Path.GetFileNameWithoutExtension(Path);

            if (!fileName.Equals( _scenarioFrameId))
                Rename(_scenarioFrameId);
        }
        
        if(IsScenarioFrameAvailable)
        {
            ScenarioFrame._text = Text;
            ScenarioFrame._nextScenarioFrameId = NextScenarioFrameId;
            ScenarioFrame._characterId = CharacterId;
            ScenarioFrame._characterAlignment = CharacterAlignment;

            if(_choiceOption == null || _choiceOption.Length< 1)
            {
                ScenarioFrame._choiceOptionText = null;
                ScenarioFrame._choiceOptionScenarioFrameId = null; 
            }
            else
            {
                ScenarioFrame._choiceOptionText = new string[_choiceOption.Length];
                ScenarioFrame._choiceOptionScenarioFrameId = new string[_choiceOption.Length];

                for (int i = 0; i < _choiceOption.Length; i++)
                {
                    ScenarioFrame._choiceOptionText[i] = _choiceOption[i]._coiceText;
                    ScenarioFrame._choiceOptionScenarioFrameId[i] = _choiceOption[i]._sceneFrameId; 
                }
            }
        }

        OnDataUpdated?.Invoke();
    }

    public void ImportData(ScenarioFrameModel dataSource)
    {
        Clear(); 

        ScenarioFrame = new ScenarioFrameModel(dataSource);

        AppVersion = dataSource._appVersion;
        ScenarioFrameId = dataSource._scenarioFrameId;
        SceneId = dataSource._sceneId;
        
        CharacterId = dataSource._characterId;
        CharacterAlignment = dataSource._characterAlignment;

        if( 
            (dataSource._choiceOptionText != null && dataSource._choiceOptionScenarioFrameId != null) &&
            (dataSource._choiceOptionText.Length == dataSource._choiceOptionScenarioFrameId.Length) &&
            (dataSource._choiceOptionText.Length > 0)
            )
        {
            SetChoicessData(dataSource._choiceOptionText, dataSource._choiceOptionScenarioFrameId); 
        }
        else
        {
            Text = dataSource._text;
            NextScenarioFrameId = dataSource._nextScenarioFrameId;
        }

        Debug.Log("\t imported"); 
        OnDataUpdated?.Invoke();
    }

    public void SetCharacterData(string characterId, AlignmentId alignmentId)
    {
        _characterId = characterId;
        _characterAlignment = alignmentId;

        if (IsScenarioFrameAvailable)
        {
            ScenarioFrame._characterId = characterId;
            ScenarioFrame._characterAlignment = alignmentId;
        }

        OnDataUpdated?.Invoke();
    }

    public void SetChoicessData(string[] choicesTextId, string[] choicesOptionScenarioFrameId)
    {
        _choiceOption = new ChoiceData[choicesTextId.Length];

        if (IsScenarioFrameAvailable) 
        {
            ScenarioFrame._choiceOptionText = new string[choicesTextId.Length];
            ScenarioFrame._choiceOptionScenarioFrameId = new string[choicesTextId.Length];
        }

        for (int i = 0; i < _choiceOption.Length; i++)
        {
            _choiceOption[i] = new ChoiceData();
            _choiceOption[i]._coiceText = choicesTextId[i];
            _choiceOption[i]._sceneFrameId = choicesOptionScenarioFrameId[i];

            if (IsScenarioFrameAvailable)
            {
                ScenarioFrame._choiceOptionText[i] = _choiceOption[i]._coiceText;
                ScenarioFrame._choiceOptionScenarioFrameId[i] = _choiceOption[i]._sceneFrameId;
            }
        }
    }

#if UNITY_EDITOR
    public void Rename(string newFileName)
    {
        if (!IsScenarioFrameAvailable) return; 
        if (string.IsNullOrEmpty(Path)) return;
        if (string.IsNullOrEmpty(newFileName)) return;

        string directory = System.IO.Path.GetDirectoryName(Path);
        string newPath = System.IO.Path.Combine(directory, newFileName);

        try
        {
            if (System.IO.File.Exists(Path))
            {
                if (!System.IO.Path.HasExtension(newPath))
                    newPath += EXTENSION;

                System.IO.File.Move(Path, newPath);
                Path = newPath;
            }
            else
            {
                Debug.LogWarning($"Rename failed: File not found at path {Path}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Rename failed: {ex.Message}");
        }
    }

    public void SaveCurrent()
    {
        if (!IsScenarioFrameAvailable) return;

        try
        {
            string json = JsonUtility.ToJson(ScenarioFrame, true);
            System.IO.File.WriteAllText(Path, json, Encoding.UTF8);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        
    }
#endif

    public void Clear()
    {
        _path = default; 
        _scenarioFrameId = default;
        _sceneId = default;
        _text = default;
        _nextScenarioFrameId = default;
        _choiceOption = null;
        _characterId = default;
        _characterAlignment = AlignmentId.Non;

        _scenarioFrame = null; 
    }

    [Serializable]
    private struct CharacterData
    {
        [HideInInspector]
        public string _name;
        public string _characterId;
        public AlignmentId _alignmanetId;
    }

    [Serializable]
    private struct ChoiceData
    {
        [TextAreaAttribute]
        public string _coiceText;
        public string _sceneFrameId;
    }
}
