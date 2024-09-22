import { apiPath } from "../options.js";

async function getEntityByIdAsync(controllerName, id) {
    let entity;
    const resp = await fetch(apiPath + "/" + controllerName + "/" + id)
    entity = await resp.json()

    return entitiy;
}

export async function getAllEntitiesAsync(controllerName) {
    let entities
    const resp = await fetch(apiPath + "/" + controllerName)
    entities = await resp.json()
    return entities
}