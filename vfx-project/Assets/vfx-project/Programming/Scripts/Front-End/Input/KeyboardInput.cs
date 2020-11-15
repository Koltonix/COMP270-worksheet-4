using UnityEngine;

namespace VFX.InputSystem
{
    public class KeyboardInput : UserInput
    {
        protected override void LeftClick() 
        {
            if (Input.GetKeyDown(keybinds.leftClick))
                onLeftClick.Raise();
        }

        protected override void RightClick() 
        {
            if (Input.GetKeyDown(keybinds.RightClick))
                onRightClick.Raise();
        }

        protected override void Interact() 
        {
            if (Input.GetKeyDown(keybinds.Interact))
                onInteract.Raise();
        }

        protected override void Restart()
        {
            if (Input.GetKeyDown(keybinds.Restart))
                onRestart.Raise();
        }

        protected override void Generate()
        {
            if (Input.GetKeyDown(keybinds.Generate))
                onGenerate.Raise();
        }

        protected override void RaycastFromCamera()
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, rayDistance, mask);

            onMouseHover.Raise(hit);
        }

    }
}