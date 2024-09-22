import { apiPath } from "../options.js";

export async function getAllEntitiesAsync(controllerName) {
    let entities
    const resp = await fetch(apiPath + "/" + controllerName)
    entities = await resp.json()
    return entities
}