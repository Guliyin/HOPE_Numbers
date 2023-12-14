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
public class EggletController : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    private EggletMgr eggletMgr;
    private AnimatorOverrideController overrideController;

    /// <summary>
    /// The clip that is currently playing.
    /// </summary>
    private AnimationClip playingClip;

    /// <summary>
    /// Called by the EggletMgr in Start function.
    /// </summary>
    public void Init()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        eggletMgr = FindObjectOfType<EggletMgr>();

        EventCenter.AddListener<AnimType>(FunctionType.correct, PlayAnim);
        EventCenter.AddListener<AnimType>(FunctionType.wrong, PlayAnim);

        InitAnimator();
    }
    /// <summary>
    /// Set up the animator for the controller.
    /// </summary>
    private async void InitAnimator()
    {
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;

        playingClip = await eggletMgr.GetAnimFromDic(AnimType.Idle);

        overrideController["AnimSlot1"] = playingClip;

        PlayAnim(AnimType.Idle);
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
    public async void PlayAnim(AnimationClip clip)
    {
        float duration = clip.length;

        playingClip = await eggletMgr.GetAnimFromDic(AnimType.Idle);
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
    public async void PlayAnim(AnimType type)
    {
        if (type == AnimType.Idle)
        {
            float duration = playingClip.length * (1 - animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.1f); 

            playingClip = await eggletMgr.GetAnimFromDic(type);
            overrideController[animator.GetCurrentAnimatorStateInfo(0).IsName("Slot1") ? "AnimSlot2" : "AnimSlot1"] = playingClip;

            StopAllCoroutines();
            StartCoroutine(Timer(duration));
        }
        else
        {
            PlayAnim(await eggletMgr.GetAnimFromDic(type));
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
        PlayAnim(AnimType.Idle);
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener<AnimType>(FunctionType.correct, PlayAnim);
        EventCenter.RemoveListener<AnimType>(FunctionType.wrong, PlayAnim);
    }
}
