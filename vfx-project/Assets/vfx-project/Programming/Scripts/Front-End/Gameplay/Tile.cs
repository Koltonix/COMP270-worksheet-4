//////////////////////////////////////////////////
// Christopher Robertson 2020.
// https://github.com/Koltonix
// Copyright (c) 2020. All rights reserved.
//////////////////////////////////////////////////
using System;
using UnityEngine;

namespace VFX.Gameplay
{
    /// <summary>
    /// Used to store the Tile data for the entire grid. This will mainly be 
    /// used to store game logic.
    /// 
    /// **NOTE:** This does not handle deleting the object if you decide to 
    /// null it, so the class storying this is reponsible for the deletion
    /// of the GameObject from memory.
    /// </summary>
    [Serializable]
    public class Tile
    {
        public Vector3 position = Vector3.zero;
        private bool isOccupied = false;
        private GameObject heldObject = null;

        public Tile(Vector3 position) { this.position = position; }
        public Tile(Vector3 position, GameObject GO)
        {
            this.position = position;
            this.heldObject = GO;
        }

        public bool IsOccupied() { return isOccupied; }
        public GameObject GetHeldObject() { return heldObject; }

        public void SetHeldObject(GameObject GO)
        {
            heldObject = GO;
            isOccupied = true;
        }

        public GameObject RemoveHeldObject()
        {
            isOccupied = false;

            GameObject _heldObject = heldObject;
            heldObject = null;

            return _heldObject;
        }
    }
}