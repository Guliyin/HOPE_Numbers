using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Number>())
        {
            GameMgr.Instance.NumberSelected(other.GetComponent<Number>());
        }
    }
}
