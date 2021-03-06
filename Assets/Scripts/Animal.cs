﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Animal : Agent
{
        public float moveForce = 2f;

        public float yawSpeed = 100f;
        
        private Rigidbody _rBody;

        public LayerMask groundMask;
        
        public FoodArea foodArea;
        
        [SerializeField] private Transform nose;

        [Tooltip("this needs to match the detectable tags on Rays Food sensor")]
        [SerializeField] private string foodTag;
        
        float FoodObtained { get; set; }

        public override void Initialize()
        {
            _rBody = GetComponent<Rigidbody>();
        }

        public override void OnEpisodeBegin()
        {
            foodArea.ResetSources();
            
            FoodObtained = 0f;

            _rBody.velocity = Vector3.zero;
            _rBody.angularVelocity = Vector3.zero;

            //only for training
            transform.Spawn(foodArea.radius, Random.Range(1.5f, 2.5f), groundMask);
        }

        /// <summary>
        /// Called when an action is received from player input or the neural network
        /// vectorAction[i] represents:
        ///Index 0: move vector forward (+1 = move else stop)
        ///Index 1: yaw angle (+1 = turn right, +2 = turn left)
        /// </summary>
        /// <param name="vectorAction">The actions to take</param>
        public override void OnActionReceived(float[] vectorAction)
        {
            int forward = Mathf.FloorToInt(vectorAction[0]);
            int yaw = Mathf.FloorToInt(vectorAction[1]);

            Vector3 moveVector = Vector3.zero;
            Vector3 rotationVector = Vector3.zero;
            
            switch (forward)
            {
                case 1:
                    moveVector = transform.forward;
                    break;
            }
            
            switch (yaw)
            {
                case 1:
                    rotationVector = transform.up;
                    break;
                case 2:
                    rotationVector = - transform.up;
                    break;
            }
            
            _rBody.AddForce(moveVector * moveForce);
            transform.Rotate(rotationVector, Time.fixedDeltaTime * yawSpeed);
        }

        /// <summary>
        /// Collect vector observations from the environment
        /// </summary>
        /// <param name="sensor">The vector sensor</param>
        public override void CollectObservations(VectorSensor sensor)
        {
            Vector3 localVelocity = transform.InverseTransformDirection(_rBody.velocity);

            //1 observation
            //mean reward lesser without this observation
            sensor.AddObservation(localVelocity.x);
            
            //1 observation
            //mean reward lesser without this observation
            sensor.AddObservation(localVelocity.z);
            
            //4 observations
            //This can be global (no significant difference)
            //Slower training and lesser mean reward without this observation
            sensor.AddObservation(transform.localRotation.normalized);
        }

        public override void Heuristic(float[] actionOut)
        {
            int forward = 0;
            int yaw = 0;

            if (Keyboard.current.wKey.isPressed) forward = 1;
            if (Keyboard.current.rightArrowKey.isPressed) yaw = 1;
            if (Keyboard.current.leftArrowKey.isPressed) yaw = 2;

            actionOut[0] = forward;
            actionOut[1] = yaw;
        }
        
        private void FixedUpdate()
        {
            Feed();
        }

        private void Feed()
        {
            if (Physics.Raycast(nose.position, nose.forward, out RaycastHit hitInfo, groundMask)
                && hitInfo.collider.CompareTag(foodTag))
            {
                Food food = hitInfo.collider.GetComponent<Food>();

                float foodReceived = food.Feed(.01f);
                
                FoodObtained += foodReceived;
                
                AddReward(.01f);
                
                if (hitInfo.collider.TryGetComponent(out Agent agent))
                {
                    //punish herbivores for being eaten
                    agent.AddReward(-.05f);
                }
            }
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("boundary") || other.collider.CompareTag(tag))
            {
                AddReward(-.5f);
            }

//            if (other.collider.CompareTag(foodTag) && other.collider.TryGetComponent(out Agent agent))
//            {
//                //punish herbivores for being eaten
//                agent.AddReward(-.5f);
//            }
        }
        
}
