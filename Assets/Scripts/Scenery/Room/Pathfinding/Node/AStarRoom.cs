namespace Assets.Scripts.Cenario.Room.Pathfinding.Node
{
    public class AStarRoom
    {
        public RoomManager room;
        public AStarRoom parent;

        public AStarRoom(RoomManager room)
        {
            this.room = room;
            parent = null;
        }

        /// <summary>
        /// Define um parent para este node.
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(AStarRoom parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Retorna TRUE caso ambos sejam iguais.
        /// </summary>
        /// <param name="otherRoom"></param>
        /// <returns></returns>
        public bool IsSameAs(AStarRoom otherRoom)
        {
            return this.room.IsSameAs(otherRoom.room);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(AStarRoom))
                return false;

            AStarRoom other = (AStarRoom) obj;
            return this.IsSameAs(other);
        }

        public override int GetHashCode()
        {
            return this.room.GetHashCode();
        }
    }
}