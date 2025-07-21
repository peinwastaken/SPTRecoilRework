using EFT;
using EFT.Animations;
using PeinRecoilRework.Config.Settings;
using UnityEngine;

namespace PeinRecoilRework.Components
{
    public class LeftStanceComponent : MonoBehaviour
    {
        public static LeftStanceComponent Instance { get; private set; }

        public bool IsLeftStance { get; set; }

        private float _leftStanceTarget = 0f;
        private float _leftStanceMult = 0f;

        private ProceduralWeaponAnimation _pwa;
        private Player _player;

        public bool SetLeftStance(bool newState)
        {
            IsLeftStance = newState;

            return newState;
        }

        public void DoLeftStance(Transform weaponRootAnim, float dt)
        {
            _leftStanceTarget = IsLeftStance ? 1f : 0f;
            _leftStanceMult = Mathf.Lerp(_leftStanceMult, _leftStanceTarget, LeftStanceSettings.LeftStanceSpeed.Value * dt);

            float offset = LeftStanceSettings.LeftStanceOffset.Value;
            float angle = LeftStanceSettings.LeftStanceAngle.Value;
            
            Vector3 leftStanceOffset = new Vector3(-offset * _leftStanceMult, 0f, 0f);
            Quaternion rotationOffset = Quaternion.Euler(0f, -angle * _leftStanceMult, 0f);

            weaponRootAnim.localPosition += leftStanceOffset;
            weaponRootAnim.localRotation *= rotationOffset;
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

            _player = GetComponent<Player>();
            _pwa = _player.ProceduralWeaponAnimation;
        }
    }
}
