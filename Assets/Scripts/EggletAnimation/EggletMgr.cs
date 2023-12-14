using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class EggletMgr : MonoBehaviour
{
    /// <summary>
    /// An array of all egglets with a EggletController component in the scene.
    /// </summary>
    private EggletController[] egglets;

    /// <summary>
    /// Stand-alone animations that are already loaded. The name of the animation is the Key. Returns an AnimationClip
    /// </summary>
    private Dictionary<string, AnimationClip> animDic = new Dictionary<string, AnimationClip>();

    /// <summary>
    /// Collaboration alphabet animations that are already loaded. Char is the kay and it returns an AnimationClip[]. Future modifications required for non-alphbet collabrotation aniamtions.
    /// </summary>
    private Dictionary<char, AnimationClip[]> alphabetAnimDic = new Dictionary<char, AnimationClip[]>();

    private Dictionary<string, string> loadingAnimDic = new Dictionary<string, string>();

    /// <summary>
    /// List of string stores aniamtion names and is used for the directory.
    /// </summary>
    [SerializeField] private AnimScriptableObject animNames;

    private static readonly object objlock = new object();

    private void Start()
    {
        //Find all egglets in the scene. store initialize them. 
        egglets = FindObjectsOfType<EggletController>();
        foreach (var egglet in egglets)
        {
            egglet.Init();
        }
    }
    private void Update()
    {
        //DELETE THIS///////////////////////////////////////////////////////////////////////////////////////////////
        //For testing only. Delete in the future.///////////////////////////////////////////////////////////////////
        if (Input.GetMouseButtonDown(1))
        {
            PlayAlphabet('d');
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    /// <summary>
    /// Get alphabet animations by char and play them. Unsafe code, need modification in the future. If there are less egglets than clips, it will throw exception.
    /// </summary>
    /// <param name="c">Character in alphabet</param>
    async void PlayAlphabet(char c)
    {
        Task<AnimationClip[]> task = GetAnimFromDic(c);
        await task;

        if (task.IsCompletedSuccessfully)
        {
            AnimationClip[] clips = task.Result;

            for (int i = 0; i < clips.Length; i++)
            {
                egglets[i].PlayAnim(clips[i]);
            }
        }
        else
        {
            throw new UnityException("Load Failed: " + c);
        }
    }

    /// <summary>
    /// Get a AnimationClip from the dictionary. If it can not be found, then load from Rescources.
    /// </summary>
    /// <param name="type">Type of the animation</param>
    /// <returns>Random stand-alone animation clip of given type.</returns>
    public async Task<AnimationClip> GetAnimFromDic(AnimType type)
    {
        string name = GetNewAnimName(type);
        if (!animDic.ContainsKey(name))
        {
            //clip = (AnimationClip)Resources.LoadAsync("Animations/Mine/Characters/Egglet/" + type.ToString() + "/" + name, typeof(AnimationClip)).asset;
            string path = "Egglet_Anim_" + type.ToString() + "/" + name + ".anim";
            AsyncOperationHandle<AnimationClip> handle = Addressables.LoadAssetAsync<AnimationClip>(path);

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Load Successful: " + name);
                if (!animDic.ContainsKey(name))
                {
                    animDic.Add(name, handle.Result);
                }
                return handle.Result;
            }
            else
            {
                throw new UnityException("Load Failed: " + path);
            }

        }
        else
        {
            return animDic[name];
        }

    }


    /// <summary>
    /// Get AnimationClips of a letter from the dictionary. If it can not be found, then load from Rescources.
    /// </summary>
    /// <param name="c">character of the alphabet</param>
    /// <returns>Array of all AnimationClips of one letter</returns>
    public async Task<AnimationClip[]> GetAnimFromDic(char c)
    {
        AnimationClip[] clips = new AnimationClip[1];
        string m_class = char.IsLower(c) ? "Small" : "Big";

        if (!alphabetAnimDic.ContainsKey(c))
        {
            //clips = Resources.LoadAll("Animations/Mine/Characters/Egglet/Alphabet/" + path + "/" + c, typeof(AnimationClip)).Cast<AnimationClip>().ToArray();
            string path = "Egglet_Anim_Collaboration/" + m_class + "/" + c;
            List<string> assets = GetAllAssetsInFolder(path);
            AsyncOperationHandle<IList<AnimationClip>> handle = Addressables.LoadAssetsAsync<AnimationClip>(assets, (x) => { Debug.Log(""); }, Addressables.MergeMode.Union);

            await handle.Task;


            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var items = handle.Result;
                clips = handle.Result.ToArray();
                if (!alphabetAnimDic.ContainsKey(c))
                    alphabetAnimDic.Add(c, clips);
            }
            else
            {
                throw new UnityException("Load Failed: " + path);
            }
        }
        else
        {
            clips = alphabetAnimDic[c];
        }
        return clips;
    }

    /// <summary>
    /// Get animation name from the scriptable object
    /// </summary>
    /// <param name="type"></param>
    /// <returns>Random animation name of a given type</returns>
    string GetNewAnimName(AnimType type)
    {
        string[] anims;
        switch (type)
        {
            case AnimType.Idle:
                anims = animNames.idleAnim;
                break;
            case AnimType.RightAnswer:
                anims = animNames.correctAnim;
                break;
            case AnimType.WrongAnswer:
                anims = animNames.wrongAnim;
                break;
            default:
                anims = animNames.idleAnim;
                break;
        }
        return anims[Random.Range(0, anims.Length)];
    }

    private bool IsKeyValid(object key, System.Type type)
    {
        foreach (var item in Addressables.ResourceLocators)
        {
            IList<IResourceLocation> locs;
            if (item.Locate(key, type, out locs))
            {
                return true;
            }
        }
        return false;
    }

    public List<string> GetAllAssetsInFolder(string folderPath)
    {
        List<string> assetsToLoad = new List<string>();

        foreach (var item in Addressables.ResourceLocators)
        {
            foreach (var key in item.Keys)
            {
                if (key.ToString().Contains(folderPath))
                {
                    assetsToLoad.Add(key.ToString());
                }
            }
        }
        return assetsToLoad;
    }
}
