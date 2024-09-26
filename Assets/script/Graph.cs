using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField, Range(10, 100)] private int resolution;
    [SerializeField] FunctionLibrary.FunctionName function;
    [SerializeField, Min(0f)] private float functionDuration = 1f, transitionDuration = 1f; 
    
    public enum TransitionMode { Cycle, Random }

    [SerializeField] private TransitionMode transitionMode;
    
    private Transform[] _points;

    float _duration;
    bool _transitioning;
    
    FunctionLibrary.FunctionName _transitionFunction;
    

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
        _duration += Time.deltaTime;
        if (_transitioning)
        {
            if (_duration >= transitionDuration)
            {
                _duration -= transitionDuration;
                _transitioning = false;
            }
        }
        else if (_duration >= functionDuration)
        {
            _duration -= functionDuration;
            _transitioning = true;
            _transitionFunction = function;
            PickNextFunction();
        }

        if (_transitioning)
        {
            UpdateFunctionTransition();
        }
        else
        {
            UpdateFunction();
        }
    }

    void UpdateFunction()
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
    
    void UpdateFunctionTransition()
    {
        FunctionLibrary.Function
            from = FunctionLibrary.GetFunction(_transitionFunction),
            to = FunctionLibrary.GetFunction(function);
        float progress = _duration / transitionDuration;
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
            _points[i].localPosition = FunctionLibrary.Morph(u, v, time, from, to, progress);
        }
    }

    void PickNextFunction()
    {
        function = transitionMode == TransitionMode.Cycle
            ? FunctionLibrary.GetNextFunctionName(function)
            : FunctionLibrary.GetRandomFunctionOtherThan(function);
    }
}
