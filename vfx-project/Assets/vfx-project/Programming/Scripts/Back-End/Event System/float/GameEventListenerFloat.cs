using UnityEngine;

namespace VFX.Events
{
    public class GameEventListenerFloat : MonoBehaviour
    {
        public GameEventFloat Event;
        public FloatEvent Reponse;

        private void OnEnable() { Event.RegisterListener(this); }
        private void OnDisable() { Event.UnregisterListener(this); }
        public void OnEventRaise(float value) { Reponse.Invoke(value); }
    }
}