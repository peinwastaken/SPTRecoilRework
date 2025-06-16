import { DependencyContainer } from "tsyringe";
import { IPostDBLoadMod } from "@spt/models/external/IPostDBLoadMod";
import path from "path";
import { DatabaseServer } from "@spt/servers/DatabaseServer";
import { IDatabaseTables } from "@spt/models/spt/server/IDatabaseTables";
import { ILogger } from "@spt/models/spt/utils/ILogger";
import Util from "./util";
import { LogTextColor } from "@spt/models/spt/logging/LogTextColor";

class Mod implements IPostDBLoadMod
{
    public async postDBLoad(container: DependencyContainer): Promise<void>
    {
        const logger = container.resolve<ILogger>("WinstonLogger")
        const dbServer = container.resolve<DatabaseServer>("DatabaseServer")
        const tables: IDatabaseTables = dbServer.getTables()
        const templates = tables.templates

        const weaponData = Util.parseJson(Util.getDataFile("weapon_data.json"))
        const strings = Util.parseJson(Util.getDataFile("strings.json"))
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

        logger.logWithColor(`[SPTRecoilRework] Successfully modified data for ${weaponCount} weapons. ${randomString}`, LogTextColor.YELLOW)
    }
}

export const mod = new Mod();
