import { useState } from "react";
import FileList from "./components/FileList";
import FileUpload from "./components/FileUpload";

const App = () => {
  const [refresh, setRefresh] = useState(false);

  return (
    <div className="min-w-full">
      <h1 className="center">DocFlow</h1>

      <FileUpload onUploadSuccess={() => setRefresh(!refresh)} />

      <FileList refresh={refresh} />
    </div>
  );
};

export default App;
