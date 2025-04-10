
using System;

namespace Scripts.ProjectSrc
{
    public interface IScenarioFrameEditor
    {
        public ScenarioFrameModel ScenarioFrame { get; }

        public string AppVersion { get; set; }
        public string ScenarioFrameId { get; set; }
        public string NextScenarioFrameId { get; set; }
        public string SceneId { get; set; }
        public string Text { get; set; }
        public string CharacterId { get; set; }
        public AlignmentId CharacterAlignment { get; set; }

        public bool IsScenarioFrameAvailable { get;}

        public void ImportData(ScenarioFrameModel dataSource);
        public void SetCharacterData(string characterId, AlignmentId alignmentId);
        public void SetChoicessData(string[] choicesTextId, string[] choicesOptionScenarioFrameId);

        public event Action OnDataUpdated;
    }
}
