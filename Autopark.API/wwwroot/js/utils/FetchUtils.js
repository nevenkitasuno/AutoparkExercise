import { apiPath } from "./../options.js";

async function getAllIDsAsync(controllerName) {
    let IDs
    const resp = await fetch(apiPath + "/" + controllerName)
    IDs = await resp.json()

    return IDs;
}

async function getEntitiesByIdsAsync(controllerName, ids) {
    var entities = [];
    let entity;

    for (let id of ids) {
        const resp = await fetch(apiPath + "/" + controllerName + "/" + id)
        entity = await resp.json()
        entities.push(entity)
    }

    return entities;
}

export async function getAllEntitiesAsync(controllerName) {
    var IDs = await getAllIDsAsync(controllerName);
    return await getEntitiesByIdsAsync(controllerName, IDs)
}