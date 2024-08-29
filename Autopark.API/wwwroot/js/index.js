import { registerVehiclesTableComponent } from './components/VehiclesTable.js';
import { registerBrandsTableComponent } from './components/BrandsTable.js';
const app = () => {
    registerVehiclesTableComponent();
    registerBrandsTableComponent();
}
document.addEventListener('DOMContentLoaded', app);