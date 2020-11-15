using System.Collections.Generic;
using UnityEngine;

namespace VFX.Events
{
    [CreateAssetMenu(fileName = "Game-Event", menuName = "ScriptableObjects/Events/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> listeners = new List<GameEventListener>();

        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventRaise();
        }

        public void RegisterListener(GameEventListener listener) { listeners.Add(listener); }
        public void UnregisterListener(GameEventListener listener) { listeners.Remove(listener); }
    }
}