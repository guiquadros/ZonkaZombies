namespace ZonkaZombies.Managers
{
    public struct GameSceneType
    {
        public GameSceneType(string sceneName, bool unloadAllOtherScenes = true, bool showHud = true, bool showPressStart = false, bool showChocolateMissionCount = false, bool showHallFirstFloorMissions = false)
        {
            SceneName = sceneName;
            UnloadAllOtherScenes = unloadAllOtherScenes;
            ShowHud = showHud;
            ShowPressStart = showPressStart;
            ShowChocolateMissionCount = showChocolateMissionCount;
            ShowHallFirstFloorMissions = showHallFirstFloorMissions;
        }

        public string SceneName { get; set; }
        public bool UnloadAllOtherScenes { get; set; }
        public bool ShowHud { get; set; }
        public bool ShowPressStart { get; set; }
        public bool ShowChocolateMissionCount { get; set; }
        public bool ShowHallFirstFloorMissions { get; set; }
    }
}