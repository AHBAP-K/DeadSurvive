using DeadSurvive.Moving.Data;

namespace DeadSurvive.Moving
{
    public struct SelfMoveComponent
    {
        public float Delay { get; set; }

        private float _defaultDelay;

        public void Setup(SelfMoveData selfMoveData)
        {
            _defaultDelay = selfMoveData.Delay;
            RefreshDelay();
        }

        public void RefreshDelay()
        {
            Delay = _defaultDelay;
        }
    }
}