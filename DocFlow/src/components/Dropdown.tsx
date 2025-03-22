import { Menu, MenuButton, MenuItem, MenuItems } from '@headlessui/react';
import { ChevronDownIcon } from '@heroicons/react/20/solid';
import { ConvertibleToExtension } from '../types/ConvertibleToExtension';
import { useServiceStatuses } from './ServiceStatusProvider';

interface DropdownProps {
  id: number,
  convertibleTo: ConvertibleToExtension[]
}

const Dropdown = ({ id, convertibleTo }: DropdownProps) => {
  const statuses = useServiceStatuses();

  return (
    <Menu as="div" className="relative inline-block text-left">
      <div>
        <MenuButton
          disabled={!statuses.processor}
          className="inline-flex w-full justify-center gap-x-1.5 rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 ring-1 shadow-xs ring-gray-300 ring-inset hover:bg-gray-50, disabled:text-gray-300">
          Convert to
          <ChevronDownIcon aria-hidden="true" className="-mr-1 size-5 text-gray-400" />
        </MenuButton>
      </div>
      <MenuItems transition className="absolute right-0 z-10 mt-2 w-56 origin-top-right rounded-md bg-white ring-1 shadow-lg ring-black/5 transition focus:outline-hidden data-closed:scale-95 data-closed:transform data-closed:opacity-0 data-enter:duration-100 data-enter:ease-out data-leave:duration-75 data-leave:ease-in">
        <div className="py-1">
          {convertibleTo.map(extension => (
            <MenuItem key={extension.id}>
              <a href="#" className="block px-4 py-2 text-sm text-gray-700 data-focus:bg-gray-100 data-focus:text-gray-900 data-focus:outline-hidden">
                {extension.name}
              </a>
            </MenuItem>
          ))}
        </div>
      </MenuItems>
    </Menu>
  )
}

export default Dropdown;
