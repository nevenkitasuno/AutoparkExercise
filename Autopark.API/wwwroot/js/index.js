import { registerVehiclesTableComponent } from './components/Vehicle/VehiclesTable.js';
import { registerBrandsTableComponent } from './components/Brand/BrandsTable.js';
import { registerVehicleUpsertFormComponent } from './components/Vehicle/VehicleUpsertForm.js';

const app = () => {
    registerVehiclesTableComponent();
    registerBrandsTableComponent();
    registerVehicleUpsertFormComponent();
}

document.addEventListener('DOMContentLoaded', app);