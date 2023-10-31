using System.Text;
using Spine.Unity;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float speed;
    public float skillStartDelay;
    
    private Rigidbody _rig;
    private SkeletonAnimation _skeletonAnimation;
    private Vector3 _axis;

    private float _localScaleX;
    private float _skillDelay;
    private int _skillIndex;
    private void Awake()
    {
        _rig = GetComponentInChildren<Rigidbody>();
        _skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        _localScaleX = _skeletonAnimation.transform.localScale.x;
        _skillDelay = skillStartDelay;
        _skillIndex = 0;
    }

    private bool _isSkillPlaying;

    private void Update()
    {
        var inputAxis = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        var strAnim = new StringBuilder();
        var isSkill = false;
        if (inputAxis == Vector3.zero)
        {
            _skillDelay -= Time.deltaTime;
            if (_skillDelay <= 0f)
            {
                isSkill = true;
                ++_skillIndex;
                strAnim.Append($"attack_0{_skillIndex % 3 + 1}_");
            }
            else
            {
                if (_isSkillPlaying)
                    return;
                
                strAnim.Append("idle_");
            }

            AppendStrAnim(strAnim, _axis);
        }
        else
        {
            strAnim.Append("run_");
            _axis = inputAxis;
            AppendStrAnim(strAnim, _axis);
            _rig.MovePosition(_rig.position + _axis * speed * Time.deltaTime);
            _skillDelay = skillStartDelay;
            _skillIndex = 0;
        }

        if (_skeletonAnimation.AnimationState.GetCurrent(0).Animation.Name != strAnim.ToString())
        {
            var track = _skeletonAnimation.AnimationState.SetAnimation(0, strAnim.ToString(), isSkill == false);
            if (isSkill)
                _skillDelay = track.Animation.Duration;
            _isSkillPlaying = isSkill;
        }

        var scale = _skeletonAnimation.transform.localScale;
        scale.x = _localScaleX;
        if (_axis.x < 0f)
            scale.x *= -1f;
        _skeletonAnimation.transform.localScale = scale;
    }

    private void AppendStrAnim(StringBuilder strAnim, Vector3 velocity)
    {
        if (velocity.x != 0f)
            strAnim.Append("halfside");
        else if (velocity.z > 0f)
            strAnim.Append("back");
        else
            strAnim.Append("front");
    }
}