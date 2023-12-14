using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberFactory : MonoBehaviour
{
    int[] numbers = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    [Range(10, 50)]
    [SerializeField] private float numberGenerateRange;
    [Range(-10, 10)]
    [SerializeField] private float numberGenerateHeight;

    [SerializeField] private GameObject textPrefab;

    private void Awake()
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            int n = Random.Range(0, 10);
            int temp = numbers[n];
            numbers[n] = numbers[i];
            numbers[i] = temp;
        }
    }

    public int GenerateNewNumber()
    {
        float r = Random.Range(10, numberGenerateRange);
        float x = Random.Range(-r, r);
        float z = Mathf.Sqrt(r * r - x * x) * (Random.Range(0, 2) * 2 - 1);
        float y = Random.Range(-numberGenerateHeight, numberGenerateHeight);
        Vector3 pos = new Vector3(x, y, z);
        Quaternion quat = Quaternion.LookRotation(Vector3.Normalize(pos));

        GameObject newNumber = Instantiate(textPrefab, pos, quat);
        newNumber.GetComponent<Number>().Init(numbers[GameMgr.Instance.Level]);
        return numbers[GameMgr.Instance.Level];
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
        Gizmos.DrawSphere(Vector3.zero, numberGenerateRange);
        Gizmos.DrawWireSphere(Vector3.zero, numberGenerateRange);
    }
}
