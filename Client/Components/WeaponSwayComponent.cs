using PeinRecoilRework.Config.Settings;
using PeinRecoilRework.Helpers;
using UnityEngine;

namespace PeinRecoilRework.Components
{
    public class WeaponSwayComponent : MonoBehaviour
    {
        public static WeaponSwayComponent Instance { get; private set; }

        public RecoilSpring WeaponSwayAngSpring;
        public RecoilSpring WeaponSwayPosSpring;
        public RecoilSpring CameraOffsetSpring;

        private Camera _camera;
        private Vector2 _mouseDelta;

        public void DoWeaponSway(Vector2 dir, float dt)
        {
            dir.x *= WeaponSwaySettings.WeaponSwayMult.Value.x;
            dir.y *= WeaponSwaySettings.WeaponSwayMult.Value.y;

            // x - vertical, y - roll, z - horizontal
            WeaponSwayAngSpring.AddVelocity(new Vector3(dir.y, -dir.x/2, -dir.x) * dt);
            // WeaponSwayPosSpring.ApplyImpulse(new Vector3(dir.x, dir.y, 0) * dt);
        }

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

            WeaponSwayAngSpring = new RecoilSpring();
            WeaponSwayPosSpring = new RecoilSpring();
            CameraOffsetSpring = new RecoilSpring();

            _camera = Camera.main;
        }

        private void Update()
        {
            _mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            DoWeaponSway(_mouseDelta, dt);

            WeaponSwayAngSpring?.Update(dt);
            WeaponSwayPosSpring?.Update(dt);
            CameraOffsetSpring?.Update(dt);
        }
    }
}
