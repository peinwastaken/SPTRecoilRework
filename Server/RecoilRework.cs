using Microsoft.AspNetCore.Mvc.Rendering;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils;
using SPTRecoilReworkServerMod.Models;
using System.Reflection;
using Path = System.IO.Path;

namespace SPTRecoilReworkServerMod
{
    [Injectable(InjectionType.Singleton, TypePriority = OnLoadOrder.PostDBModLoader + 1)]
    public class RecoilRework(ISptLogger<RecoilRework> logger, DatabaseService dbService, JsonUtil jsonUtil, ModHelper modHelper) : IOnLoad
    {
        public Task OnLoad()
        {
            string modPath = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
            string configPath = Path.Combine(modPath, "config");
            string weaponDataPath = Path.Combine(configPath, "weapondata");
            
            List<string> randomStrings = modHelper.GetJsonDataFromFile<List<string>>(configPath, "strings.json");
            string[] weaponDataJsons = Directory.GetFiles(weaponDataPath);
            Dictionary<MongoId, TemplateItem> dbItems = dbService.GetItems();
            int configCount = 0;
            
            foreach (string weaponDataJson in weaponDataJsons)
            {
                string fileName = Path.GetFileName(weaponDataJson);
                List<WeaponOverrideConfig>? overrideConfigs = jsonUtil.DeserializeFromFile<List<WeaponOverrideConfig>>(weaponDataJson);

                if (overrideConfigs == null) continue;

                foreach (WeaponOverrideConfig overrideConfig in overrideConfigs)
                {
                    if (overrideConfig.WeaponId != null)
                    {
                        UpdateBaseItemPropertiesWithOverrides(overrideConfig.OverrideProperties, dbItems[overrideConfig.WeaponId]);
                        configCount++;
                    }

                    if (overrideConfig.WeaponIds != null)
                    {
                        foreach (string weaponId in overrideConfig.WeaponIds)
                        {
                            UpdateBaseItemPropertiesWithOverrides(overrideConfig.OverrideProperties, dbItems[weaponId]);
                            configCount++;
                        }
                    }
                }
            }
            
            logger.LogWithColor($"Successfully loaded Recoil Rework! Loaded custom weapon data for {configCount} weapons. {randomStrings[new Random().Next(0, randomStrings.Count)]}", LogTextColor.Green);
            
            return Task.CompletedTask;
        }
        
        private static void UpdateBaseItemPropertiesWithOverrides(TemplateItemProperties? overrideProperties, TemplateItem itemClone)
        {
            if (overrideProperties is null || itemClone?.Properties is null)
                return;

            var target = itemClone.Properties;
            var targetType = target.GetType();

            foreach (var member in overrideProperties.GetType().GetMembers())
            {
                var value = member.MemberType switch
                {
                    MemberTypes.Property => ((PropertyInfo)member).GetValue(overrideProperties),
                    MemberTypes.Field => ((FieldInfo)member).GetValue(overrideProperties),
                    _ => null,
                };

                if (value is null)
                {
                    continue;
                }

                var targetMember = targetType.GetMember(member.Name).FirstOrDefault();
                if (targetMember is null)
                {
                    continue;
                }

                switch (targetMember.MemberType)
                {
                    case MemberTypes.Property:
                        var prop = (PropertyInfo)targetMember;
                        if (prop.CanWrite)
                        {
                            prop.SetValue(target, value);
                        }

                        break;

                    case MemberTypes.Field:
                        var field = (FieldInfo)targetMember;
                        if (!field.IsInitOnly)
                        {
                            field.SetValue(target, value);
                        }

                        break;
                }
            }
        }
    }
}
