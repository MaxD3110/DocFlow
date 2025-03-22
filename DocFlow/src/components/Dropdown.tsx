import { ChevronDownIcon } from '@heroicons/react/20/solid';
import { ConvertibleToExtension } from '../types/ConvertibleToExtension';
import { useServiceStatuses } from './ServiceStatusProvider';

interface DropdownProps {
  convertibleTo: ConvertibleToExtension[]
}

const Dropdown = ({ convertibleTo }: DropdownProps) => {
  const statuses = useServiceStatuses();

  return (

    <div className="mt-2 grid grid-cols-1">
      <select
        disabled={!statuses.processor}
        className="col-start-1 row-start-1 appearance-none button-standart bg-transparent text-gray-800 border-2 disabled:border-gray-300 disabled:text-gray-300 hover:border-blue-600 hover:text-blue-600"
      >
        {convertibleTo.map(extension => (
          <option key={extension.id} value={extension.id}>{extension.name}</option>
        ))}
      </select>
      <ChevronDownIcon
        aria-hidden="true"
        className="pointer-events-none col-start-1 row-start-1 mr-2 size-5 self-center justify-self-end text-gray-500 sm:size-4"
      />
    </div>
  )
}

export default Dropdown;
