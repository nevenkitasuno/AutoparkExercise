import { createTableFromGetRequestAsync } from "../../utils/GenerateTable.js";

class BrandsTableComponent extends HTMLElement {
    async connectedCallback() {
        var table = await createTableFromGetRequestAsync("DriversTable", "Driver")
        this.appendChild(table)
    }
}

export const registerDriversTableComponent = () => {
    customElements.define('x-drivres-table', BrandsTableComponent)
}