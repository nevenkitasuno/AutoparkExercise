import BrandsTable from "../Components/BrandsTable";
import Reports from "../Components/Reports";
import VehicleManagement from "../Components/VehiclesManagement";

function Home() {
    return (
        <div>
            <BrandsTable />
            <VehicleManagement />
            <Reports />
        </div>
    );
}

export default Home;