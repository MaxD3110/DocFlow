import { Description, Dialog, DialogBackdrop, DialogPanel, DialogTitle } from '@headlessui/react'
import { ConvertibleToExtension } from '../../types/ConvertibleToExtension';

interface PopupConvertProps {
  isOpen: boolean,
  setIsOpen: (open: boolean) => void,
  selectedExtensions: ConvertibleToExtension[]
}

function PopupConvert({ isOpen, setIsOpen, selectedExtensions }: PopupConvertProps) {
  return (
      <Dialog open={isOpen} onClose={() => setIsOpen(false)} className="relative z-10 focus:outline-none">
        <DialogBackdrop transition className="fixed inset-0 bg-black/30 matte-effect duration-300" />
        <div className="fixed inset-0 z-10 w-screen overflow-y-auto">
          <div className="flex min-h-full items-center justify-center p-4">
          <DialogPanel transition className="w-full max-w-md rounded-xl bg-white p-6 duration-300 ease-out data-[closed]:transform-[scale(95%)] data-[closed]:opacity-0">
            <DialogTitle className="font-bold">Multiple files convertation</DialogTitle>
            {selectedExtensions.length > 1 ? (
              <div>
                <Description>You've chosen files with <b>different</b> extensions: <b>{selectedExtensions.map(i => `${i.name} `)}</b></Description>
                <p>Convertation options <b>can be limited</b> by the common available convertation compatibility of this extensions</p>
              </div>
            ) : (
              <Description>You've chosen files with <b>{selectedExtensions.length > 0 ? selectedExtensions[0].name : ''}</b> extension</Description>
            )}
            <Description>Choose desired format to convert selected files</Description>
            <div className="flex gap-4">
              <button onClick={() => setIsOpen(false)}>Cancel</button>
              <button onClick={() => setIsOpen(false)}>Convert</button>
            </div>
          </DialogPanel>
          </div>
        </div>
      </Dialog>
  )
}

export default PopupConvert;