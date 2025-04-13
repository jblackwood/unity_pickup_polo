using System.Collections.Generic;
using UnityEngine;

namespace PickupPolo
{
    public class CollisionHandler : MonoBehaviour
    {
        public GameObject athleteObj;

        void OnCollisionEnter(Collision collision)
        {
            GameLoop.state.entity_collisions.Add((athleteObj.name,collision.gameObject.name));
        }
    }
}

