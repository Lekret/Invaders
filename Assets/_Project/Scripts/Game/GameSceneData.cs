﻿using UnityEngine;

namespace _Project.Scripts.Game
{
    public class GameSceneData : MonoBehaviour
    {
        [SerializeField] private Transform _shipSpawnPoint;
        [SerializeField] private Transform _invadersFleetSpawnPoint;

        public Vector3 ShipSpawnPosition => _shipSpawnPoint.position;
        public Vector3 InvadersFleetSpawnPosition => _invadersFleetSpawnPoint.position;
    }
}