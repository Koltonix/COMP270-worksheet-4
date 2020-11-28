using UnityEngine;
using VFX.Events;

namespace VFX.InputSystem
{
    public class UserInput : MonoBehaviour
    {
        [SerializeField]
        protected Keybinds keybinds;

        [Space]
        [SerializeField]
        protected GameEvent onLeftClick;
        [SerializeField]
        protected GameEvent onRightClick;
        [SerializeField]
        protected GameEventRaycastHit onMouseHover;

        [Space]
        [SerializeField]
        protected GameEvent onInteract;
        [SerializeField]
        protected GameEvent onRestart;
        [SerializeField]
        protected GameEvent onGenerate;

        [Space]
        protected Camera mainCamera;
        [SerializeField]
        protected LayerMask mask;
        [SerializeField]
        protected float rayDistance = 500.0f;
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
            // Check needed since the ScriptableObject is somehow becoming
            // null for one frame and then a normal reference the second.
            if (keybinds)
            {
                LeftClick();
                RightClick();

                Interact();
                Restart();
                Generate();

                RaycastFromCamera();
                DrawDebugRay(hit, ray);
            }
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