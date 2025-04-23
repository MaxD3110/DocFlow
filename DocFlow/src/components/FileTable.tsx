import { useState, useEffect } from "react";
import axios from "axios";
import { FileData } from "../types/File";
import { CloudArrowUpIcon, XCircleIcon } from '@heroicons/react/20/solid';
import PopupUpload from "./popups/PopupUpload";
import { ArrowPathRoundedSquareIcon } from "@heroicons/react/20/solid";
import { useServiceStatuses } from "./ServiceStatusProvider";
import Popup from "./popups/PopupConvert";
import FileList from "./FileList";
import MassOperationsPanel from "./MassOperationsPanel";

const FileTable = ({ refresh }: { refresh: boolean }) => {
    const [files, setFiles] = useState<FileData[]>([]);
    const [selectedExtensions, setSelectedExtensions] = useState<Record<number, number>>({});
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>("");
    const [selectedFileIds, setSelectedFileIds] = useState<number[]>([]);
    const [isPopupUploadOpen, setIsPopupUploadOpen] = useState(false);
    const [isPopupConvertOpen, setIsPopupConvertOpen] = useState(false);
    const [globalExtensionId, setGlobalExtensionId] = useState<number | null>(null);
    const statuses = useServiceStatuses();

    useEffect(() => { fetchFiles() }, [refresh]);

    const fetchFiles = async () => {
        setLoading(true);

        try {
            const response = await axios.get<FileData[]>("/api/files");
            setFiles(response.data);
        } catch (error) {
            setError("Failed to load files");
        }

        setLoading(false);
        setSelectedFileIds([]);
        setIsPopupUploadOpen(false)
    };

    const convertFile = async () => {

        const fileId = selectedFileIds[0];
        const extensionId = uniqueExtensions[0].convertibleTo[0].id;

        try {
            await axios.post(`/api/files/ConvertFile/${fileId}-${extensionId}`);
        } catch (error) {
            setError("Failed to convert file");
        }

        fetchFiles();
    }

    const uniqueExtensions = Array.from(
        new Map(
            files
                .filter(file => selectedFileIds.includes(file.id))
                .map(file => [file.extension.id, file.extension])
        ).values()
    );

    return (
        <div className="mx-auto mt-48">
            <PopupUpload onUploadSuccess={() => fetchFiles()} isOpen={isPopupUploadOpen} setIsOpen={setIsPopupUploadOpen} />
            <Popup isOpen={isPopupConvertOpen} setIsOpen={setIsPopupConvertOpen} selectedExtensions={uniqueExtensions} />
            {(!error && !loading) && (
                <div className="overflow-visible">
                    {files.length > 0 ? (
                        <div>
                            <div className="flex justify-between">
                                <button
                                    className="mb-3 p-3 flex gap-2 font-bold items-center rounded-4xl max-w-100 shadow-xs bg-lavender text-white hover:bg-white hover:text-lavender duration-150"
                                    onClick={() => setIsPopupUploadOpen(!isPopupUploadOpen)}>
                                    <CloudArrowUpIcon aria-hidden="true" className="h-6 w-6" /> Upload file
                                </button>
                                <button
                                    className="mb-3 p-3 flex gap-2 items-center text-base font-bold max-w-100 shadow-xs rounded-4xl bg-verdigris text-white
                                    hover:bg-white hover:text-verdigris duration-150
                                    disabled:bg-gray-300 disabled:hover:text-white disabled:cursor-not-allowed"
                                    onClick={() => convertFile()}
                                    disabled={!statuses.processor}>
                                    <ArrowPathRoundedSquareIcon aria-hidden="true" className="h-6 w-6" />
                                    <span>Convert files</span>
                                </button>
                            </div>
                            <div className="w-full rounded-2xl overflow-visible bg-white shadow-xs">
                                <MassOperationsPanel
                                    files={files}
                                    selectedFileIds={selectedFileIds}
                                    setSelectedFileIds={setSelectedFileIds}
                                    refreshTable={fetchFiles}
                                    selectedExtensions={selectedExtensions}
                                    setSelectedExtensions={setSelectedExtensions}
                                    globalExtensionId={globalExtensionId}
                                    setGlobalExtensionId={setGlobalExtensionId} />
                                <FileList
                                    files={files}
                                    fetchFiles={() => fetchFiles()}
                                    selectedFileIds={selectedFileIds}
                                    setSelectedFileIds={setSelectedFileIds}
                                    selectedExtensions={selectedExtensions}
                                    setSelectedExtensions={setSelectedExtensions}
                                    setGlobalExtensionId={setGlobalExtensionId} />
                            </div>
                        </div>
                    ) : (
                        <div className="text-center font-extrabold py-4 text-gray-700 flex flex-col justify-center items-center">
                            <span className="mb-20 text-2xl">No files uploaded yet</span>
                            <button
                                className="p-6 flex flex-col items-center rounded-4xl max-w-100 bg-lavender text-white hover:bg-white hover:text-lavender duration-150"
                                onClick={() => setIsPopupUploadOpen(!isPopupUploadOpen)}>
                                <CloudArrowUpIcon aria-hidden="true" className="h-10 w-10" /> Upload your first file
                            </button>
                        </div>
                    )}
                </div>
            )}

            <div className="flex justify-center text-center font-extrabold py-4 text-3xl">

                {loading && <p className="text-gray-700 text-center">Loading files...</p>}

                {error &&
                    <div className="flex justify-center items-center flex-col">
                        <XCircleIcon aria-hidden="true" className="h-12 w-12 text-red-500" />
                        <p className="text-red-500 text-center">{error}</p>
                    </div>
                }
            </div>
        </div>
    );
};

export default FileTable;