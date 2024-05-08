using System;
using UniRx;

namespace _Project.Scripts.Game.Services
{
    public class PlayerScoreCounter : IDisposable
    {
        private readonly IntReactiveProperty _score = new();

        public int Score => _score.Value;
        public IObservable<int> ScoreAsObservable() => _score;
        
        void IDisposable.Dispose()
        {
            _score.Dispose();
        }

        public void AddScore(int delta)
        {
            _score.Value += delta;
        }
    }
}