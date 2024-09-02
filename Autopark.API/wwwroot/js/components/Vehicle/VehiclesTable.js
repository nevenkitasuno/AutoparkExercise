import { createTableFromGetRequestAsync } from "../../utils/GenerateTable.js";
import { mapTableColumnAndChangeTitleAsync } from "../../utils/TableUtils.js";
import { brandIdToBrandNameAsync, enterpriseIdToNameAsync, driverIdToFullNameAsync } from "../../utils/Converters.js";

class VehiclesTableComponent extends HTMLElement {
    async connectedCallback() {
        var table = await createTableFromGetRequestAsync("VehiclesTable", "Vehicle")
        this.appendChild(table)

        mapTableColumnAndChangeTitleAsync(table.id, "brandId", "Brand", brandIdToBrandNameAsync)
        mapTableColumnAndChangeTitleAsync(table.id, "enterpriseId", "Enterprise", enterpriseIdToNameAsync)
        mapTableColumnAndChangeTitleAsync(table.id, "currentDrvierId", "Current driver", driverIdToFullNameAsync)
    }
}

export const registerVehiclesTableComponent = () => {
    customElements.define('x-vehicles-table', VehiclesTableComponent)
}