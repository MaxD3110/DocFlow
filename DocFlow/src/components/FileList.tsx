import { useState, useEffect } from "react";
import axios from "axios";
import { FileData } from "../types/File";
import { FolderArrowDownIcon, XMarkIcon } from '@heroicons/react/20/solid';
import Dropdown from "./Dropdown";
import Checkbox from "./Checkbox";
import MassOperationsPanel from "./MassOperationsPanel";

const FileList = ({ refresh }: { refresh: boolean }) => {
    const [files, setFiles] = useState<FileData[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>("");
    const [selectedFileIds, setSelectedFileIds] = useState<number[]>([]);

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
        setSelectedFileIds([]);
    };

    const deleteFile = async (id: number) => {
        try {
            await axios.delete<number>(`/api/files/${id}`);
        } catch (error) {
            setError("Failed to delete file.");
        }

        fetchFiles();
    }

    const toggleSelection = (fileId: number, isChecked: boolean) => {
        setSelectedFileIds((prevSelected) =>
            isChecked ? [...prevSelected, fileId] : prevSelected.filter((id) => id !== fileId)
        );
    };

    const toggleAllSelection = (isChecked: boolean) => {
        setSelectedFileIds(() => isChecked ? files.map(file => file.id) : []);
    };

    const isAllSelected = files.length > 0 && selectedFileIds.length === files.length;

    return (
        <div className="mx-auto mt-8">
            {selectedFileIds.length > 0 ? (
                <MassOperationsPanel files={files} selectedFileIds={selectedFileIds} setError={setError} refreshTable={fetchFiles} />
            ) : (
                <div className="mt-32"></div>
            )}

            {/* Loading State */}
            {loading && <p className="text-blue-500 text-center">Loading files...</p>}

            {/* Error Message */}
            {error && <p className="text-red-500 text-center">{error}</p>}

            {/* File Table */}
            <div className="overflow-x-auto">
                {files.length > 0 ? (
                    <div className="w-full rounded-lg overflow-hidden bg-white">
                        <div className="bg-gray-700 text-gray-100 uppercase text-sm flex justify-between">
                            <div className="py-2 px-4 text-center">
                                <Checkbox
                                    checked={isAllSelected}
                                    onChange={(checked) => toggleAllSelection(checked)}
                                />
                            </div>
                            <div>

                            </div>
                        </div>
                        <div>
                            {files.map((file) => (
                                <div key={file.id} className={`${selectedFileIds.includes(file.id) ? 'bg-blue-50' : 'hover:bg-blue-50'} transition flex gap-3 justify-between`}>
                                    <div className="file-list-row">
                                        <Checkbox
                                            checked={selectedFileIds.includes(file.id)}
                                            onChange={(checked) => toggleSelection(file.id, checked)}
                                        />
                                    </div>
                                    <div className="file-list-row">
                                        <div className="flex flex-col">
                                            <span className="font-bold">{file.name}</span>
                                            <span>{(file.fileSize / 1024).toFixed(2)}</span>
                                        </div>
                                    </div>
                                    <div className="file-list-row">
                                        <span>{new Date(file.uploadedAt).toLocaleString()}</span>
                                    </div>
                                    <div className="file-list-row">
                                        <Dropdown convertibleTo={file.extension?.convertibleTo ?? []} />
                                    </div>
                                    <div className="file-list-row">
                                        <a href={file.storagePath} download>
                                            <FolderArrowDownIcon aria-hidden="true" className="cursor-pointer size-8 text-gray-500 hover:fill-blue-500 transition-colors" />
                                        </a>
                                    </div>
                                    <div className="file-list-row">
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
                ) : (
                    <div>
                        <div className="text-center text-2xl font-extrabold py-4 text-gray-700">
                            No files uploaded yet <br></br>
                            <b className="text-4xl">...</b>
                        </div>
                    </div>
                )}

            </div>
        </div >
    );
};

export default FileList;