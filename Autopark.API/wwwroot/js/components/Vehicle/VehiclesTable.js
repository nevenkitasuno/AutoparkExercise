import { createTableFromGetRequestAsync } from "../../utils/GenerateTable.js";
import { mapTableColumnAsync } from "../../utils/TableUtils.js";
import { brandIdToBrandNameAsync } from "../../utils/Converters.js";

class VehiclesTableComponent extends HTMLElement {
    async connectedCallback() {
        var table = await createTableFromGetRequestAsync("VehiclesTable", "Vehicle")
        this.appendChild(table)
        mapTableColumnAsync(table.id, "brandId", brandIdToBrandNameAsync)
    }
}

export const registerVehiclesTableComponent = () => {
    customElements.define('x-vehicles-table', VehiclesTableComponent)
}