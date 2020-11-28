using System;
using UnityEngine;
using UnityEngine.Events;

namespace VFX.Events
{
    [Serializable]
    public class Vector2Event : UnityEvent<Vector2> { };
    [Serializable]
    public class Vector3Event : UnityEvent<Vector3> { };
    [Serializable]
    public class RaycastHitEvent : UnityEvent<RaycastHit> { };
    [Serializable]
    public class FloatEvent : UnityEvent<float> { };
    [Serializable]
    public class StringEvent : UnityEvent<string> { };
    [Serializable]
    public class Vector3MDEvent : UnityEvent<Vector3[,]> { };
}
