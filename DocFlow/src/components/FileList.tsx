import { useState, useEffect } from "react";
import axios from "axios";
import { FileData } from "../types/File";
import { FolderArrowDownIcon, XMarkIcon } from '@heroicons/react/20/solid'

const FileList = ({ refresh }: { refresh: boolean }) => {
    const [files, setFiles] = useState<FileData[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>("");

    const fetchFiles = async () => {
        setLoading(true);
        try {
            const response = await axios.get<FileData[]>("/api/files");
            setFiles(response.data);
        } catch (error) {
            setError("Failed to load files.");
        }
        setLoading(false);
    };

    const deleteFile = async (id: number) => {
        try {
            await axios.delete<number>(`/api/files/${id}`);
        } catch (error) {
            setError("Failed to delete file.");
        }

        fetchFiles();
    }

    const convertFile = async (id: number) => {
        try {
            await axios.delete<number>(`/api/files/${id}`);
        } catch (error) {
            setError("Failed to delete file.");
        }
    }

    useEffect(() => {
        fetchFiles();
    }, [refresh]); // Reload when refresh changes

    return (
        <div className="mx-auto mt-8">
            <h2 className="text-2xl font-semibold center text-gray-800 mb-4">📂 Uploaded Files</h2>

            {/* Loading State */}
            {loading && <p className="text-blue-500 text-center">Loading files...</p>}

            {/* Error Message */}
            {error && <p className="text-red-500 text-center">{error}</p>}

            {/* File Table */}
            <div className="overflow-x-auto">
                <table className="w-full rounded-lg overflow-hidden">
                    <thead className="bg-blue-100 text-gray-700 uppercase text-sm">
                        <tr>
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
                                    <td className="py-2 px-4 text-center">{file.name}</td>
                                    <td className="py-2 px-4 text-center">{file.extensionName}</td>
                                    <td className="py-2 px-4 text-center">
                                        {(file.fileSize / 1024).toFixed(2)}
                                    </td>
                                    <td className="py-2 px-4 text-center">{new Date(file.uploadedAt).toLocaleString()}</td>
                                    <td className="py-2 px-4 text-center">
                                        <button onClick={() => convertFile(file.id)}>Convert</button>
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