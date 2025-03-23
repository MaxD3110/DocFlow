import { useState, ChangeEvent, useEffect } from "react";
import { Description, Dialog, DialogBackdrop, DialogPanel, DialogTitle } from '@headlessui/react'
import axios from "axios";
import FileUpload from "./FileUpload";
import { XCircleIcon } from "@heroicons/react/20/solid";

interface PopupUploadProps {
    isOpen: boolean,
    setIsOpen: (open: boolean) => void,
    onUploadSuccess: () => void
}

const PopupUpload = ({ isOpen, setIsOpen, onUploadSuccess }: PopupUploadProps) => {
    return (
        <Dialog open={isOpen} onClose={() => setIsOpen(false)} className="relative z-10 focus:outline-none">
            <DialogBackdrop transition className="fixed inset-0 bg-black/30 matte-effect duration-300" />
            <div className="fixed inset-0 z-10 w-screen overflow-y-auto">
                <div className="flex min-h-full items-center justify-center p-4">
                    <DialogPanel transition className="w-full max-w-md rounded-xl bg-white p-6 duration-300 ease-out data-[closed]:transform-[scale(95%)] data-[closed]:opacity-0">
                        <div className="pb-5 flex justify-between items-start w-full">
                            <DialogTitle className="text-2xl font-bold">File upload</DialogTitle>
                            <button
                                className="rounded-full text-gray-400 hover:text-red-400 duration-150"
                                onClick={() => setIsOpen(false)}>
                                <XCircleIcon aria-hidden="true" className="h-10 w-10" />
                            </button>
                        </div>
                        <FileUpload onUploadSuccess={onUploadSuccess} />
                    </DialogPanel>
                </div>
            </div>
        </Dialog>



    );
}

export default PopupUpload;
