import axios from "axios";
import { createContext, useContext, useEffect, useState } from "react";

interface ServiceStatus {
    manager: boolean,
    processor: boolean
};

const defaultStatus: ServiceStatus = {
    manager: false,
    processor: false
};

const ServiceStatusContext = createContext<ServiceStatus>(defaultStatus);

export const useServiceStatuses = () => useContext(ServiceStatusContext);

export const ServiceStatusProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [status, setStatus] = useState<ServiceStatus>(defaultStatus);

    const fetchStatus = async () => {
        let responseManager = false; 
        let responseProcessor = false;

        try {
            responseManager = (await axios.get("/api/status")).data;
        } catch (error) {
            console.error("Failed to fetch file manager service status", error);
        }

        try {
            responseProcessor = (await axios.get("/api/processor/status")).data;
        } catch (error) {
            console.error("Failed to fetch file processor service status", error);
        }

        setStatus({ manager: responseManager, processor: responseProcessor });
    };

    useEffect(() => {
        fetchStatus();
        const interval = setInterval(fetchStatus, 15000);
        return () => clearInterval(interval);
    }, []);

    return (
        <ServiceStatusContext.Provider value={status}>
            {children}
        </ServiceStatusContext.Provider>
    );
}