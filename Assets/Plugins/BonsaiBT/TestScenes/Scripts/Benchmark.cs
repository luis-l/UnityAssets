
using System.Collections.Generic;
using UnityEngine;

using Bonsai.Core;

public class Benchmark : MonoBehaviour
{
    public int count = 0;

    public GameObject prefab;

    // Use this for initialization
    void Awake()
    {
        for (int i = 0; i < count; ++i) {
            var go = GameObject.Instantiate(prefab);
            go.transform.parent = transform;
        }
    }
}
