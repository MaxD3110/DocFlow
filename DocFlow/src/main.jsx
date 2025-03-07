import React from "react";
import ReactDOM from "react-dom/client";
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import FileUploader from './FileUploader.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <FileUploader />
  </StrictMode>,
)
