import { getAllEntitiesAsync } from "./FetchUtils.js"

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
            return "Unknown";
    }
}

export async function brandIdToBrandNameAsync(brandId) {
    let brands = await getAllEntitiesAsync("Brand")
    let brand = brands.find(b => b.id == brandId)
    return brand ? brand.manufacturerCompany + " " + brand.modelName : "Unknown brand"
}