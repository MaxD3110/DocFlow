import { useState, useEffect } from "react";
import axios from "axios";
import { FileData } from "../types/File";
import { FolderArrowDownIcon, XMarkIcon } from '@heroicons/react/20/solid';
import Dropdown from "./Dropdown";
import Checkbox from "./Checkbox";
import Popup from "./Popup";

const FileList = ({ refresh }: { refresh: boolean }) => {
    const [files, setFiles] = useState<FileData[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>("");
    const [selectedFiles, setSelectedFiles] = useState<number[]>([]);
    const [isPopupOpen, setIsPopupOpen] = useState(false);

    useEffect(() => {
        fetchFiles();
    }, [refresh]); // Reload when refresh changes

    const fetchFiles = async () => {
        setLoading(true);

        try {
            const response = await axios.get<FileData[]>("/api/files");
            setFiles(response.data);
        } catch (error) {
            setError("Failed to load files.");
        }

        setLoading(false);
        setSelectedFiles([]);
    };

    const deleteFile = async (id: number) => {
        try {
            await axios.delete<number>(`/api/files/${id}`);
        } catch (error) {
            setError("Failed to delete file.");
        }

        fetchFiles();
    }

    const deleteSelectedFiles = async () => {
        try {
            await axios.post("/api/files/bulkDelete", selectedFiles, { headers: { "Content-Type": "application/json" } });
        } catch (error) {
            setError("Failed to delete files.");
        }

        fetchFiles();
    }

    const toggleSelection = (fileId: number, isChecked: boolean) => {
        setSelectedFiles((prevSelected) =>
            isChecked ? [...prevSelected, fileId] : prevSelected.filter((id) => id !== fileId)
        );
    };

    const toggleAllSelection = (isChecked: boolean) => {
        setSelectedFiles(() => isChecked ? files.map(file => file.id) : []);
    };

    const isAllSelected = files.length > 0 && selectedFiles.length === files.length;
    const uniqueExtensions = Array.from(
        new Map(
            files
                .filter(file => selectedFiles.includes(file.id))
                .map(file => [file.extension.id, file.extension])
        ).values()
    );

    return (
        <div className="mx-auto mt-8">
            {files.length > 0 && (
                <Popup isOpen={isPopupOpen} setIsOpen={setIsPopupOpen} selectedExtensions={uniqueExtensions} />
            )}

            <h2 className="text-2xl font-semibold center text-gray-800 mb-4">📂 Uploaded Files</h2>

            <div
                className="py-2 gap-1 justify-end flex transition duration-75"
                style={selectedFiles.length === 0 ? { opacity: '0%' } : { opacity: '100%' }}>

                <button
                    onClick={() => setIsPopupOpen(true)}
                    className="mt-4 px-4 py-2 bg-transparent text-gray-800 border-2 rounded hover:border-blue-600 hover:text-blue-600 duration-150"
                    disabled={selectedFiles.length === 0}
                >
                    Convert all to
                </button>

                <button className="mt-4 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-900 duration-150 cursor-pointer">
                    Download all
                </button>

                <button
                    className="mt-4 px-4 py-2 bg-red-600 text-white rounded hover:bg-red-900 duration-150 cursor-pointer"
                    onClick={() => deleteSelectedFiles()}>
                    Delete all
                </button>
            </div>

            {/* Loading State */}
            {loading && <p className="text-blue-500 text-center">Loading files...</p>}

            {/* Error Message */}
            {error && <p className="text-red-500 text-center">{error}</p>}

            {/* File Table */}
            <div className="overflow-x-auto">
                <table className="w-full rounded-lg overflow-hidden">
                    <thead className="bg-blue-100 text-gray-700 uppercase text-sm">
                        <tr>
                            <th className="py-2 px-4 text-center">
                                <Checkbox
                                    label=""
                                    checked={isAllSelected}
                                    onChange={(checked) => toggleAllSelection(checked)}
                                />
                            </th>
                            <th className="py-2 px-4 text-center">File Name</th>
                            <th className="py-2 px-4 text-center">File Type</th>
                            <th className="py-2 px-4 text-center">Size (KB)</th>
                            <th className="py-2 px-4 text-center">Uploaded at</th>
                            <th className="py-2 px-4 text-center">Convert</th>
                            <th className="py-2 px-4 text-center">Download</th>
                            <th className="py-2 px-4 text-center">Delete</th>
                        </tr>
                    </thead>
                    <tbody>
                        {files.length > 0 ? (
                            files.map((file) => (
                                <tr key={file.id} className="hover:bg-blue-50 transition">
                                    <td className="py-2 px-4 text-center">
                                        <Checkbox
                                            label=""
                                            checked={selectedFiles.includes(file.id)}
                                            onChange={(checked) => toggleSelection(file.id, checked)}
                                        />
                                    </td>
                                    <td className="py-2 px-4 text-center">{file.name}</td>
                                    <td className="py-2 px-4 text-center">{file.extensionName}</td>
                                    <td className="py-2 px-4 text-center">
                                        {(file.fileSize / 1024).toFixed(2)}
                                    </td>
                                    <td className="py-2 px-4 text-center">{new Date(file.uploadedAt).toLocaleString()}</td>
                                    <td className="py-2 px-4 text-center">
                                        <Dropdown id={file.id} convertibleTo={file.extension.convertibleTo} />
                                    </td>
                                    <td className="py-2 px-4 text-center">
                                        <a href={file.storagePath} download>
                                            <FolderArrowDownIcon aria-hidden="true" className="cursor-pointer size-8 text-gray-500 hover:fill-blue-500 transition-colors" />
                                        </a>
                                    </td>
                                    <td className="py-2 px-4 text-center">
                                        <a onClick={() => deleteFile(file.id)}>
                                            <XMarkIcon aria-hidden="true" className="cursor-pointer size-8 text-gray-500 hover:fill-red-500 transition-colors" />
                                        </a>
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr>
                                <td colSpan={3} className="text-center py-4 text-gray-500">
                                    No files uploaded yet.
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div >
    );
};

export default FileList;