using PeinRecoilRework.Config.Settings;
using PeinRecoilRework.Helpers;
using UnityEngine;

namespace PeinRecoilRework.Components
{
    public class CameraOffsetComponent : MonoBehaviour
    {
        public static CameraOffsetComponent Instance { get; private set; }

        public RecoilSpring SlowShakeSpring;
        public RecoilSpring FastShakeSpring;
        public RecoilSpring CameraSpring;

        public void DoRecoilShake(Vector2? dir = null, float intensity = 1f)
        {
            Vector2 recoilDir = dir ?? Vector2.zero;

            DebugLogger.Log($"Recoil direction: {recoilDir}");

            if (AdditionalCameraRecoilSettings.EnableSlowSpring.Value == true)
            {
                bool useRealRecoil = AdditionalCameraRecoilSettings.SlowSpringUseRealRecoilDir.Value;
                Vector2 slowAng = AdditionalCameraRecoilSettings.SlowSpringAngleMinMax.Value;

                float angle = Random.Range(slowAng.x, slowAng.y);
                Vector2 slowDir = useRealRecoil ? recoilDir : Util.GetAngleVector(angle).normalized;

                if (useRealRecoil)
                {
                    if (AdditionalCameraRecoilSettings.SlowSpringRealRecoilDirNormalize.Value == true)
                    {
                        slowDir = slowDir.normalized;
                    }

                    Vector2 horizontalMult = AdditionalCameraRecoilSettings.SlowSpringRealRecoilHorizontalMinMax.Value * intensity;
                    Vector2 verticalMult = AdditionalCameraRecoilSettings.SlowSpringRealRecoilVerticalMinMax.Value * intensity;

                    slowDir.x *= Random.Range(horizontalMult.x, horizontalMult.y);
                    slowDir.y *= Random.Range(verticalMult.x, verticalMult.y);
                }
                else
                {
                    Vector2 horizontalMult = AdditionalCameraRecoilSettings.SlowSpringHorizontalMinMax.Value * intensity;
                    Vector2 verticalMult = AdditionalCameraRecoilSettings.SlowSpringVerticalMinMax.Value * intensity;

                    slowDir.x *= Random.Range(horizontalMult.x, horizontalMult.y);
                    slowDir.y *= Random.Range(verticalMult.x, verticalMult.y);
                }

                SlowShakeSpring.ApplyImpulse(slowDir);
            }

            if (AdditionalCameraRecoilSettings.EnableFastSpring.Value == true)
            {
                bool useRealRecoil = AdditionalCameraRecoilSettings.FastSpringUseRealRecoilDir.Value;
                Vector2 fastAng = AdditionalCameraRecoilSettings.FastSpringAngleMinMax.Value;

                float angle = Random.Range(fastAng.x, fastAng.y);
                Vector2 fastDir = useRealRecoil ? recoilDir : Util.GetAngleVector(angle).normalized;

                if (useRealRecoil)
                {
                    if (AdditionalCameraRecoilSettings.FastSpringRealRecoilDirNormalize.Value == true)
                    {
                        fastDir = fastDir.normalized;
                    }

                    Vector2 horizontalMult = AdditionalCameraRecoilSettings.FastSpringRealRecoilHorizontalMinMax.Value * intensity;
                    Vector2 verticalMult = AdditionalCameraRecoilSettings.FastSpringRealRecoilVerticalMinMax.Value * intensity;

                    fastDir.x *= Random.Range(horizontalMult.x, horizontalMult.y);
                    fastDir.y *= Random.Range(verticalMult.x, verticalMult.y);
                }
                else
                {
                    Vector2 horizontalMult = AdditionalCameraRecoilSettings.FastSpringHorizontalMinMax.Value * intensity;
                    Vector2 verticalMult = AdditionalCameraRecoilSettings.FastSpringVerticalMinMax.Value * intensity;

                    fastDir.x *= Random.Range(horizontalMult.x, horizontalMult.y);
                    fastDir.y *= Random.Range(verticalMult.x, verticalMult.y);
                }

                FastShakeSpring.ApplyImpulse(fastDir);
            }

            if (AdditionalCameraRecoilSettings.EnableCameraSpring.Value == true)
            {
                Vector3 camDir = AdditionalCameraRecoilSettings.CameraSpringDirection.Value;
                Vector2 horizontalMult = AdditionalCameraRecoilSettings.CameraSpringHorizontalMinMax.Value;
                Vector2 verticalMult = AdditionalCameraRecoilSettings.CameraSpringVerticalMinMax.Value;
                Vector2 rotationMult = AdditionalCameraRecoilSettings.CameraSpringRotationMinMax.Value;

                camDir.x *= Random.Range(horizontalMult.x, horizontalMult.y);
                camDir.y *= Random.Range(verticalMult.x, verticalMult.y);
                camDir.z *= Random.Range(rotationMult.x, rotationMult.y);

                CameraSpring.ApplyImpulse(camDir);
            }
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

            SlowShakeSpring = new RecoilSpring();
            FastShakeSpring = new RecoilSpring();
            CameraSpring = new RecoilSpring();
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            SlowShakeSpring.Update(dt);
            FastShakeSpring.Update(dt);
            CameraSpring.Update(dt);
        }
    }
}
