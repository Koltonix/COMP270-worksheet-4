using UnityEngine;

namespace VFX.Events
{
    public class GameEventListenerString : MonoBehaviour
    {
        public GameEventString Event;
        public StringEvent Reponse;

        private void OnEnable() { Event.RegisterListener(this); }
        private void OnDisable() { Event.UnregisterListener(this); }
        public void OnEventRaise(string value) { Reponse.Invoke(value); }
    }
}