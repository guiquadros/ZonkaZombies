namespace ZonkaZombies.Managers
{
    public struct GameSceneType
    {
        public GameSceneType(string sceneName, bool unloadAllOtherScenes = true, bool showHud = true)
        {
            SceneName = sceneName;
            UnloadAllOtherScenes = unloadAllOtherScenes;
            ShowHud = showHud;
        }

        public string SceneName { get; set; }
        public bool UnloadAllOtherScenes { get; set; }
        public bool ShowHud { get; set; }
    }
}