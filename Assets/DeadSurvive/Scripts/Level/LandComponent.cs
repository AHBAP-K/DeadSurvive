namespace DeadSurvive.Level
{
    public struct LandComponent
    {
        public LandView LandView { get; private set; }
        
        public void Setup(LandView landView)
        {
            LandView = landView;
        }
    }
}