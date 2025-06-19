using BepInEx.Logging;
using Comfort.Common;
using EFT;

namespace PeinRecoilRework.Helpers
{
    public static class Util
    {
        public static ManualLogSource Logger { get; set; }

        public static Player GetLocalPlayer()
        {
            GameWorld gameWorld = Singleton<GameWorld>.Instance;
            if (gameWorld == null || gameWorld.MainPlayer == null)
            {
                return null;
            }

            return gameWorld.MainPlayer;
        }
    }
}
