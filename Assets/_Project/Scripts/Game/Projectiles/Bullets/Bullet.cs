﻿using System;
using System.Collections.Generic;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.Game.Player;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Projectiles.Bullets
{
    public class Bullet : IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new();
        private readonly Vector3ReactiveProperty _position = new();
        private readonly Vector2ReactiveProperty _velocity = new();
        private readonly ReactiveCommand<Bullet> _destroyedCommand = new();
        private Team _team;

        public Team Team
        {
            get => _team;
            set => _team = value;
        }
        
        public Vector3 Position
        {
            get => _position.Value;
            set => _position.Value = value;
        }

        public Vector2 Velocity
        {
            get => _velocity.Value;
            set => _velocity.Value = value;
        }

        public ICollection<IDisposable> Subscriptions => _subscriptions;

        public IObservable<Vector3> PositionAsObservable() => _position;

        public IObservable<Vector2> VelocityAsObservable() => _velocity;
        
        public IObservable<Bullet> DestroyedAsObservable() => _destroyedCommand;
        
        public void Init()
        {
            Observable
                .Timer(TimeSpan.FromSeconds(5f))
                .Subscribe(_ => Destroy())
                .AddTo(_subscriptions);
        }
        
        void IDisposable.Dispose()
        {
            _subscriptions.Dispose();
            _position.Dispose();
            _velocity.Dispose();
            _destroyedCommand.Dispose();
        }

        private void Destroy()
        {
            _destroyedCommand.Execute(this);
        }

        public void OnHitWithShip(Ship ship)
        {
            if (_team == Team.Player)
                return;

            ship.ApplyDamage();
            Destroy();
        }

        public void OnHitWithInvader(Invader invader)
        {
            if (_team == Team.Invaders)
                return;

            invader.ApplyDamage();
            Destroy();
        }
    }
}