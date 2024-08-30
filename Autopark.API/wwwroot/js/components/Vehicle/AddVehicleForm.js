class AddVehicleFormComponent extends HTMLElement {
    async connectedCallback() {
        //...
    }
}

export const registerVehiclesTableComponent = () => {
    customElements.define('x-add-vehicle-form', AddVehicleFormComponent)
}