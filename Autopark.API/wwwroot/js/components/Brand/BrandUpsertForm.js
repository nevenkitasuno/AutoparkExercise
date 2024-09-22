import { createUpsertFormAsync } from "../../utils/UpsertForm.js";

class BrandUpsertFormComponent extends HTMLElement {
    async connectedCallback() {
        var form = await createUpsertFormAsync("BrandsUpsertForm", "Brand")
        this.appendChild(form)
    }
}

export const registerBrandUpsertFormComponent = () => {
    customElements.define('x-brand-upsert-form', BrandUpsertFormComponent)
}