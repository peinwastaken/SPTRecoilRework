using PeinRecoilRework.Config.Settings;
using PeinRecoilRework.Helpers;
using UnityEngine;

namespace PeinRecoilRework.Components
{
    public class CameraOffsetComponent : MonoBehaviour
    {
        public RecoilSpring SlowShakeSpring;
        public RecoilSpring FastShakeSpring;
        public RecoilSpring CameraSpring;
        public RecoilSpring WeaponSwaySpring;

        public float InputSwayAngSpeed = 5f;
        public float InputSwaySpeed = 0.1f;
        public float SwayMult = 0.001f;

        private Vector2 mouseDelta;

        public void DoRecoilShake()
        {
            if (AdditionalCameraRecoilSettings.EnableSlowSpring.Value == true)
            {
                Vector2 horizontalMult = AdditionalCameraRecoilSettings.SlowSpringHorizontalMinMax.Value;
                Vector2 verticalMult = AdditionalCameraRecoilSettings.SlowSpringVerticalMinMax.Value;
                Vector2 slowAng = AdditionalCameraRecoilSettings.SlowSpringAngleMinMax.Value;

                float angle = Random.Range(slowAng.x, slowAng.y);
                Vector2 slowDir = Util.GetAngleVector(angle).normalized;

                slowDir.x *= Random.Range(horizontalMult.x, horizontalMult.y);
                slowDir.y *= Random.Range(verticalMult.x, verticalMult.y);

                SlowShakeSpring.ApplyImpulse(slowDir);
            }

            if (AdditionalCameraRecoilSettings.EnableFastSpring.Value == true)
            {
                Vector2 horizontalMult = AdditionalCameraRecoilSettings.FastSpringHorizontalMinMax.Value;
                Vector2 verticalMult = AdditionalCameraRecoilSettings.FastSpringVerticalMinMax.Value;
                Vector2 fastAng = AdditionalCameraRecoilSettings.FastSpringAngleMinMax.Value;

                float angle = Random.Range(fastAng.x, fastAng.y);
                Vector2 fastDir = Util.GetAngleVector(angle).normalized;

                fastDir.x *= Random.Range(horizontalMult.x, horizontalMult.y);
                fastDir.y *= Random.Range(verticalMult.x, verticalMult.y);

                FastShakeSpring.ApplyImpulse(fastDir);
            }

            if (AdditionalCameraRecoilSettings.EnableCameraSpring.Value == true)
            {
                Util.Logger.LogInfo("Applying camera spring");
                Vector3 dir = AdditionalCameraRecoilSettings.CameraSpringDirection.Value;
                Vector2 horizontalMult = AdditionalCameraRecoilSettings.CameraSpringHorizontalMinMax.Value;
                Vector2 verticalMult = AdditionalCameraRecoilSettings.CameraSpringVerticalMinMax.Value;
                Vector2 rotationMult = AdditionalCameraRecoilSettings.CameraSpringRotationMinMax.Value;

                dir.x *= Random.Range(horizontalMult.x, horizontalMult.y);
                dir.y *= Random.Range(verticalMult.x, verticalMult.y);
                dir.z *= Random.Range(rotationMult.x, rotationMult.y);

                CameraSpring.ApplyImpulse(dir);
            }
        }

        private void Start()
        {
            SlowShakeSpring = new RecoilSpring();
            FastShakeSpring = new RecoilSpring();
            CameraSpring = new RecoilSpring();
        }

        private void Update()
        {
            mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            SlowShakeSpring.Update(dt);
            FastShakeSpring.Update(dt);
            CameraSpring.Update(dt);
        }
    }
}
