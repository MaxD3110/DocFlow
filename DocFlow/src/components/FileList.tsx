import axios from "axios";
import { FileData } from "../types/File";
import { CloudArrowDownIcon, XMarkIcon } from '@heroicons/react/20/solid';
import Dropdown from "./ui-elements/Dropdown";
import Checkbox from "./ui-elements/Checkbox";
import MassOperationsPanel from "./MassOperationsPanel";
import { useNotify } from "./notifications/NotificationContext";

interface FileListProps {
    files: FileData[],
    fetchFiles: () => Promise<void>,
    selectedFileIds: number[],
    setSelectedFileIds: React.Dispatch<React.SetStateAction<number[]>>
}

const FileList = ({ files, fetchFiles, selectedFileIds, setSelectedFileIds }: FileListProps) => {
    const notify = useNotify();

    const deleteFile = async (id: number) => {
        try {
            await axios.delete<number>(`/api/files/${id}`);
        } catch (error) {
            notify("Failed to delete file", "error");
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

    return (
        <div className="w-full rounded-2xl overflow-visible bg-white shadow-xs">
            <MassOperationsPanel files={files} selectedFileIds={selectedFileIds} setSelectedFileIds={setSelectedFileIds} refreshTable={fetchFiles} />
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
    );
};

export default FileList;