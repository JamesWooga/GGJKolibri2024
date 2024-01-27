using _Scripts.Objects;
using TMPro;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerCatchPoint : MonoBehaviour
    {
        [SerializeField] private bool _left;
        [SerializeField] private Transform _root;
        [SerializeField] private TMP_Text _weightText;
        
        public bool Left => _left;
        public float TotalWeight { get; private set; }

        public void Attach(DroppedObject droppedObject)
        {
            droppedObject.Rigidbody2D.bodyType = RigidbodyType2D.Static;
            droppedObject.transform.SetParent(_root);

            droppedObject.SetTag(_left ? "DroppedObjectLeft" : "DroppedObjectRight");
            GameEvents.GameEvents.ObstacleCaught(droppedObject.transform.position.y);
            
            TotalWeight += droppedObject.Weight;
            _weightText.text = $"{TotalWeight}kg";
        }
    }
}