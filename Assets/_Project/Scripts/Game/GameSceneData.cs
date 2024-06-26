﻿using UnityEngine;

namespace _Project.Scripts.Game
{
    public class GameSceneData : MonoBehaviour
    {
        [SerializeField] private Transform _shipSpawnPoint;
        [SerializeField] private BoxCollider2D _shipMovementBounds;
        
        [SerializeField] private Transform _invadersFleetSpawnPoint;
        [SerializeField] private BoxCollider2D _invadersMovementBounds;

        public Vector3 ShipSpawnPosition => _shipSpawnPoint.position;
        public Bounds ShipMovementBounds => _shipMovementBounds.bounds;
        
        public Vector3 InvadersFleetSpawnPosition => _invadersFleetSpawnPoint.position;
        public Bounds InvadersMovementBounds => _invadersMovementBounds.bounds;
    }
}