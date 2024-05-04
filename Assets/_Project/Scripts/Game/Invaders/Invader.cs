using System;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Invaders
{
    public class Invader
    {
        private readonly Vector3ReactiveProperty _position = new();

        public IObservable<Vector3> PositionAsObservable() => _position;
    }
}