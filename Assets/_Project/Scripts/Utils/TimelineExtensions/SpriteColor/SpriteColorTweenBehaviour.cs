using System;
using UnityEngine;
using UnityEngine.Playables;

namespace _Project.Scripts.Utils.TimelineExtensions.SpriteColor
{
    [Serializable]
    public class SpriteColorTweenBehaviour : PlayableBehaviour
    {
        public Color FromColor;
        public Color ToColor;
        public SpriteRenderer SpriteRenderer;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var time = (float) playable.GetTime();
            var duration = (float) playable.GetDuration();
            SpriteRenderer.color = Color.Lerp(FromColor, ToColor, time / duration);
        }
    }
}