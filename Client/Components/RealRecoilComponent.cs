using EFT;
using SPTRecoilRework.Config.Settings;
using SPTRecoilRework.Helpers;
using UnityEngine;

namespace SPTRecoilRework.Components
{
    public class RealRecoilComponent : MonoBehaviour
    {
        public static RealRecoilComponent Instance { get; private set; }

        public RecoilSpring RecoilSpring { get; private set; }
        public Vector2 RecoilDirection = Vector2.zero;
        
        private Player _player;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }

            _player = GetComponent<Player>();
            RecoilSpring = new RecoilSpring()
            {
                Damping = 0.2f,
                Stiffness = 15f,
                Speed = 30f
            };
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            if (RealRecoilSettings.EnableRealRecoilAlternateSystem.Value == true)
            {
                RecoilSpring.Update(dt);

                Vector2 dir = new Vector2(RecoilSpring.Velocity.x, RecoilSpring.Velocity.y) * dt;

                _player.Rotate(dir, true);
            }
            else
            {
                RecoilDirection = Vector2.Lerp(RecoilDirection, Vector2.zero, RealRecoilSettings.RealRecoilDecaySpeed.Value * dt);

                _player.Rotate(RecoilDirection, true);
            }
        }

        public Vector2 ApplyRecoil(float verticalAmount, float horizontalAmount, bool randomHorizontal = true)
        {
            float vertical = verticalAmount;
            float horizontal = randomHorizontal ? Random.Range(-horizontalAmount, horizontalAmount) : horizontalAmount;

            if (RealRecoilSettings.EnableRealRecoilAlternateSystem.Value == true)
            {
                RecoilSpring.ApplyImpulse(new Vector3(horizontal, -vertical, 0) * 100);
            }
            else
            {
                RecoilDirection.x = horizontal;
                RecoilDirection.y = -vertical;
            }

            return new Vector2(horizontal, vertical);
        }
    }
}
