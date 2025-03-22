import { useState } from "react";
import { FileData } from "../types/File";
import Popup from "./Popup";
import { useServiceStatuses } from "./ServiceStatusProvider";
import axios from "axios";
import Dropdown from "./Dropdown";
import { CheckIcon } from '@heroicons/react/20/solid';

interface MassOperationsPanelProps {
    files: FileData[],
    selectedFileIds: number[],
    setError: React.Dispatch<React.SetStateAction<string>>,
    refreshTable: () => Promise<void>
}

const MassOperationsPanel = ({ files, selectedFileIds, setError, refreshTable }: MassOperationsPanelProps) => {
    const [isPopupOpen, setIsPopupOpen] = useState(false);
    const statuses = useServiceStatuses();

    const uniqueExtensions = Array.from(
        new Map(
            files
                .filter(file => selectedFileIds.includes(file.id))
                .map(file => [file.extension.id, file.extension])
        ).values()
    );

    const convertibleTo = Array.from(
        new Map(
            uniqueExtensions.map(ext => [ext.id, ext])
        ).values()
    );

    const deleteSelectedFiles = async () => {
        /*         try {
                    await axios.post("/api/files/bulkDelete", selectedFileIds, { headers: { "Content-Type": "application/json" } });
                } catch (error) {
                    setError("Failed to delete files.");
                } */
        await refreshTable();
    }

    return (
        <div className="transition duration-250">
            <Popup isOpen={isPopupOpen} setIsOpen={setIsPopupOpen} selectedExtensions={uniqueExtensions} />

            <div
                className="p-5 mb-2 border-3 flex justify-between rounded-3xl border-blue-200 bg-blue-50 items-center"
                style={selectedFileIds.length === 0 ? { opacity: '0%' } : { opacity: '100%' }}>

                <div className="flex gap-1.5 items-center">
                    <CheckIcon aria-hidden="true" className="h-6 w-6" /> <b>{selectedFileIds.length}</b> Files selected
                </div>

                <div className="flex justify-end gap-1">
                    <div className="flex items-center gap-3">
                        Convert to
                        <Dropdown convertibleTo={convertibleTo ?? []} />
                    </div>

                    <button className="button-standart bg-blue-600 text-white rounded-3xl hover:bg-blue-900">
                        Download
                    </button>

                    <button
                        className="button-standart bg-red-600 text-white hover:bg-red-900"
                        onClick={() => deleteSelectedFiles()}>
                        Delete
                    </button>
                </div>
            </div>
        </div>
    );
}

export default MassOperationsPanel;