using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using UnityEngine;
using Animation = Spine.Animation;

public class AnimLooping : MonoBehaviour
{
    private SkeletonAnimation _skeletonAnimation;
    private float _animDelay;
    
    private List<Animation> _anims;
    private void Awake()
    {
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
        _anims = _skeletonAnimation.skeletonDataAsset.GetSkeletonData(false).Animations.ToList();
    }
    
    private int _animIndex;
    private void Update()
    {
        _animDelay -= Time.deltaTime;
        
        if (_animDelay <= 0f)
        {
            var anim = _anims[_animIndex % _anims.Count];
            _skeletonAnimation.AnimationState.SetAnimation(0, anim, false);
            _animDelay = anim.Duration;
            ++_animIndex;
        }
    }
}
