import { createTableFromGetRequestAsync } from "../../utils/GenerateTable.js";

class BrandsTableComponent extends HTMLElement {
    async connectedCallback() {
        var table = await createTableFromGetRequestAsync("EnterprisesTable", "Enterprise")
        this.appendChild(table)
    }
}

export const registerEnterprisesTableComponent = () => {
    customElements.define('x-enterprises-table', BrandsTableComponent)
}