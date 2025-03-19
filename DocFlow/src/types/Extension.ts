import { ConvertibleToExtension } from "./ConvertibleToExtension"

export interface ExtensionData {
    id: number,
    name: string,
    mediaType: string,
    convertibleTo: ConvertibleToExtension[]
}