using ZonkaZombies.Util;
// ReSharper disable InconsistentNaming

namespace ZonkaZombies.Managers
{
    public struct GameScenes
    {
        public static readonly GameSceneType[] GameScenesOrdered = {HALL_FIRST_FLOOR, DIALLOGUE_SCIENTIST};

        public static GameSceneType HALL_FIRST_FLOOR
        {
            get { return new GameSceneType(SceneConstants.HALL_FIRST_FLOOR_NAME); }
        }

        public static GameSceneType DIALLOGUE_SCIENTIST
        {
            get { return new GameSceneType(SceneConstants.DIALLOGUE_SCIENTIST_NAME/*, false*/); }
        }
        
        public static GameSceneType PLAYER_WIN_SCENE
        {
            get { return new GameSceneType(SceneConstants.PLAYER_WIN_SCENE_NAME); }
        }
        
        public static GameSceneType GAME_OVER_SCENE
        {
            get { return new GameSceneType(SceneConstants.GAME_OVER_SCENE_NAME); }
        }
    }
}