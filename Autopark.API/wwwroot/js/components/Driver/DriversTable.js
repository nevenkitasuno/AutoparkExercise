import { createTableFromGetRequestAsync } from "../../utils/GenerateTable.js";
import { mapTableColumnAndChangeTitleAsync } from "../../utils/TableUtils.js";
import { enterpriseIdToNameAsync, vehicleIdToLicensePlateAsync } from "../../utils/Converters.js";

class BrandsTableComponent extends HTMLElement {
    async connectedCallback() {
        var table = await createTableFromGetRequestAsync("DriversTable", "Driver")
        this.appendChild(table)

        mapTableColumnAndChangeTitleAsync(table.id, "enterpriseId", "Enterprise", enterpriseIdToNameAsync)
        mapTableColumnAndChangeTitleAsync(table.id, "currentVehicleId", "Current vehicle", vehicleIdToLicensePlateAsync)
    }
}

export const registerDriversTableComponent = () => {
    customElements.define('x-drivres-table', BrandsTableComponent)
}