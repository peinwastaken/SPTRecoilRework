using EFT.Animations;
using PeinRecoilRework.Config.Settings;
using PeinRecoilRework.Helpers;
using UnityEngine;

namespace PeinRecoilRework.Components
{
    public class LeftStanceComponent : MonoBehaviour
    {
        public static LeftStanceComponent Instance { get; private set; }

        public bool IsLeftStance { get; set; }
        public Vector3 CurrentLeftStanceOffset = Vector3.zero;
        public Vector3 CurrentLeftStanceAngle = Vector3.zero;

        private float _leftStanceTarget = 0f;
        private float _leftStanceMult = 0f;

        public bool SetLeftStance(bool newState)
        {
            IsLeftStance = newState;

            return newState;
        }

        public void ZeroAdjustments(ProceduralWeaponAnimation pwa, float dt)
        {
            Spring handsPosition = pwa.HandsContainer.HandsPosition;
            Spring handsRotation = pwa.HandsContainer.HandsRotation;
            Vector3 posOffset = CurrentLeftStanceOffset;
            Vector3 angOffset = CurrentLeftStanceAngle;

            DebugLogger.LogInfo("ZeroAdjustments called");

            handsPosition.Zero += posOffset;
            handsRotation.Zero += angOffset;
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            _leftStanceTarget = IsLeftStance ? 1f : 0f;
            _leftStanceMult = Mathf.Lerp(_leftStanceMult, _leftStanceTarget, LeftStanceSettings.LeftStanceSpeed.Value * dt);

            float offset = LeftStanceSettings.LeftStanceOffset.Value;
            float angle = LeftStanceSettings.LeftStanceAngle.Value;

            CurrentLeftStanceOffset = new Vector3(-offset * _leftStanceMult, 0f, 0f);
            CurrentLeftStanceAngle = new Vector3(0f, -angle * _leftStanceMult, 0f);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
