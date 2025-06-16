import { readFileSync } from "fs"
import path from "path"

class Util {
    public static parseJson = (path) => {
        const dataJson = readFileSync(path, "utf-8")
        const parsed = JSON.parse(dataJson)
        return parsed
    }

    public static getDataFile = (fileName) => {
        return path.join(path.join(__dirname, `/../data/${fileName}`))
    }
}

export default Util