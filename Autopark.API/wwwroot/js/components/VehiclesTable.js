import { createTableFromGetRequestAsync } from "../utils/GenerateTable.js";

class VehiclesTableComponent extends HTMLElement {
    async connectedCallback() {
        this.appendChild(await createTableFromGetRequestAsync("VehiclesTable", "Vehicle"))
    }
}

export const registerVehiclesTableComponent = () => {
    customElements.define('x-vehicles-table', VehiclesTableComponent)
}