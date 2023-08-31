using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAntBehavior
{
    public void Begin();

    public void Update();

    public void End();
}