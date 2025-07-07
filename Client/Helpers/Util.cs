using BepInEx.Logging;
using Comfort.Common;
using EFT;
using UnityEngine;

namespace PeinRecoilRework.Helpers
{
    public static class Util
    {
        public static ManualLogSource Logger { get; set; }
        private static Player.FirearmController _firearmController;
        private static GameWorld _gameWorld;

        public static Player GetLocalPlayer()
        {
            GameWorld gameWorld = Singleton<GameWorld>.Instance;
            if (gameWorld == null || gameWorld.MainPlayer == null)
            {
                return null;
            }

            return gameWorld.MainPlayer;
        }

        public static Player.FirearmController GetFirearmController()
        {
            if (_firearmController == null)
            {
                Player localPlayer = GetLocalPlayer();
                if (localPlayer != null)
                {
                    Player.FirearmController comp = localPlayer.GetComponent<Player.FirearmController>();
                    if (comp != null)
                    {
                        _firearmController = comp;
                    }
                }
                else
                {
                    return null;
                }
            }

            return _firearmController;
        }

        public static GameWorld GetGameWorld()
        {
            if (_gameWorld == null)
            {
                _gameWorld = Singleton<GameWorld>.Instance;
            }

            return _gameWorld;
        }
    }
}
