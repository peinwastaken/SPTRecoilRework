using PeinRecoilRework.Data;
using SPT.Common.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace PeinRecoilRework.Helpers
{
    public class RouteHelper
    {
        public static List<WeaponRecoilData> FetchWeaponDataFromServer()
        {
            try
            {
                string route = "/recoilrework/get/recoildata";
                var json = RequestHandler.GetJson(route);
                return JsonConvert.DeserializeObject<List<WeaponRecoilData>>(json);
            }
            catch (Exception e)
            {
                Util.Logger.LogError($"Failed to fetch weapon data from server: {e.Message}");
                return new List<WeaponRecoilData>();
            }
        }
    }
}
