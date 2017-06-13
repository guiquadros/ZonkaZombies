﻿using GlowingObjects.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZonkaZombies.Managers;
using ZonkaZombies.Util;

namespace ZonkaZombies.Scenery.Interaction
{
    public class ElevatorFloorInteractable : InteractableBase
    {
        public override void OnBegin(IInteractor interactor)
        {
            //if all players are inside the elevator and all enemies were killed
            if (Count == EntityManager.Instance.Players.Count && EntityManager.Instance.Enemies.Count <= 0)
            {
                //TODO: execute elevator event
                SceneManager.LoadScene(SceneConstants.PLAYER_WIN_SCENE_NAME);
            }
        }

        public override void OnFinish(IInteractor interactor)
        {
            
        }
    }
}