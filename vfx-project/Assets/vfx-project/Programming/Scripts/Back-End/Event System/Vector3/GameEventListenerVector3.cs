using UnityEngine;

namespace VFX.Events
{
    public class GameEventListenerVector3 : MonoBehaviour
    {
        public GameEventVector3 Event;
        public Vector3Event Reponse;

        private void OnEnable() { Event.RegisterListener(this); }
        private void OnDisable() { Event.UnregisterListener(this); }
        public void OnEventRaise(Vector3 value) { Reponse.Invoke(value); }
    }
}