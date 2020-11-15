using System.Collections.Generic;
using UnityEngine;

namespace VFX.Events
{
    [CreateAssetMenu(fileName = "Game-Event-RaycastHit", menuName = "ScriptableObjects/Events/GameEventRaycastHit")]
    public class GameEventRaycastHit : ScriptableObject
    {
        private List<GameEventListenerRaycastHit> listeners = new List<GameEventListenerRaycastHit>();

        public void Raise(RaycastHit value)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventRaise(value);
        }

        public void RegisterListener(GameEventListenerRaycastHit listener) { listeners.Add(listener); }
        public void UnregisterListener(GameEventListenerRaycastHit listener) { listeners.Remove(listener); }
    }
}