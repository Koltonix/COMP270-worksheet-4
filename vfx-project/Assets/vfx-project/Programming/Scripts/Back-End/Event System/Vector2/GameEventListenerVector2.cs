using UnityEngine;

namespace VFX.Events
{
    public class GameEventListenerVector2 : MonoBehaviour
    {
        public GameEventVector2 Event;
        public Vector2Event Reponse;

        private void OnEnable() { Event.RegisterListener(this); }
        private void OnDisable() { Event.UnregisterListener(this); }
        public void OnEventRaise(Vector2 value) { Reponse.Invoke(value); }
    }
}