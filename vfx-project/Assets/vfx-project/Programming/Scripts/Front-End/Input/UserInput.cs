using UnityEngine;
using VFX.Events;

namespace VFX.InputSystem
{
    public class UserInput : MonoBehaviour
    {
        [SerializeField]
        protected UnityKeybinds keybinds;

        [SerializeField]
        protected GameEvent onLeftClick;
        [SerializeField]
        protected GameEvent onRightClick;
        [SerializeField]
        protected GameEventRaycastHit onMouseHover;

        [SerializeField]
        protected GameEvent onInteract;
        [SerializeField]
        protected GameEvent onRestart;
        [SerializeField]
        protected GameEvent onGenerate;

        protected Camera mainCamera;
        [SerializeField]
        protected LayerMask mask;
        [SerializeField]
        protected float rayDistance = Mathf.Infinity;
        protected RaycastHit hit;
        protected Ray ray;

        [SerializeField]
        private Color32 rayColour = Color.yellow;
        

        private void Awake() => mainCamera = Camera.main;

        protected virtual void LeftClick() { }
        protected virtual void RightClick() { }

        protected virtual void Interact() { }
        protected virtual void Restart() { }
        protected virtual void Generate() { }
        protected virtual void RaycastFromCamera() { }

        protected virtual void Update()
        {
            LeftClick();
            RightClick();

            Interact();
            Restart();
            Generate();

            RaycastFromCamera();
            DrawDebugRay(hit, ray);
        }

        private void DrawDebugRay(RaycastHit hit, Ray ray)
        {
            if (hit.collider)
                Debug.DrawLine(ray.origin, hit.point, rayColour);

            else
                Debug.DrawLine(ray.origin, ray.direction.normalized * rayDistance, rayColour);
        }

    }
}