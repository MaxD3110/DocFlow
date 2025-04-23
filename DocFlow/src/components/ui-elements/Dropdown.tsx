import { ChevronDownIcon } from '@heroicons/react/20/solid';
import { ConvertibleToExtension } from '../../types/ConvertibleToExtension';
import { useServiceStatuses } from '../ServiceStatusProvider';
import { Listbox, ListboxButton, ListboxOption, ListboxOptions } from '@headlessui/react';

interface DropdownProps {
  convertibleTo: ConvertibleToExtension[],
  selectedExtensionId: number | null,
  onSelect?: (extensionId: number) => void
}

const Dropdown = ({ convertibleTo, selectedExtensionId, onSelect }: DropdownProps) => {
  const statuses = useServiceStatuses();
  const isActive = convertibleTo.length > 0 && statuses.processor;
  const selectedOption = convertibleTo.find(e => e.id === selectedExtensionId);

  return (
    <Listbox value={selectedOption} onChange={(val) => onSelect?.(val.id)} disabled={!isActive}>
      {({ open }) => (
        <div className="relative inline-block text-left w-32">
          <ListboxButton
            className={`
              ${open ? 'rounded-t-4xl' : 'rounded-4xl'}
              ${isActive ? 'border-verdigris bg-white' : 'border-red-400 bg-gray-100 cursor-not-allowed'}
              inline-flex justify-center w-full gap-x-1.5 px-3 py-3 text-sm font-semibold border-t-4 text-gray-700 shadow-xs hover:bg-gray-50 duration-200
            `}
          >
            {selectedOption?.name ?? "N/A"}
            <ChevronDownIcon
              className={`${open ? 'rotate-180' : ''} -mr-1 size-5 text-gray-700 duration-150`}
              aria-hidden="true"
            />
          </ListboxButton>

          <ListboxOptions
            transition
            className="absolute right-0 z-3 font-bold origin-top-right w-32 rounded-b-4xl bg-verdigris shadow-lg
            transition transform-gpu data-closed:-translate-y-6 data-closed:transform data-closed:z-1 data-closed:opacity-0
            data-enter:duration-300 data-enter:ease-out data-leave:duration-60"
          >
            <div className="mb-1 rounded-b-4xl bg-white hover:cursor-pointer">
              {convertibleTo.map((extension) => (
                <ListboxOption
                  key={extension.id}
                  value={extension}
                  className={({ active }) =>
                    `block px-4 py-2 text-center text-sm text-gray-700 last:rounded-b-4xl duration-150 ${active ? 'bg-gray-100 text-gray-900' : ''
                    }`
                  }
                >
                  {extension.name}
                </ListboxOption>
              ))}
            </div>
          </ListboxOptions>
        </div>
      )}
    </Listbox>
  )
}

export default Dropdown;

