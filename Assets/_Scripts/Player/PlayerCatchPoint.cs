﻿using _Scripts.Objects;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerCatchPoint : MonoBehaviour
    {
        [SerializeField] private bool _left;
        [SerializeField] private Transform _root;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerController _playerController;
        
        public bool Left => _left;
        public float TotalWeight { get; private set; }

        public void Attach(DroppedObject droppedObject)
        {
            droppedObject.Rigidbody2D.bodyType = RigidbodyType2D.Static;
            droppedObject.transform.SetParent(_root);
            Destroy(droppedObject.Rigidbody2D);
            Destroy(droppedObject.Collider);
            droppedObject.SimpleCollider.SetActive(true);

            droppedObject.SetTag(_left ? "DroppedObjectLeft" : "DroppedObjectRight");
            
            TotalWeight += droppedObject.Weight;
            _playerAnimator.CollectItem();
            
            GameEvents.GameEvents.ObstacleCaught(droppedObject.transform.position.y, _playerController.AllWeight);
        }
    }
}