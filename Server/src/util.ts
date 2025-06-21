import { dir } from "console"
import fs from "fs"
import path, { parse } from "path"

class Util {
    public static parseJson = (filePath) => {
        const dataJson = fs.readFileSync(filePath, "utf-8")
        const parsed = JSON.parse(dataJson)
        return parsed
    }

    public static getConfigFile = (fileName) => {
        return path.join(__dirname, `/../config/${fileName}`)
    }

    public static parseDirectory = (dirPath) => {
        const dir = path.join(__dirname, dirPath)
        const jsons = fs.readdirSync(dir).filter(file => path.extname(file) === ".json")
        const data = []

        jsons.forEach(file => {
            const json = fs.readFileSync(path.join(dir, file), "utf-8")
            const parsed = JSON.parse(json)

            if (Array.isArray(parsed)) {
                parsed.forEach(entry => {
                    data.push(entry)
                });
            }
            else {
                data.push(parsed)
            }
        })

        return data
    }
}

export default Util