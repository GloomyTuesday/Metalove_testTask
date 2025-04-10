using System;

namespace Scripts.ProjectSrc
{
    [Serializable]
    public class ScenarioFrameModel
    {
        public string _appVersion;

        public string _scenarioFrameId;

        public string _sceneId;

        public string _text;
        public string _nextScenarioFrameId; 

        public string[] _choiceOptionText;
        public string[] _choiceOptionScenarioFrameId;

        public string _characterId;
        public AlignmentId _characterAlignment; 

        public ScenarioFrameModel(string appVersion)
        {
            _appVersion = appVersion;
        }

        public ScenarioFrameModel(ScenarioFrameModel sourceToImportFrom)
        {
            _appVersion = sourceToImportFrom._appVersion;
            _scenarioFrameId = sourceToImportFrom._scenarioFrameId;
            _sceneId = sourceToImportFrom._sceneId;
            _text = sourceToImportFrom._text;
            _nextScenarioFrameId = sourceToImportFrom._nextScenarioFrameId;

            _characterId = sourceToImportFrom._characterId;
            _characterAlignment = sourceToImportFrom._characterAlignment;

            if (sourceToImportFrom._choiceOptionText != null )
            {
                _choiceOptionText = new string[sourceToImportFrom._choiceOptionText.Length];
                _choiceOptionScenarioFrameId = new string[sourceToImportFrom._choiceOptionScenarioFrameId.Length];  

                if(_choiceOptionText.Length  == _choiceOptionScenarioFrameId.Length)
                {
                    for (int i = 0; i < _choiceOptionText.Length; i++)
                    {
                        _choiceOptionText[i] = sourceToImportFrom._choiceOptionText[i];
                        _choiceOptionScenarioFrameId[i] = sourceToImportFrom._choiceOptionScenarioFrameId[i]; 
                    }
                }
            }
        }
    }
}
