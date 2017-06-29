namespace ZonkaZombies.Managers
{
    public struct GameSceneType
    {
        public GameSceneType(string sceneName, bool unloadAllOtherScenes = true, bool showHud = true, bool showPressStart = false)
        {
            SceneName = sceneName;
            UnloadAllOtherScenes = unloadAllOtherScenes;
            ShowHud = showHud;
            ShowPressStart = showPressStart;
        }

        public string SceneName { get; set; }
        public bool UnloadAllOtherScenes { get; set; }
        public bool ShowHud { get; set; }
        public bool ShowPressStart { get; set; }
    }
}