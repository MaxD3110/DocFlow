import { useServiceStatuses } from "./ServiceStatusProvider";

const Footer: React.FC = () => {
  const statuses = useServiceStatuses();

  const getIndicator = (isOnline: boolean) => (
    <div className={`flex-none rounded-full ${isOnline ? "bg-emerald-500/50" : "bg-red-500/50"} p-1`}>
      <div className={`size-1.5 rounded-full ${isOnline ? "bg-emerald-500" : "bg-red-500"}`}></div>
    </div>
  );

  return (
    <footer className="mt-20 bottom-0 w-full bg-white border-t-gray-200 border-t-1 p-4 flex justify-center space-x-6">
      <div className="flex items-center gap-1.5">
        Manager Service {getIndicator(statuses.manager)}
      </div>
      <div className="flex items-center gap-1.5">
        Convertation Service {getIndicator(statuses.processor)}
      </div>
    </footer>
  );
};

export default Footer;
