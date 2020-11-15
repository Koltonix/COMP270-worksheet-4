using System.Collections.Generic;
using UnityEngine;

namespace VFX.Events
{
    [CreateAssetMenu(fileName = "Game-Event-Float", menuName = "ScriptableObjects/Events/GameEventFloat")]
    public class GameEventFloat : ScriptableObject
    {
        private List<GameEventListenerFloat> listeners = new List<GameEventListenerFloat>();

        public void Raise(float value)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventRaise(value);
        }

        public void RegisterListener(GameEventListenerFloat listener) { listeners.Add(listener); }
        public void UnregisterListener(GameEventListenerFloat listener) { listeners.Remove(listener); }
    }
}