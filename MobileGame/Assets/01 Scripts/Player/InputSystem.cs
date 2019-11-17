using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class InputSystem : ComponentSystem
{    
    
     protected override void OnUpdate() 
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        Entities.ForEach((ref MoveSpeed moveSpeed, ref Translation transform) =>
        {
            transform.Value.x += horizontal * moveSpeed.xSpeed;
        });

    }
}
