using EFT;
using PeinRecoilRework.Config.Settings;
using UnityEngine;

namespace PeinRecoilRework.Components
{
    public class RealRecoilComponent : MonoBehaviour
    {
        public Vector2 RecoilDirection = Vector2.zero; // Y - vertical, X - horizontal
        
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            Camera camera = Camera.main;

            RecoilDirection = Vector2.Lerp(RecoilDirection, Vector2.zero, RealRecoilSettings.RealRecoilDecaySpeed.Value * dt);

            _player.Rotate(RecoilDirection, true);
        }

        public Vector2 ApplyRecoil(float verticalAmount, float horizontalAmount, bool randomHorizontal = true)
        {
            float vertical = verticalAmount;
            float horizontal = randomHorizontal ? Random.Range(-horizontalAmount, horizontalAmount) : horizontalAmount;

            RecoilDirection.x = horizontal;
            RecoilDirection.y = -vertical;

            return new Vector2(horizontal, vertical);
        }
    }
}
