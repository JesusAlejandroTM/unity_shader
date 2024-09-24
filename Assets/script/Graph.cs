using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField, Range(10, 100)] private int resolution;
    [SerializeField] FunctionLibrary.FunctionName function;
    private Transform[] _points;
    

    void Awake()
    {
        float step = 2f / resolution;
        var scale = Vector3.one * step;
        _points = new Transform[resolution * resolution];
        for (int i = 0; i < _points.Length; i++)
        {
            Transform point = _points[i] = point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        FunctionLibrary.Function libFunction = FunctionLibrary.GetFunction(function);
        float time = Time.time; 
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < _points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z++;
                v = (z + 0.5f) * step - 1f;
            }

            float u = (x + 0.5f) * step - 1f;
            _points[i].localPosition = libFunction(u, v, time);
        }
    }
}
