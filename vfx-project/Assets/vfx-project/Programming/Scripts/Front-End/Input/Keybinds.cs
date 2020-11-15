using UnityEngine;

namespace VFX.InputSystem
{
    [CreateAssetMenu(fileName = "Keybinds", menuName = "ScriptableObjects/Input/Keybinds")]
    public class Keybinds : ScriptableObject
    {
        public KeyCode leftClick = KeyCode.Mouse0;
        public KeyCode RightClick = KeyCode.Mouse1;

        public KeyCode Interact = KeyCode.E;
        public KeyCode Restart = KeyCode.R;
        public KeyCode Generate = KeyCode.G;
    }
}