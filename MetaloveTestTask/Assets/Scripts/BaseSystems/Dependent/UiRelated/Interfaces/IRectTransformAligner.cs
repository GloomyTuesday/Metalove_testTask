using UnityEngine;

namespace Scripts.BaseSystems.UiRelated
{
    public interface IRectTransformAligner
    {
        public int InstanceId{ get; }
        public float OffsetOnArrangingAxis { get; set; }
        public RectTransformAlignmentId RectTransformAlignmentId { get; set; }
        //  public bool AdoptSize { get; set; } Need to be implemented later
        public void Align();
        public void AlignNextFrame();
        public void AlignWithDelay(int frameAmountToDelay);
    }
}
