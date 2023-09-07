using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasteStats
{ 
    private float _speed;
    private Caste _caste;

    public CasteStats(Caste caste, float speed)
    {
        _speed = speed;
        _caste = caste;
    }

    public override string ToString()
    {
        string str = _caste.ToString();
        str += "\nSpeed: " + _speed;
        return str;
    }
}