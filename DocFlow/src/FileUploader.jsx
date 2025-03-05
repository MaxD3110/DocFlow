import { useState } from "react";
import axios from "axios";

function FileUploader() {
  const [file, setFile] = useState(null);
  const [message, setMessage] = useState("");

  const handleFileChange = (event) => {
      if (event.target.files.length > 0) {
          setFile(event.target.files[0]);
      }
  };

  const handleUpload = async () => {
      if (!file) return;

      const formData = new FormData();
      formData.append("file", file);

      try {
          const response = await axios.post("/api/files/upload", formData);
          setMessage(`File uploaded: ${response.data}`);
      } catch (error) {
          setMessage("Upload failed");
      }
  };

  const handleGet = async () => {
    await axios.get("/api/files");
  }

  return (
      <div>
          <input type="file" onChange={handleFileChange} />
          <button onClick={handleUpload}>Upload</button>
          <p>{message}</p>

          
          <button onClick={handleGet}>Get</button>
      </div>
  );
};

export default FileUploader;