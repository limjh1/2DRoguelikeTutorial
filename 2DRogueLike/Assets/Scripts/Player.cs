using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{    
    public int pointPerFood = 10;
    public int pointPerSoda = 20;

    public int wallDamage = 3;

    Animator animator;
    
    protected override void Start()
    {
        
    }
  
    void Update()
    {
        int hor = 0;
        int ver = 0;

        hor = (int)Input.GetAxisRaw("Horizontal");
        ver = (int)Input.GetAxisRaw("Vertical");

        if (hor != 0) ver = 0;


    }

    protected override void OnCantMove<T>(T component)
    {
        
    }

}
