using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallProducer : Station
{
    // Start is called before the first frame update
    void Start()
    {
        _stationType = StationType.Producer;
    }
}
