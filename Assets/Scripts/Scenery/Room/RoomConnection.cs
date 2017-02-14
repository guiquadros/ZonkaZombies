using Assets.Scripts.Cenario.Room.Node;
using UnityEngine;

namespace Assets.Scripts.Cenario.Room
{
    [System.Serializable]
    public struct RoomConnection
    {
        [SerializeField]
        private NodeBehaviour thisRoomNode;
        public NodeBehaviour ThisRoomNode { get { return thisRoomNode; } }
        [SerializeField]
        private NodeBehaviour otherRoomNode;
        public NodeBehaviour OtherRoomNode { get { return otherRoomNode; } }
    }
}