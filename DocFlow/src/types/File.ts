import { ExtensionData } from "./Extension";

export interface FileData {
    id: number,
    name: string,
    extensionName: string,
    fileSize: number,
    uploadedAt: Date,
    storagePath: string,
    extension: ExtensionData
}