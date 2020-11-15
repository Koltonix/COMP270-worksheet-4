using UnityEngine;

namespace VFX.Events
{
    public class GameEventListenerRaycastHit : MonoBehaviour
    {
        public GameEventRaycastHit Event;
        public RaycastHitEvent Reponse;

        private void OnEnable() { Event.RegisterListener(this); }
        private void OnDisable() { Event.UnregisterListener(this); }
        public void OnEventRaise(RaycastHit value) { Reponse.Invoke(value); }
    }
}