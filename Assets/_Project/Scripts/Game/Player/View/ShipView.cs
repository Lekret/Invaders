using UnityEngine;

namespace _Project.Scripts.Game.Player.View
{
    public interface IShipView
    {
        Vector3 Position { get; set; }
    }

    public class ShipView : MonoBehaviour, IShipView
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
    }
}