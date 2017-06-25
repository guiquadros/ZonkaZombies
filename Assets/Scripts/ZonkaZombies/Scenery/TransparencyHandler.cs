using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZonkaZombies.Multiplayer;

namespace ZonkaZombies.Scenery
{
    public class TransparencyHandler : MonoBehaviour
    {
        public List<Transform> cameras;
        public List<Transform> players;

        public List<TransparentSceneObject> newCollisions = new List<TransparentSceneObject>();
        public List<TransparentSceneObject> oldCollisions = new List<TransparentSceneObject>();

        //[SerializeField]
        //private SplitscreenHandler _splitscreenHandler;

        //private void Awake()
        //{
        //    //_splitscreenHandler.
        //}

        // Update is called once per frame
        private void Update ()
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
                    render.material.SetColor("_Color", new Color(render.material.color.r, render.material.color.g, render.material.color.b, 1f));
                }
            }

            //Make objects transparent
            foreach (TransparentSceneObject obj in newCollisions)
            {
                foreach (Renderer render in obj.objectRenderers)
                {
                    render.material.SetColor("_Color", new Color(render.material.color.r, render.material.color.g, render.material.color.b, 0.05f));
                }
            }
        }
    }
}
