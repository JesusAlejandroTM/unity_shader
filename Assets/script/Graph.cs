using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField, Range(10, 100)] private int resolution;
    [SerializeField, Range(0, 2)] private int function;
    private Transform[] _points;
    
    
    

    void Awake()
    {
        float step = 2f / resolution;
        var position = Vector3.zero;
        var scale = Vector3.one * step;
        _points = new Transform[resolution];
        for (int i = 0; i < _points.Length; i++)
        {
            Transform point = _points[i] = point = Instantiate(pointPrefab);
            position.x = (i + 0.5f) * step - 1f;
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        float time = Time.time; 
        for (int i = 0; i < _points.Length; i++)
        {
            Transform point = _points[i];
            Vector3 position = point.localPosition;
            if (function == 0) {
                position.y = FunctionLibrary.Wave(position.x, time);
            }
            else if (function == 1){
                position.y = FunctionLibrary.MultiWave(position.x, time);
            }
            else
            {
                position.y = FunctionLibrary.Ripple(position.x, time);
            }
            point.localPosition = position;
        }
    }
}
