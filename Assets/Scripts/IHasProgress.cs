using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<OnProgressChangeArgs> OnProgressChange;

    public class OnProgressChangeArgs : EventArgs
    {
        public float progressNormalized;
    }
}
