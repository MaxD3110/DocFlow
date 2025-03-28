import { useState } from "react";
import FileList from "./components/FileList";
import Header from "./components/Header";
import Footer from "./components/Footer";
import { ServiceStatusProvider } from "./components/ServiceStatusProvider";

const App = () => {
  const [refresh, setRefresh] = useState(false);

  return (
    <ServiceStatusProvider>
      <Header />
      <div className="pt-20 px-10 flex-1">

        <FileList refresh={refresh} />
      </div>
      <Footer />
    </ServiceStatusProvider>
  );
};

export default App;
