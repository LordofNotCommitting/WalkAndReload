//using HarmonyLib;
//using MGSC;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

using HarmonyLib;
using MGSC;

namespace WalkAndReload
{

    [HarmonyPatch(typeof(Player), nameof(Player.OnMoved))]
    public class onMoveFix
    {

        static bool full_Processing = Plugin.ConfigGeneral.ModData.GetConfigValue<bool>("Full_Processing", false);
        static void Postfix(ref Player __instance)
        {
            if (!full_Processing)
            {
                ReloadFuncs.Light_Check(ref __instance);
            }
            else {
                ReloadFuncs.Heavy_Check(ref __instance);
            }
            return;
        }
    }

}

