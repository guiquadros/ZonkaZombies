namespace ZonkaZombies.Managers
{
    public struct GameSceneType
    {
        public GameSceneType(string sceneName, bool unloadAllOtherScenes = true)
        {
            SceneName = sceneName;
            UnloadAllOtherScenes = unloadAllOtherScenes;
        }

        public string SceneName { get; set; }
        public bool UnloadAllOtherScenes { get; set; }
    }
}