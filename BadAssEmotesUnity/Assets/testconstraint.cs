using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class testconstraint : MonoBehaviour, IConstraint
{
    public ConstraintSource targetTransform;
    public Transform source;
    public float weight { get; set; }
    public bool constraintActive { get; set; }
    public bool locked { get; set; }

    public int sourceCount => throw new System.NotImplementedException();

    public int AddSource(ConstraintSource source)
    {
        targetTransform = source;
        return 0;
    }

    public ConstraintSource GetSource(int index)
    {
        return targetTransform;
    }

    public void GetSources(List<ConstraintSource> sources)
    {
        return;
    }

    public void RemoveSource(int index)
    {
        return;
    }

    public void SetSource(int index, ConstraintSource source)
    {
        return;
    }

    public void SetSources(List<ConstraintSource> sources)
    {
        return;
    }

    private void Awake()
    {
        targetTransform.sourceTransform = source;
        targetTransform.weight = 1;
        weight = 1;
        constraintActive = true;
        locked = true;
    }
}
