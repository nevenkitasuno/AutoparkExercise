import { getAllEntitiesAsync } from "./FetchGetUtils.js"

const NULL_VALUE = "";

export function vehicleTypeIntToStr(vehicleTypeInt) {
    if (typeof vehicleTypeInt === "string") vehicleTypeInt = parseInt(vehicleTypeInt)
    switch (vehicleTypeInt) {
        case 0:
            return "Легковой автомобиль";
        case 1:
            return "Грузовой автомобиль";
        case 2:
            return "Автобус";
        default:
            return NULL_VALUE;
    }
}

export async function idToFieldAsync(controllerName, id, field) {
    let entities = await getAllEntitiesAsync(controllerName)
    let entity = entities.find(e => e.id == id)
    return entity? entity[field] : NULL_VALUE
}

export async function enterpriseIdToNameAsync(enterpriseId)
{
    return await idToFieldAsync("Enterprise", enterpriseId, "name")
}

export async function driverIdToFullNameAsync(driverId)
{
    let drivres = await getAllEntitiesAsync("Driver")
    let driver = drivres.find(d => d.id == driverId)
    return driver ?  driver.surname + " " + driver.firstName + " " + driver.patronymic : NULL_VALUE   
}

export async function vehicleIdToLicensePlateAsync(vehicleId)
{
    let vehicles = await getAllEntitiesAsync("Vehicle")
    let vehicle = vehicles.find(v => v.id == vehicleId)
    return vehicle? vehicle.licensePlate : NULL_VALUE
}

export async function brandIdToBrandNameAsync(brandId) {
    let brands = await getAllEntitiesAsync("Brand")
    let brand = brands.find(b => b.id == brandId)
    return brand ? brand.manufacturerCompany + " " + brand.modelName : NULL_VALUE
}