import { Description, Dialog, DialogBackdrop, DialogPanel, DialogTitle } from '@headlessui/react'
import { ConvertibleToExtension } from '../types/ConvertibleToExtension';

interface PopupProps {
  isOpen: boolean,
  setIsOpen: (open: boolean) => void,
  selectedExtensions: ConvertibleToExtension[]
}

function Popup({ isOpen, setIsOpen, selectedExtensions }: PopupProps) {
  return (
      <Dialog open={isOpen} onClose={() => setIsOpen(false)} className="relative z-50">
        <DialogBackdrop className="fixed inset-0 bg-black/30" />
        <div className="fixed inset-0 flex w-screen items-center justify-center p-4">
          <DialogPanel className="max-w-lg space-y-4 border bg-white p-12">
            <DialogTitle className="font-bold">Multiple files convertation</DialogTitle>
            {selectedExtensions.length > 1 ? (
              <div>
                <Description>You've chosen files with <b>different</b> extensions: <b>{selectedExtensions.map(i => `${i.name} `)}</b></Description>
                <p>Considering that, options <b>can be limited</b> by the common available convertation compatibility of this extensions</p>
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
      </Dialog>
  )
}

export default Popup;