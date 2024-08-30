import { registerVehiclesTableComponent } from './components/Vehicle/VehiclesTable.js';
import { registerBrandsTableComponent } from './components/Brand/BrandsTable.js';
const app = () => {
    registerVehiclesTableComponent();
    registerBrandsTableComponent();
}
document.addEventListener('DOMContentLoaded', app);