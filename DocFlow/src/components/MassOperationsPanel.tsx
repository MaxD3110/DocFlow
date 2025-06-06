import { useState, useEffect } from "react";
import { FileData } from "../types/File";
import axios from "axios";
import { CloudArrowDownIcon, TrashIcon } from '@heroicons/react/20/solid';
import Checkbox from "./ui-elements/Checkbox";
import Dropdown from "./ui-elements/Dropdown";
import { useNotify } from "./notifications/NotificationContext";

interface MassOperationsPanelProps {
    files: FileData[],
    selectedFileIds: number[],
    setSelectedFileIds: React.Dispatch<React.SetStateAction<number[]>>,
    refreshTable: () => Promise<void>,
    selectedExtensions: Record<number, number>,
    setSelectedExtensions: React.Dispatch<React.SetStateAction<Record<number, number>>>
    globalExtensionId: number | null,
    setGlobalExtensionId: React.Dispatch<React.SetStateAction<number | null>>
}

const MassOperationsPanel = ({
    files,
    selectedFileIds,
    setSelectedFileIds,
    refreshTable,
    selectedExtensions,
    setSelectedExtensions,
    globalExtensionId,
    setGlobalExtensionId
}: MassOperationsPanelProps) => {
    const notify = useNotify();

    const selectedFiles = files.filter(f => selectedFileIds.includes(f.id));

    useEffect(() => {
        console.log(selectedExtensions);
        
    })

    const convertible = selectedFiles.reduce(
        (common, file) => common.filter(ext => file.extension.convertibleTo.some(e => e.id === ext.id)),
        selectedFiles[0]?.extension.convertibleTo || []
    );

    const deleteSelectedFiles = async () => {
        try {
            await axios.post("api/files/bulkDelete", selectedFileIds, { headers: { "Content-Type": "application/json" } });
        } catch (error) {
            notify("Failed to delete files", "error");
        }
        await refreshTable();
    }

    const toggleAllSelection = (isChecked: boolean) => {
        setSelectedFileIds(isChecked ? files.map(file => file.id) : []);
    };

    const applyGlobalConversion = (extensionId: number) => {

        setGlobalExtensionId(extensionId);

        setSelectedExtensions(prev => {
            const updated = { ...prev };
            selectedFileIds.forEach(id => {
                updated[id] = extensionId!;
            });
            return updated;
        });
    };

    const isAllSelected = files.length > 0 && selectedFileIds.length === files.length;
    const selectionMode = selectedFileIds.length > 0;

    return (
        <div className="bg-gray-700 text-gray-100 text-sm flex justify-between items-center content-center rounded-t-2xl">
            <div className="py-6 pl-4 text-center flex items-center content-center">
                <Checkbox
                    checked={isAllSelected}
                    onChange={(checked) => toggleAllSelection(checked)}
                />
                <div className="pl-3 font-light text-lg duration-150">
                    {selectionMode ? (
                        <div><b className="font-bold">{selectedFileIds.length}</b> files selected</div>
                    ) : (<div>Click to select all</div>)}
                </div>
            </div>
            <div className="flex pr-4 gap-3 duration-150" style={selectionMode ? { opacity: '100%' } : { opacity: '0%' }}>
                <div className="pl-3 font-light text-lg flex items-center duration-150">Convert selected to</div>
                <Dropdown
                    convertibleTo={convertible}
                    selectedExtensionId={globalExtensionId}
                    onSelect={(extensionId) => applyGlobalConversion(extensionId)}
                />
                <button
                    className="p-3 rounded-full bg-lavender shadow-xs text-white hover:bg-white hover:text-lavender duration-150"
                    onClick={() => deleteSelectedFiles()}>
                    <CloudArrowDownIcon aria-hidden="true" className="h-6 w-6" />
                </button>
                <button
                    className="p-3 rounded-full bg-red-400 shadow-xs text-white hover:bg-white hover:text-red-400 duration-150"
                    onClick={() => deleteSelectedFiles()}>
                    <TrashIcon aria-hidden="true" className="h-6 w-6" />
                </button>
            </div>
        </div>
    );
}

export default MassOperationsPanel;