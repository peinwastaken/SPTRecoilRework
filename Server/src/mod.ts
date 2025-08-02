import { DependencyContainer } from "tsyringe";
import { IPostDBLoadMod } from "@spt/models/external/IPostDBLoadMod";
import { DatabaseServer } from "@spt/servers/DatabaseServer";
import { IDatabaseTables } from "@spt/models/spt/server/IDatabaseTables";
import { ILogger } from "@spt/models/spt/utils/ILogger";
import Util from "./util";
import { LogTextColor } from "@spt/models/spt/logging/LogTextColor";
import { IPreSptLoadMod } from "@spt/models/external/IPreSptLoadMod";
import { StaticRouterModService } from "@spt/services/mod/staticRouter/StaticRouterModService";

class Mod implements IPostDBLoadMod //, IPreSptLoadMod
{
    private cachedWeaponData = []

    /*
    public preSptLoad(container: DependencyContainer): void {
        const staticRouter = container.resolve<StaticRouterModService>("StaticRouterModService")

        staticRouter.registerStaticRouter(
            "getRecoilData",
            [
                {
                    url: "/recoilrework/get/recoildata",
                    action: async (url, info, sessionId, output) => {
                        return JSON.stringify(this.cachedWeaponData)
                    }
                }
            ],
            "PeinRecoilRework"
        )
    }*/

    public postDBLoad(container: DependencyContainer): void
    {
        const logger = container.resolve<ILogger>("WinstonLogger")
        const dbServer = container.resolve<DatabaseServer>("DatabaseServer")
        const tables: IDatabaseTables = dbServer.getTables()
        const templates = tables.templates

        const weaponData = Util.parseDirectory("/../config/weapondata")
        const strings = Util.parseJson(Util.getConfigFile("strings.json"))
        const randomString = strings[Math.floor(Math.random() * strings.length)]

        // do weapon data
        let weaponCount = 0
        weaponData.forEach(weapon => {
            const weaponId = weapon["weaponId"]
            const weaponIds = weapon["weaponIds"]
            const override = weapon["overrideProperties"]

            for (const [property, value] of Object.entries(override)) {
                if (weaponId) {
                    templates.items[weaponId]._props[property] = value
                }
                else if (weaponIds) {
                    weaponIds.forEach(id => {
                        templates.items[id]._props[property] = value
                    });
                }
            }

            weaponCount += weaponIds?.length || 1
        })

        this.cachedWeaponData = weaponData

        logger.logWithColor(`[SPTRecoilRework] Successfully modified data for ${weaponCount} weapons. ${randomString}`, LogTextColor.YELLOW)
    }
}

export const mod = new Mod();