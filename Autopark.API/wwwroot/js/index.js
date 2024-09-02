import { registerVehiclesTableComponent } from './components/Vehicle/VehiclesTable.js';
import { registerVehicleUpsertFormComponent } from './components/Vehicle/VehicleUpsertForm.js';

import { registerBrandsTableComponent } from './components/Brand/BrandsTable.js';
import { registerDriversTableComponent } from './components/Driver/DriversTable.js';
import { registerEnterprisesTableComponent } from './components/Enterprise/EnterprisesTable.js';

const app = () => {
    registerVehiclesTableComponent();
    registerVehicleUpsertFormComponent();

    registerBrandsTableComponent();
    
    registerDriversTableComponent();
    registerEnterprisesTableComponent();
}

document.addEventListener('DOMContentLoaded', app);