using HarmonyLib;
using MGSC;
using System;
using System.Numerics;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

namespace WalkAndReload
{

    [HarmonyPatch(typeof(Player))]
    public class ReloadFuncs
    {

        public static void Light_Check(ref Player temp_player)
        {
            if (temp_player.CreatureData.Inventory.CurrentWeapon != null && temp_player.CreatureData.Inventory.CurrentWeapon.Record<WeaponRecord>().WeaponClass != WeaponClass.GrenadeLauncher && !temp_player.CreatureData.Inventory.CurrentWeapon.Locked)
            {
                WeaponRecord weaponRecord = temp_player.CreatureData.Inventory.CurrentWeapon.Record<WeaponRecord>();
                if ((weaponRecord.ReloadDuration == 1))
                {
                    int woundeffect_reload = Mathf.RoundToInt(temp_player.CreatureData.EffectsController.SumEffectsValue<WoundEffectReloadDuration>((WoundEffectReloadDuration w) => (float)w.Value));

                    int woundeffect_actiondamage = Mathf.RoundToInt(temp_player.CreatureData.EffectsController.SumEffectsValue<WoundEffectActionDamage>((WoundEffectActionDamage w) => (float)w.DamagePerTurn));
                    
                    if (woundeffect_reload <= 0 && woundeffect_actiondamage == 0)
                    {
                        if (ReloadWeaponSystem.CanReload(temp_player.CreatureData.Inventory, temp_player.CreatureData.Inventory.CurrentWeapon, temp_player.CreatureData.Inventory.Storages, true))
                        {
                            WeaponComponent weaponComponent = temp_player.CreatureData.Inventory.CurrentWeapon.Comp<WeaponComponent>();
                            WeaponDescriptor weaponDescriptor = temp_player.CreatureData.Inventory.CurrentWeapon.View<WeaponDescriptor>();
                            SingletonMonoBehaviour<SoundController>.Instance.PlaySound(temp_player.GetSoundContext(), weaponDescriptor.RandomReloadSoundBank, false, 0f);
                            bool flag2 = false;
                            short currentAmmo = weaponComponent.CurrentAmmo;
                            ReloadWeaponSystem.Reload(temp_player.CreatureData.Inventory.CurrentWeapon, temp_player.CreatureData.Inventory.Storages, out flag2, false);
                            temp_player._weaponsToReload.Add(temp_player.CreatureData.Inventory.CurrentWeapon);
                            temp_player.CreatureData.Inventory.CurrentWeapon.LockCounter = (byte)1;
                            weaponComponent.LastReloadAmount = weaponComponent.CurrentAmmo;
                            weaponComponent.CurrentAmmo = currentAmmo;
                            temp_player.CreatureData.Inventory.CheckLockStates();
                            temp_player.RefreshVisualAfterReloadOperation();
                            temp_player.CreatureData.Inventory.RecalculateCurrentWeight();
                        }
                    }


                }
            }
            return;
        }

        public static void Heavy_Check(ref Player temp_player)
        {
            if (temp_player.CreatureData.Inventory.CurrentWeapon != null && temp_player.CreatureData.Inventory.CurrentWeapon.Record<WeaponRecord>().WeaponClass != WeaponClass.GrenadeLauncher && !temp_player.CreatureData.Inventory.CurrentWeapon.Locked)
            {
                int woundeffect_actiondamage = Mathf.RoundToInt(temp_player.CreatureData.EffectsController.SumEffectsValue<WoundEffectActionDamage>((WoundEffectActionDamage w) => (float)w.DamagePerTurn));

                if (woundeffect_actiondamage == 0) {
                    WeaponRecord weaponRecord = temp_player.CreatureData.Inventory.CurrentWeapon.Record<WeaponRecord>();
                    int woundeffect_reload = Mathf.RoundToInt(temp_player.CreatureData.EffectsController.SumEffectsValue<WoundEffectReloadDuration>((WoundEffectReloadDuration w) => (float)w.Value));

                    int reload_timer = 0;
                    BasePickupItem first = temp_player.CreatureData.Inventory.VestSlot.First;
                    VestRecord vestRecord = (first != null) ? first.Record<VestRecord>() : null;
                    int backpack_bonus = 0;
                    if (temp_player.CreatureData.Inventory.CurrentWeapon.Storage == temp_player.CreatureData.Inventory.ServoArmSlot)
                    {
                        BasePickupItem first2 = temp_player.CreatureData.Inventory.BackpackSlot.First;
                        if (first2 != null)
                        {
                            BackpackRecord backpackRecord = first2.Record<BackpackRecord>();
                            backpack_bonus += backpackRecord.ReloadTurnMod;
                        }
                    }
                    int temp_sum = weaponRecord.ReloadDuration + ((vestRecord != null) ? vestRecord.ReloadTurnMod : 0) + backpack_bonus + temp_player.Mercenary.CreatureData.ReloadBonus + woundeffect_reload;
                    reload_timer = Mathf.Max(temp_sum, 1);
                    if (reload_timer == 1)
                    {
                        if (ReloadWeaponSystem.CanReload(temp_player.CreatureData.Inventory, temp_player.CreatureData.Inventory.CurrentWeapon, temp_player.CreatureData.Inventory.Storages, true))
                        {
                            WeaponComponent weaponComponent = temp_player.CreatureData.Inventory.CurrentWeapon.Comp<WeaponComponent>();
                            WeaponDescriptor weaponDescriptor = temp_player.CreatureData.Inventory.CurrentWeapon.View<WeaponDescriptor>();
                            SingletonMonoBehaviour<SoundController>.Instance.PlaySound(temp_player.GetSoundContext(), weaponDescriptor.RandomReloadSoundBank, false, 0f);
                            bool flag2 = false;
                            short currentAmmo = weaponComponent.CurrentAmmo;
                            ReloadWeaponSystem.Reload(temp_player.CreatureData.Inventory.CurrentWeapon, temp_player.CreatureData.Inventory.Storages, out flag2, false);
                            temp_player._weaponsToReload.Add(temp_player.CreatureData.Inventory.CurrentWeapon);
                            temp_player.CreatureData.Inventory.CurrentWeapon.LockCounter = (byte)1;
                            weaponComponent.LastReloadAmount = weaponComponent.CurrentAmmo;
                            weaponComponent.CurrentAmmo = currentAmmo;
                            temp_player.CreatureData.Inventory.CheckLockStates();
                            temp_player.RefreshVisualAfterReloadOperation();
                            temp_player.CreatureData.Inventory.RecalculateCurrentWeight();
                        }
                    }
                }
            }
            return;
        }
    }

}
