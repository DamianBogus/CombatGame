using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class FollowCamera : MonoBehaviour
{
    
    public void InitializeCamera(Translation playerPosition)
    {
        EntityManager entityManager = World.Active.EntityManager;
       // playerPos = playerPosition;
    }

    private Transform playerTransform;
    private Vector3 offset = new Vector3(-5,3.2f,-8);
    [SerializeField] private float cameraSpeed = 2.0f;
    void Start()
    {
        playerTransform = FindObjectOfType<PlayerMovement>().transform;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, (playerTransform.position + offset), Time.deltaTime * cameraSpeed);
        transform.LookAt(playerTransform);
    }
}

