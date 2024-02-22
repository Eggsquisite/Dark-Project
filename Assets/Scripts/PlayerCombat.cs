using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : PlayerSystem
{
    private Collider coll;

    protected override void Awake()
    {
        base.Awake();
        coll = transform.root.GetComponent<Collider>();
    }



    private void OnEnable()
    {
        
    }
}
