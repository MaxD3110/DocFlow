import React, { useEffect, useRef, useState } from "react";
import {
    CheckCircleIcon,
    ExclamationCircleIcon,
    InformationCircleIcon,
    XCircleIcon
} from "@heroicons/react/24/solid";

type NotificationItemProps = {
    message: string;
    onClose: () => void;
    type?: "success" | "error" | "warning" | "info";
};

export const NotificationItem: React.FC<NotificationItemProps> = ({
    message,
    type = "info"
}) => {
    const [visible, setVisible] = useState(false);
    const timeoutRef = useRef<number | null>(null);
    const delay = 5000;

    const icons = {
        success: <CheckCircleIcon className="h-6 w-6 text-green-500" />,
        error: <XCircleIcon className="h-6 w-6 text-red-500" />,
        warning: <ExclamationCircleIcon className="h-6 w-6 text-yellow-500" />,
        info: <InformationCircleIcon className="h-6 w-6 text-blue-500" />
    };

    const colors = {
        success: "border-green-500 bg-green-50",
        error: "border-red-500 bg-red-50",
        warning: "border-yellow-500 bg-yellow-50",
        info: "border-blue-500 bg-blue-50"
    };

    useEffect(() => {
        requestAnimationFrame(() => setVisible(true));
        startAutoDismiss();

        return () => {
            if (timeoutRef.current) {
                clearTimeout(timeoutRef.current);
            }
        };
    }, []);

    const startAutoDismiss = () => {
        timeoutRef.current = setTimeout(() => setVisible(false), delay - 200);
    };

    const pauseDismiss = () => {
        if (timeoutRef.current) {
            clearTimeout(timeoutRef.current);
        }
        
    };

    return (
        <div
            onMouseEnter={pauseDismiss}
            onMouseLeave={startAutoDismiss}
            onClick={() => setVisible(false)}
            className={`relative w-full cursor-pointer max-w-xs flex items-center gap-2 px-3 py-2 rounded-full border-2 shadow transition-all duration-200
        ${colors[type]} 
        ${visible ? "opacity-100 translate-y-0" : "opacity-0 translate-y-4"}
      `}
        >
            {icons[type]}
            <span className="text-lg font-semibold text-gray-700 flex-1">{message}</span>
        </div>
    );
};
