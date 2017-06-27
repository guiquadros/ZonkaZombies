﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZonkaZombies.Messaging;
using ZonkaZombies.Messaging.Messages.UI;
using ZonkaZombies.Multiplayer;
using ZonkaZombies.Spawn;

namespace ZonkaZombies.Scenery
{
    public class TransparencyHandler : MonoBehaviour
    {
        public List<Transform> cameras;
        public List<Transform> players;

        public List<TransparentSceneObject> newCollisions = new List<TransparentSceneObject>();
        public List<TransparentSceneObject> oldCollisions = new List<TransparentSceneObject>();

        public float updateTime = 0.3f;
        public float updateTimeCounter = 0f;

        private void OnEnable()
        {
            MessageRouter.AddListener<SplitScreenCamerasInitializedMessage>(SplitScreenHandler_OnCamerasInitialized);
            MessageRouter.AddListener<OnPlayerSpawnMessage>(OnPlayerMessageCallback);
        }

        private void OnDisable()
        {
            MessageRouter.RemoveListener<SplitScreenCamerasInitializedMessage>(SplitScreenHandler_OnCamerasInitialized);
            MessageRouter.RemoveListener<OnPlayerSpawnMessage>(OnPlayerMessageCallback);
        }

        private void SplitScreenHandler_OnCamerasInitialized(SplitScreenCamerasInitializedMessage splitScreenCamerasInitializedMessage)
        {
            cameras.Add(splitScreenCamerasInitializedMessage.CameraClone.transform);
        }

        private void OnPlayerMessageCallback(OnPlayerSpawnMessage msg)
        {
            players.Add(msg.Player.transform);
        }

        // Update is called once per frame
        private void Update()
        {
            updateTimeCounter += Time.deltaTime;
            if (updateTimeCounter >= updateTime)
            {
                UpdateTransparency();
                updateTimeCounter -= updateTime;
            }
        }
        private void UpdateTransparency ()
        {
            //Cicle the collisions list
            oldCollisions = newCollisions;
            newCollisions = new List<TransparentSceneObject>();

            foreach (var targetCamera in cameras)
            {
                foreach (Transform player in players)
                {
                    if (player == null || player.ToString() == "null") continue;

                    //Get the new collisions
                    RaycastHit[] collisions = Physics.RaycastAll(
                        new Ray(targetCamera.position, Vector3.Normalize(player.position - targetCamera.position)),
                        Vector3.Distance(targetCamera.position, player.position));

                    //Filter to only get objects that can become transparent
                    foreach (RaycastHit hit in collisions)
                    {
                        if (hit.transform.CompareTag("Transparent"))
                            newCollisions.Add(hit.transform.GetComponent<TransparentSceneObject>());
                    }
                }
            }

            //Remove double entrances
            newCollisions = newCollisions.Distinct().ToList();
            
            //Remove recurrent objects from the old list
            for (int i = oldCollisions.Count - 1; i >= 0; i--)
            {
                if (newCollisions.Contains(oldCollisions[i]))
                    oldCollisions.Remove(oldCollisions[i]);
            }

            //Make remaining objects on the old list opaque
            foreach (TransparentSceneObject obj in oldCollisions)
            {
                foreach (Renderer render in obj.objectRenderers)
                {
                    render.material.SetFloat("_Mode", 0f);
                    render.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    render.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    render.material.SetInt("_ZWrite", 1);
                    render.material.DisableKeyword("_ALPHATEST_ON");
                    render.material.DisableKeyword("_ALPHABLEND_ON");
                    render.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    render.material.renderQueue = -1;
                    render.material.color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, 1f);
                }
            }

            //Make objects transparent
            foreach (TransparentSceneObject obj in newCollisions)
            {
                foreach (Renderer render in obj.objectRenderers)
                {
                    render.material.SetFloat("_Mode", 3f);
                    render.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    render.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    render.material.SetInt("_ZWrite", 0);
                    render.material.DisableKeyword("_ALPHATEST_ON");
                    render.material.DisableKeyword("_ALPHABLEND_ON");
                    render.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    render.material.renderQueue = 3000;
                    render.material.color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, 0.05f);
                }
            }
        }
    }
}
