import { createTableFromGetRequestAsync } from "../../utils/GenerateTable.js";
import { mapTableColumnAsync } from "../../utils/TableUtils.js";
import { vehicleTypeIntToStr } from "../../utils/Converters.js";

class BrandsTableComponent extends HTMLElement {
    async connectedCallback() {
        var table = await createTableFromGetRequestAsync("BrandsTable", "Brand")
        this.appendChild(table)
        mapTableColumnAsync(table.id, "vehicleType", vehicleTypeIntToStr)
    }
}

export const registerBrandsTableComponent = () => {
    customElements.define('x-brands-table', BrandsTableComponent)
}