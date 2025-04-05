import { useState } from "react";
import Header from "./components/Header";
import Footer from "./components/Footer";
import { ServiceStatusProvider } from "./components/ServiceStatusProvider";
import FileTable from "./components/FileTable";
import { NotificationProvider } from "./components/notifications/NotificationContext";

const App = () => {
  const [refresh, setRefresh] = useState(false);

  return (
    <ServiceStatusProvider>
      <NotificationProvider>
        <Header />
        <div className="pt-20 px-10 flex-1">

          <FileTable refresh={refresh} />
        </div>
        <Footer />
      </NotificationProvider >
    </ServiceStatusProvider>
  );
};

export default App;
