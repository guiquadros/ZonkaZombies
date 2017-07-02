using ZonkaZombies.Util;
// ReSharper disable InconsistentNaming

namespace ZonkaZombies.Managers
{
    public struct GameScenes
    {
        public static readonly GameSceneType[] GameScenesOrdered = { MAIN_MENU ,INTRO1, INTRO2, HALL_FIRST_FLOOR, DIALOGUE_SCIENTIST, CITY, DIALOGUE_SCIENTIST, END_GAME1, END_GAME2};

        public static GameSceneType MAIN_MENU
        {
            get { return new GameSceneType(SceneConstants.MAIN_MENU, showHud: false); }
        }

        public static GameSceneType INTRO1
        {
            get { return new GameSceneType(SceneConstants.INTRO1_NAME, showHud: false); }
        }

        public static GameSceneType INTRO2
        {
            get { return new GameSceneType(SceneConstants.INTRO2_NAME, showHud: false); }
        }

        public static GameSceneType HALL_FIRST_FLOOR
        {
            get { return new GameSceneType(SceneConstants.HALL_FIRST_FLOOR_NAME, showPressStart: true, showHallFirstFloorMissions: true); }
        }

        public static GameSceneType DIALOGUE_SCIENTIST
        {
            get { return new GameSceneType(SceneConstants.DIALOGUE_SCIENTIST_NAME/*, false*/); }
        }
        
        public static GameSceneType END_GAME1
        {
            get { return new GameSceneType(SceneConstants.END_GAME_1_SCENE_NAME, showHud: false); }
        }

        public static GameSceneType END_GAME2
        {
            get { return new GameSceneType(SceneConstants.END_GAME_2_SCENE_NAME, showHud: false); }
        }

        public static GameSceneType GAME_OVER_SCENE
        {
            get { return new GameSceneType(SceneConstants.GAME_OVER_SCENE_NAME, showHud: false); }
        }

        public static GameSceneType CITY
        {
            get { return new GameSceneType(SceneConstants.CITY_NAME, showChocolateMissionCount: true); }
        }
    }
}