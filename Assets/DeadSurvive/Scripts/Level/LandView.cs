using System.Collections.Generic;
using DeadSurvive.Common;
using DeadSurvive.TapPosition;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Level
{
    public class LandView : MonoBehaviour, IEcsWorldReceiver
    {
        public TapOnTrigger TapOnTrigger => _tapOnTrigger;
        
        [SerializeField]
        private TapOnTrigger _tapOnTrigger;

        [SerializeField] 
        private List<TransitionView> _transitionViews;

        public void SetEcsWorld(EcsWorld world)
        {
            TapOnTrigger.SetEcsWorld(world);

            foreach (var transitionView in _transitionViews)
            {
                transitionView.SetEcsWorld(world);
            }
        }
        
        public void ConfigureTransitions(List<DirectionType> allowedTransitions)
        {
            foreach (var transitionView in _transitionViews)
            {
                if (allowedTransitions.Contains(transitionView.Direction))
                {
                    continue;
                }
                
                transitionView.gameObject.SetActive(false);
            }
        }
    }
}