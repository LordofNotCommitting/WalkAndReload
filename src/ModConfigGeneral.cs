using MGSC;
using ModConfigMenu.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WalkAndReload
{
    // Token: 0x02000006 RID: 6
    public class ModConfigGeneral
    {
        // Token: 0x0600001D RID: 29 RVA: 0x00002840 File Offset: 0x00000A40
        public ModConfigGeneral(string ModName, string ConfigPath)
        {
            this.ModName = ModName;
            this.ModData = new ModConfigData(ConfigPath);
            this.ModData.AddConfigHeader("STRING:General Settings", "general");

            this.ModData.AddConfigValue("general", "about_1", "STRING:lite check will only check if weapon reload time = 1AP and if you have any +reload implants. Enabling Full Reload time processing will factor in all vest/servo/skills to see if reload time = 1 and may impact ingame performance.\n");
            this.ModData.AddConfigValue("general", "Full_Processing", false, "STRING:Full Reload time processing", "STRING:Turn on full reload duration calculation with weapon reload/implants/vest/servo/skills to see if reload time = 1.\n");

            this.ModData.AddConfigValue("general", "about_2", "STRING:Full Reload time processing will apply vest bonus even if an ammo is not from the vest to save performance.\n");
            this.ModData.AddConfigValue("general", "about_final", "STRING:<color=#f51b1b>The game must be restarted after setting then saving this config to take effect.</color>\n");
            this.ModData.RegisterModConfigData(ModName);
        }

        // Token: 0x04000011 RID: 17
        private string ModName;

        // Token: 0x04000012 RID: 18
        public ModConfigData ModData;

    }
}
