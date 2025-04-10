namespace Scripts.BaseSystems.Core
{
    public interface IPopUpAnimationEvents
    {
        public void PopUpAnimOpenStart();
        public void PopUpAnimOpenEnd();
        public void PopUpAnimCloseStart();
        public void PopUpAnimCloseEnd();
        public void BgAnimOpenStart();
        public void BgAnimOpenEnd();
        public void BgAnimPointerDownStart();
        public void BgAnimPointerDownEnd();
        public void BgAnimCloseCancelStart();
        public void BgAnimCloseCancelEnd();
        public void BgAnimCloseStart();
        public void BgAnimCloseEnd();
    }
}
