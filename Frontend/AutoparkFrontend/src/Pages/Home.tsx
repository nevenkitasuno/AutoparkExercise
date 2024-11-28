import BrandsTable from "../Components/BrandsTable";
import ReportForm from "../Components/ReportForm";
import VehicleManagement from "../Components/VehiclesManagement";

function Home() {
    return (
        <div>
            <BrandsTable />
            <VehicleManagement />
            <ReportForm />
        </div>
    );
}

export default Home;