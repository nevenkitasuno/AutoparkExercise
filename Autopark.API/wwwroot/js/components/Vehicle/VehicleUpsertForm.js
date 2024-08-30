import { createUpsertFormAsync } from "../../utils/UpsertForm.js";

class VehicleUpsertFormComponent extends HTMLElement {
    async connectedCallback() {
        var form = await createUpsertFormAsync("VehiclesUpsertForm", "Vehicle")
        this.appendChild(form)
    }
}

export const registerVehicleUpsertFormComponent = () => {
    customElements.define('x-vehicle-upsert-form', VehicleUpsertFormComponent)
}