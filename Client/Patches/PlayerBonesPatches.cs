using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using SPTRecoilRework.Helpers;
using System.Reflection;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace SPTRecoilRework.Patches
{
    public class ShiftWeaponRootPatch : ModulePatch
    {
        private static FieldInfo _targetLSInAimPositionField;
        private static FieldInfo _offsterDirrectionForLeftStanceInAimField;
        private static FieldInfo _offsetForRootAimInLeftStanceField;
        private static FieldInfo _lastWeaponRotationInLeftStanceField;
        private static FieldInfo _targetLSPositionField;
        private static FieldInfo _gotFullLeftStanceLastFrameField;
        private static FieldInfo _wasMovingInLsAimField;

        protected override MethodBase GetTargetMethod()
        {
            _targetLSInAimPositionField = AccessTools.Field(typeof(PlayerBones), "_targetLSInAimPosition");
            _offsterDirrectionForLeftStanceInAimField = AccessTools.Field(typeof(PlayerBones), "_offsterDirrectionForLeftStanceInAim");
            _offsetForRootAimInLeftStanceField = AccessTools.Field(typeof(PlayerBones), "_offsetForRootAimInLeftStance");
            _lastWeaponRotationInLeftStanceField = AccessTools.Field(typeof(PlayerBones), "_lastWeaponRotationInLeftStance");
            _targetLSPositionField = AccessTools.Field(typeof(PlayerBones), "targetLSPosition");
            _gotFullLeftStanceLastFrameField = AccessTools.Field(typeof(PlayerBones), "_gotFullLeftStanceLastFrame");
            _wasMovingInLsAimField = AccessTools.Field(typeof(PlayerBones), "_wasMovingInLsAim");

            return AccessTools.Method(typeof(PlayerBones), nameof(PlayerBones.ShiftWeaponRoot));
        }

        [PatchPrefix]
        private static bool PatchPrefix(
            PlayerBones __instance, float t, EPointOfView pv, float thirdPersonAuthority,
            bool armsupdated, float positionCacheValue, float leftStanceCurveValue,
            bool inSprint, bool inLeftStanceAnimValue, bool inLeftStanceCacheValue,
            ref bool isAiming, bool isAnimatedInteraction)
        {
            var weaponRootAnim = __instance.Weapon_Root_Anim;
            var weaponRootThird = __instance.Weapon_Root_Third;

            if (WeaponHelper.IsPistolCurrentlyEquipped)
            {
                isAiming = false;
            }

            if (pv != EPointOfView.FirstPerson || weaponRootAnim == null)
            {
                if (pv != EPointOfView.FirstPerson)
                {
                    _lastWeaponRotationInLeftStanceField.SetValue(__instance, weaponRootAnim.rotation);
                    _targetLSPositionField.SetValue(__instance, weaponRootAnim.position);
                    _targetLSInAimPositionField.SetValue(__instance, weaponRootAnim.localPosition);

                    if (weaponRootAnim != null && weaponRootThird != null)
                    {
                        Vector3 pos = Vector3.Lerp(weaponRootAnim.position, weaponRootThird.position + weaponRootThird.rotation * __instance.Offset, thirdPersonAuthority);
                        Quaternion rot = Quaternion.Lerp(weaponRootAnim.rotation, weaponRootThird.rotation * __instance.DeltaRotation, thirdPersonAuthority);
                        weaponRootAnim.SetPositionAndRotation(pos, rot);
                        return false;
                    }
                }
                else
                {
                    _lastWeaponRotationInLeftStanceField.SetValue(__instance, weaponRootAnim.rotation);
                    _targetLSPositionField.SetValue(__instance, weaponRootAnim.position);
                    _targetLSInAimPositionField.SetValue(__instance, weaponRootAnim.localPosition);
                }

                return false;
            }

            if (leftStanceCurveValue >= 1f && !inSprint)
            {
                if (isAiming && __instance.Player.InputDirection.sqrMagnitude > 0.001f)
                    _wasMovingInLsAimField.SetValue(__instance, true);

                bool wasMovingInAim = (bool)_wasMovingInLsAimField.GetValue(__instance);

                if (isAiming && !wasMovingInAim)
                {
                    Vector3 target = Vector3.Lerp(
                        (Vector3)_targetLSPositionField.GetValue(__instance),
                        weaponRootThird.position + weaponRootThird.rotation * __instance.Offset,
                        t * __instance.leftStanceRotationAndPositionChangeSpeed
                    );
                    _targetLSPositionField.SetValue(__instance, target);
                }
                else
                {
                    _targetLSPositionField.SetValue(__instance, weaponRootThird.position + weaponRootThird.rotation * __instance.Offset);
                }

                weaponRootAnim.position = (Vector3)_targetLSPositionField.GetValue(__instance);

                // this part fucks with pistol left stance shooting, guessing its because of recoil position but unsure
                if (isAiming && WeaponHelper.IsPistolCurrentlyEquipped == false)
                {
                    Vector3 aimTarget = Vector3.Lerp(
                        (Vector3)_targetLSInAimPositionField.GetValue(__instance),
                        __instance.Ribcage.Original.localPosition +
                            (Vector3)_offsterDirrectionForLeftStanceInAimField.GetValue(__instance) *
                            (float)_offsetForRootAimInLeftStanceField.GetValue(__instance),
                        t
                    );
                    _targetLSInAimPositionField.SetValue(__instance, aimTarget);
                    weaponRootAnim.localPosition = aimTarget;

                    Quaternion oldRot = weaponRootAnim.rotation;
                    Vector3 localEuler = weaponRootAnim.localEulerAngles;

                    weaponRootAnim.rotation = Quaternion.Lerp(
                        (Quaternion)_lastWeaponRotationInLeftStanceField.GetValue(__instance),
                        oldRot,
                        t * __instance.leftStanceRotationAndPositionChangeSpeed
                    );

                    weaponRootAnim.localEulerAngles = new Vector3(localEuler.x, weaponRootAnim.localEulerAngles.y, localEuler.z);
                    weaponRootAnim.localPosition += __instance.Player.ProceduralWeaponAnimation.Shootingg.CurrentRecoilEffect.GetHandPositionRecoil();
                }
                else
                {
                    Vector3 localEuler = weaponRootAnim.localEulerAngles;
                    if ((bool)_gotFullLeftStanceLastFrameField.GetValue(__instance))
                    {
                        _lastWeaponRotationInLeftStanceField.SetValue(__instance, weaponRootThird.rotation * __instance.DeltaRotation);
                    }
                    else
                    {
                        Quaternion last = (Quaternion)_lastWeaponRotationInLeftStanceField.GetValue(__instance);
                        float speed = !inLeftStanceAnimValue ? __instance.leftStanceReturnBackSpeed : __instance.leftStanceRotationAndPositionChangeSpeed;
                        Quaternion lerped = Quaternion.Lerp(last, weaponRootThird.rotation * __instance.DeltaRotation, t * speed);
                        _lastWeaponRotationInLeftStanceField.SetValue(__instance, lerped);
                    }

                    weaponRootAnim.rotation = (Quaternion)_lastWeaponRotationInLeftStanceField.GetValue(__instance);
                    weaponRootAnim.localEulerAngles = new Vector3(localEuler.x, weaponRootAnim.localEulerAngles.y, localEuler.z);
                }

                _lastWeaponRotationInLeftStanceField.SetValue(__instance, weaponRootAnim.rotation);
                _gotFullLeftStanceLastFrameField.SetValue(__instance, false);
                return false;
            }

            _gotFullLeftStanceLastFrameField.SetValue(__instance, true);
            _wasMovingInLsAimField.SetValue(__instance, false);
            Vector3 defaultEuler = weaponRootAnim.localEulerAngles;

            if (positionCacheValue <= 0f)
            {
                if (isAnimatedInteraction)
                {
                    weaponRootAnim.SetPositionAndRotation(
                        Vector3.Lerp(weaponRootAnim.position, weaponRootThird.position + weaponRootThird.rotation * __instance.Offset, thirdPersonAuthority),
                        Quaternion.Lerp(weaponRootAnim.rotation, weaponRootThird.rotation, thirdPersonAuthority)
                    );
                    weaponRootAnim.localRotation = __instance.DeltaRotation;
                }
                else
                {
                    float interp = Mathf.Max(leftStanceCurveValue, thirdPersonAuthority);
                    weaponRootAnim.SetPositionAndRotation(
                        Vector3.Lerp(weaponRootAnim.position, weaponRootThird.position + weaponRootThird.rotation * __instance.Offset, interp),
                        Quaternion.Lerp(weaponRootAnim.rotation, weaponRootThird.rotation * __instance.DeltaRotation, interp)
                    );
                }

                if (leftStanceCurveValue > 0f && !inSprint)
                    weaponRootAnim.localEulerAngles = new Vector3(defaultEuler.x, weaponRootAnim.localEulerAngles.y, defaultEuler.z);
            }
            else
            {
                weaponRootAnim.SetPositionAndRotation(
                    Vector3.Lerp(weaponRootAnim.position, weaponRootThird.position + weaponRootThird.rotation * __instance.Offset, positionCacheValue),
                    Quaternion.Lerp(weaponRootAnim.rotation, weaponRootThird.rotation * __instance.DeltaRotation, positionCacheValue)
                );

                if (leftStanceCurveValue > 0f && !inSprint)
                    weaponRootAnim.localEulerAngles = new Vector3(defaultEuler.x, weaponRootAnim.localEulerAngles.y, defaultEuler.z);
            }

            _targetLSPositionField.SetValue(__instance, weaponRootAnim.position);
            _targetLSInAimPositionField.SetValue(__instance, weaponRootAnim.localPosition);
            _lastWeaponRotationInLeftStanceField.SetValue(__instance, weaponRootAnim.rotation);

            return false; // skip original
        }
    }
}