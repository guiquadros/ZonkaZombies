namespace ZonkaZombies.Managers
{
    public struct GameSceneType
    {
        public GameSceneType(string sceneName, bool unloadAllOtherScenes = true, bool showHud = true, bool showPressStart = false, bool showPlayerMissionCount = false)
        {
            SceneName = sceneName;
            UnloadAllOtherScenes = unloadAllOtherScenes;
            ShowHud = showHud;
            ShowPressStart = showPressStart;
            ShowPlayerMissionCount = showPlayerMissionCount;
        }

        public string SceneName { get; set; }
        public bool UnloadAllOtherScenes { get; set; }
        public bool ShowHud { get; set; }
        public bool ShowPressStart { get; set; }
        public bool ShowPlayerMissionCount { get; set; }
    }
}