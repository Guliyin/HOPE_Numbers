using System.Collections;
using UnityEngine;

/// <summary>
/// Type of animations, Use this if there are multiple aniamtions in a type and you want to play a random one.
/// </summary>
public enum AnimType
{
    Idle,
    RightAnswer,
    WrongAnswer
}

[RequireComponent(typeof(Animator))]
public class CharacterAnimationController : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    //private EggletAnimationMgr eggletMgr;
    protected CharacterAnimationMgr characterAnimationMgr;
    private AnimatorOverrideController overrideController;

    /// <summary>
    /// The clip that is currently playing.
    /// </summary>
    private AnimationClip playingClip;

    /// <summary>
    /// Called by the Mgr in Start function.
    /// </summary>
    public virtual void Init(CharacterAnimationMgr mgr)
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        characterAnimationMgr = mgr;

        EventCenter.AddListener<AnimType>(FunctionType.correct, PlayAnimAsync);
        EventCenter.AddListener<AnimType>(FunctionType.wrong, PlayAnimAsync);

        InitAnimator();
    }

    /// <summary>
    /// Set up the animator for the controller.
    /// </summary>
    private async void InitAnimator()
    {
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;

        playingClip = await characterAnimationMgr.GetAnimFromDicAsync(AnimType.Idle);

        overrideController["AnimSlot1"] = playingClip;

        PlayAnimAsync(AnimType.Idle);
    }

    /// <summary>
    /// Called by the animation event to play the aniamtion sound.
    /// </summary>
    /// <param name="audioClip">AudioClip that is about to be played</param>
    public void PlayAnimAudio(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    /// <summary>
    /// Play the given clip directly. Goes back to idling when finish.
    /// </summary>
    /// <param name="clip">AnimationClip this is about to be played</param>
    public async void PlayAnimAsync(AnimationClip clip)
    {
        float duration = clip.length;

        playingClip = await characterAnimationMgr.GetAnimFromDicAsync(AnimType.Idle);
        overrideController["AnimSlot3"] = clip;
        overrideController["AnimSlot2"] = playingClip;

        animator.CrossFade("Slot3", 0.05f);

        StopAllCoroutines();
        StartCoroutine(Timer(duration));
    }

    /// <summary>
    /// Play a random aniamtion of the given type.
    /// </summary>
    /// <param name="type">animation clip type</param>
    public async void PlayAnimAsync(AnimType type)
    {
        if (type == AnimType.Idle)
        {
            float duration = playingClip.length * (1 - animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.1f);

            playingClip = await characterAnimationMgr.GetAnimFromDicAsync(type);
            overrideController[animator.GetCurrentAnimatorStateInfo(0).IsName("Slot1") ? "AnimSlot2" : "AnimSlot1"] = playingClip;

            StopAllCoroutines();
            StartCoroutine(Timer(duration));
        }
        else
        {
            PlayAnimAsync(await characterAnimationMgr.GetAnimFromDicAsync(type));
        }
    }

    /// <summary>
    /// Wait until the end of the current animation clip.
    /// </summary>
    /// <param name="time">animation clip length</param>
    /// <returns></returns>
    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        PlayAnimAsync(AnimType.Idle);
    }
    private void OnDisable()
    {
        EventCenter.RemoveListener<AnimType>(FunctionType.correct, PlayAnimAsync);
        EventCenter.RemoveListener<AnimType>(FunctionType.wrong, PlayAnimAsync);
    }
}
