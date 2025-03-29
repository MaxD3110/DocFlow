import { ChevronDownIcon } from '@heroicons/react/20/solid';
import { ConvertibleToExtension } from '../types/ConvertibleToExtension';
import { useServiceStatuses } from './ServiceStatusProvider';
import { Menu, MenuButton, MenuItem, MenuItems } from '@headlessui/react';
import { useEffect, useRef, useState } from 'react';

interface DropdownProps {
  convertibleTo: ConvertibleToExtension[]
}

const Dropdown = ({ convertibleTo }: DropdownProps) => {
  const [isOpened, setOpened] = useState(false);
  const statuses = useServiceStatuses();
  const isActive = convertibleTo.length > 0 && statuses.processor;
  const buttonRef = useRef<HTMLButtonElement>(null);

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (buttonRef.current && !buttonRef.current.contains(event.target as Node))
        buttonRef.current?.click();
    }

    if (isOpened) {
      document.addEventListener("mousedown", handleClickOutside);
    }

    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [isOpened]);

  return (
    <Menu as="div" className="relative inline-block text-left">
      <MenuButton
        ref={buttonRef}
        disabled={!isActive}
        onClick={() => setOpened(isActive ? !isOpened : false)}
        className={`${isOpened ? 'rounded-t-4xl' : 'rounded-4xl'} ${isActive ? 'border-verdigris bg-white' : 'border-red-400 bg-gray-100'}
          inline-flex w-32 justify-center z-2 relative gap-x-1.5 px-3 py-3 text-sm font-semibold border-t-4 text-gray-700 shadow-xs hover:bg-gray-50 duration-200`}>
        Options
        <ChevronDownIcon aria-hidden="true" className={`${isOpened ? 'rotate-180' : ''} -mr-1 size-5 text-gray-700 duration-150`} />
      </MenuButton>

      <MenuItems
        transition
        className="absolute right-0 z-3 font-bold origin-top-right w-32 rounded-b-4xl bg-verdigris shadow-lg
        transition transform-gpu data-closed:-translate-y-6 data-closed:transform data-closed:z-1 data-closed:opacity-0
        data-enter:duration-300 data-enter:ease-out data-leave:duration-60"
      >
        <div className="mb-1 rounded-b-4xl bg-white">
          {convertibleTo.map(extension => (
            <MenuItem key={extension.id}>
              <a
                href="#"
                className="block px-4 py-2 text-center text-sm text-gray-700 last:rounded-b-4xl data-focus:bg-gray-100 data-focus:text-gray-900 data-focus:outline-hidden duration-150"
              >
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

