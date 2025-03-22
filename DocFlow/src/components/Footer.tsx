import { useServiceStatuses } from "./ServiceStatusProvider";

const Footer: React.FC = () => {
  const statuses = useServiceStatuses();

  const getIndicator = (isOnline: boolean) => (
    <span className={`w-3 h-3 ml-2 rounded-full ${isOnline ? "bg-green-500" : "bg-red-500"}`} />
  );

  return (
    <footer className="mt-20 bottom-0 w-full bg-white border-t-gray-200 border-t-1 p-4 flex justify-center space-x-6">
      <div className="flex items-center">
        Manager Service {getIndicator(statuses.manager)}
      </div>
      <div className="flex items-center">
        Convertation Service {getIndicator(statuses.processor)}
      </div>
    </footer>
  );
};

export default Footer;
