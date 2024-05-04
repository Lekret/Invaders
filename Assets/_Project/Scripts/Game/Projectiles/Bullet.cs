using System;
using _Project.Scripts.Game.Core;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Projectiles
{
    public class Bullet : IUpdatable, IFixedUpdatable
    {
        private readonly Vector3ReactiveProperty _position = new();
        private readonly Vector3ReactiveProperty _moveDirection = new();

        public IObservable<Vector3> PositionAsObservable() => _position;
        
        void IUpdatable.OnUpdate(float deltaTime)
        {
            
        }

        void IFixedUpdatable.OnFixedUpdate(float deltaTime)
        {
            
        }

        public void SetPosition(Vector3 position)
        {
            _position.Value = position;
        }

        public void SetMoveDirection(Vector3 up)
        {
            _moveDirection.Value = up;
        }
    }
}