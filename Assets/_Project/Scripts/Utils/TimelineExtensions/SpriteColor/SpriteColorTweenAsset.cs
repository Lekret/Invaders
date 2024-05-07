using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace _Project.Scripts.Utils.TimelineExtensions.SpriteColor
{
    [Serializable]
    public class SpriteColorTweenAsset : PlayableAsset, ITimelineClipAsset
    {
        public Color FromColor;
        public Color ToColor;
        public ExposedReference<SpriteRenderer> SpriteRenderer;
        
        public ClipCaps clipCaps => ClipCaps.None;
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SpriteColorTweenBehaviour>.Create(graph);
            var behaviour = playable.GetBehaviour();
            behaviour.FromColor = FromColor;
            behaviour.ToColor = ToColor;
            behaviour.SpriteRenderer = SpriteRenderer.Resolve(graph.GetResolver());
            return playable;
        }
    }
}