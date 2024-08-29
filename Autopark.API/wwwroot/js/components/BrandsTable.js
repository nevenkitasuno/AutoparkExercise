import { createTableFromGetRequestAsync } from "../utils/GenerateTable.js";

class BrandsTableComponent extends HTMLElement {
    async connectedCallback() {
        this.appendChild(await createTableFromGetRequestAsync("BrandsTable", "Brand"))
    }
}

export const registerBrandsTableComponent = () => {
    customElements.define('x-brands-table', BrandsTableComponent)
}