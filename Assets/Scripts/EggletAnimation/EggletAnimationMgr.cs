using UnityEngine;

public class EggletAnimationMgr : CharacterAnimationMgr
{

    private void Update()
    {
        //DELETE THIS///////////////////////////////////////////////////////////////////////////////////////////////
        //For testing only. Delete in the future.///////////////////////////////////////////////////////////////////
        if (Input.GetMouseButtonDown(1))
        {
            PlayAlphabetAsync('d');
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
