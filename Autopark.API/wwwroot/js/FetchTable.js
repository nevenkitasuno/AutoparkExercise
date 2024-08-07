// TODO: Split to separate files

const apiPath = "http://127.0.0.1:5237/api";

function getAndDisplayData(controller, displayFunction) {
    fetch(apiPath + controller)
        .then(response => response.json())
        .then(data => displayFunction(data))
        .catch(error => console.error('Error:', error));
}

function displayVehiclesWithManufacturerAndModelName(vehiclesWithManufacturerAndModelNames) {
    var temp = "";

    vehiclesWithManufacturerAndModelNames.forEach((x) => {
        temp += makeRowForVehicleWithManufacturerAndModelName(x);
    });

    document.getElementById("VehiclesTable").innerHTML += temp;
}

function makeRowForVehicleWithManufacturerAndModelName(vehicleWithManufacturerAndModelName) {
    vId = vehicleWithManufacturerAndModelName.id;
    row = ""
    row += '<tr id="vehicle-row-' + vId + '">';
    row += "<td>" + makeDeleteVehicleButton(vId) + "</td>";
    row += "<td>" + makeUpdateVehicleButton(vId) + "</td>";
    row += "<td>" + vId + "</td>";
    row += "<td>" + vehicleWithManufacturerAndModelName.price + "</td>";
    row += "<td>" + vehicleWithManufacturerAndModelName.manufactureYear + "</td>";
    row += "<td>" + vehicleWithManufacturerAndModelName.mileage + "</td>";
    row += "<td>" + vehicleWithManufacturerAndModelName.licensePlate + "</td>";
    row += "<td>" + vehicleWithManufacturerAndModelName.manufacturerCompany + " " + vehicleWithManufacturerAndModelName.modelName + "</td>";
    row += "</tr>"
    return row
}

function editVehicleRow(vehicleWithManufacturerAndModelName)
{
    document.getElementById('vehicle-row-'+vehicleWithManufacturerAndModelName.id)
        .replaceWith(makeRowForEditVehicle(vehicleWithManufacturerAndModelName))
}

function updateVehicleAndCloseMenu()
{
    const vehicleId = document.getElementById("UpdateVehicleFormId").innerHTML;
    const price = document.getElementById('UpdateVehicleFormPrice').value;
    const manufactureYear = document.getElementById('UpdateVehicleFormManufactureYear').value;
    const mileage = document.getElementById('UpdateVehicleFormMileage').value;
    const licensePlate = document.getElementById('UpdateVehicleFormLicensePlate').value;
    const brandId = document.getElementById('UpdateVehicleFormBrand').value;

    fetch(apiPath + '/Vehicle/' + vehicleId, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            price: price,
            manufactureYear: manufactureYear,
            mileage: mileage,
            licensePlate: licensePlate,
            brandId: brandId
        })
    }).catch(error => console.error('Error:', error));

    // Close menu
    document.getElementById("EditVehicleDiv").style.display = 'none';
    document.getElementById("UpdateVehicleFormId").innerHTML = "";

    location.reload(); // TODO: get rid somehow
    return false;
}

function makeUpdateVehicleButton(id) {
    const updateFunctionName = 'openUpdateVehicleMenu'
    return `<button class="updateBtn" onclick="${updateFunctionName}('${id}')">✏️</button>`;
}

function openUpdateVehicleMenu(vehicleId) {
    document.getElementById("EditVehicleDiv").style.display = 'block'; // TODO: rename edit to update, use crud terms
    document.getElementById("UpdateVehicleFormId").innerHTML = vehicleId;
}

document.getElementById('SubmitVehicleBtn').addEventListener('click', async function (event) {
    // event.preventDefault();

    const formData = {
        Price: document.getElementById('SubmitVehicleFormPrice').value,
        ManufactureYear: document.getElementById('SubmitVehicleFormManufactureYear').value,
        Mileage: document.getElementById('SubmitVehicleFormMileage').value,
        LicensePlate: document.getElementById('SubmitVehicleFormLicensePlate').value,
        BrandId: document.getElementById('SubmitVehicleFormBrand').value
    };

    try {
        const response = await fetch(apiPath + '/Vehicle', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(formData)
        });

        if (response.ok) {
            console.log('Form submitted successfully');
            console.log(response.body) // TODO: Add row instead of page reload
        } else {
            console.error('Form submission failed');
        }
    } catch (error) {
        console.error('An error occurred during form submission:', error);
    }

    location.reload(); // TODO: Add row instead of page reload
});

function makeDeleteVehicleButton(id) {
    const deleteFunctionName = 'deleteVehicle'
    return `<button class="deleteBtn" onclick="${deleteFunctionName}('${id}')">❌</button>`;
}

function deleteVehicle(id) {
    fetch(apiPath + "/Vehicle/" + id, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .catch(error => console.error('Error:', error));
    rowId = "vehicle-row-" + id
    const row = document.getElementById(rowId);
    row.remove();
}

function displayBrands(brands) {
    var temp = "";

    brands.forEach((x) => {
        temp += "<tr>";
        temp += "<td>" + x.id + "</td>";
        temp += "<td>" + x.manufacturerCompany + "</td>";
        temp += "<td>" + x.modelName + "</td>";
        temp += "<td>" + x.engineDisplacementLiters + "</td>";
        temp += "<td>" + vehicleTypeIntToStr(x.vehicleType) + "</td>";
        temp += "<td>" + x.fuelTankCapacityLiters + "</td>";
        temp += "<td>" + x.seatsCount + "</td>";
        temp += "<td>" + x.liftWeightCapacityKg + "</td>";
        temp += "</tr>"
    });

    document.getElementById("BrandsTable").innerHTML += temp;
}

function vehicleTypeIntToStr(vehicleTypeInt) {
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