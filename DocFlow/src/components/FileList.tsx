import { useState, useEffect } from "react";
import axios from "axios";
import { FileData } from "../types/File";
import { CloudArrowDownIcon, CloudArrowUpIcon, XCircleIcon, XMarkIcon } from '@heroicons/react/20/solid';
import Dropdown from "./Dropdown";
import Checkbox from "./Checkbox";
import MassOperationsPanel from "./MassOperationsPanel";
import PopupUpload from "./PopupUpload";
import { ArrowPathRoundedSquareIcon } from "@heroicons/react/20/solid";
import { useServiceStatuses } from "./ServiceStatusProvider";
import Popup from "./PopupConvert";

const FileList = ({ refresh }: { refresh: boolean }) => {
    const [files, setFiles] = useState<FileData[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>("");
    const [selectedFileIds, setSelectedFileIds] = useState<number[]>([]);
    const [isPopupUploadOpen, setIsPopupUploadOpen] = useState(false);
    const [isPopupConvertOpen, setIsPopupConvertOpen] = useState(false);
    const statuses = useServiceStatuses();

    useEffect(() => {
        fetchFiles();
    }, [refresh]); // Reload when refresh changes

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

    const deleteFile = async (id: number) => {
        try {
            await axios.delete<number>(`/api/files/${id}`);
        } catch (error) {
            setError("Failed to delete file");
        }

        fetchFiles();
    }

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

    const toggleSelection = (fileId: number, isChecked: boolean) => {
        setSelectedFileIds((prevSelected) =>
            isChecked ? [...prevSelected, fileId] : prevSelected.filter((id) => id !== fileId)
        );
    };

    function fileSize(bytes: number) {
        if (bytes == 0) { return "0.00 B"; }
        var e = Math.floor(Math.log(bytes) / Math.log(1024));
        return (bytes / Math.pow(1024, e)).toFixed(2) + ' ' + ' KMGTP'.charAt(e) + 'B';
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
            {/* File Table */}
            {(!error && !loading) && (
                <div className="overflow-visible">
                    {files.length > 0 ? (
                        <div>
                            <div className="flex justify-between">
                                <button
                                    className="mb-3 p-3 flex gap-2 font-bold items-center rounded-4xl max-w-100 bg-lavender text-white hover:bg-white hover:text-lavender duration-150"
                                    onClick={() => setIsPopupUploadOpen(!isPopupUploadOpen)}>
                                    <CloudArrowUpIcon aria-hidden="true" className="h-6 w-6" /> Upload file
                                </button>
                                <button
                                    className="mb-3 p-3 flex gap-2 items-center text-base font-bold max-w-100 rounded-4xl bg-verdigris text-white hover:bg-white hover:text-verdigris duration-150"
                                    onClick={() => convertFile()}>
                                    <ArrowPathRoundedSquareIcon aria-hidden="true" className="h-6 w-6" />
                                    <span>Convert files</span>
                                </button>
                            </div>
                            <div className="w-full rounded-2xl overflow-visible bg-white">
                                <MassOperationsPanel files={files} selectedFileIds={selectedFileIds} setError={setError} setSelectedFileIds={setSelectedFileIds} refreshTable={fetchFiles} />
                                <div>
                                    {files.map((file) => (
                                        <div
                                            key={file.id}
                                            className={`${selectedFileIds.includes(file.id) ? 'bg-purple-100 border-b-purple-100' : 'hover:bg-purple-100 hover:border-b-purple-100'}
                                            py-2 transition flex gap-3 justify-between border-b-gray-100 last:border-0 last:rounded-b-2xl border-b-2`}>
                                            <div className="file-list-column">
                                                <Checkbox
                                                    checked={selectedFileIds.includes(file.id)}
                                                    onChange={(checked) => toggleSelection(file.id, checked)}
                                                />
                                            </div>
                                            <div className="file-list-column">
                                                <div className="flex flex-col text-gray-700">
                                                    <span className="font-bold">{file.name}</span>
                                                    <span>{fileSize(file.fileSize)}</span>
                                                </div>
                                            </div>
                                            <div className="file-list-column text-gray-700">
                                                <span>{new Date(file.uploadedAt).toLocaleString("en-us", { year: 'numeric', month: 'long', day: 'numeric', hour: 'numeric', hour12: false, minute: 'numeric' })}</span>
                                            </div>
                                            <div className="file-list-column gap-11">
                                                <Dropdown convertibleTo={file.extension?.convertibleTo ?? []} />
                                                <a href={file.storagePath} download>
                                                    <CloudArrowDownIcon aria-hidden="true" className="cursor-pointer size-8 text-gray-500 hover:fill-lavender transition-colors" />
                                                </a>
                                                <a onClick={() => deleteFile(file.id)}>
                                                    <div className="peer h-8 w-8 cursor-pointer transition-all flex justify-center items-center text-red-400 appearance-none rounded-full bg-red-200 shadow hover:shadow-md hover:bg-red-400 hover:text-red-200">
                                                        <XMarkIcon aria-hidden="true" className="h-6 w-6" />
                                                    </div>
                                                </a>
                                            </div>
                                        </div>
                                    ))}
                                </div>
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
                {/* Loading State */}
                {loading && <p className="text-gray-700 text-center">Loading files...</p>}

                {/* Error Message */}
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

export default FileList;