import React, { createContext, useContext, useState, useCallback } from "react";
import { NotificationType } from "../../types/NotificationType";
import { NotificationItem } from "./NotificationItem";

type NotificationContextType = {
    notify: (message: string, type?: NotificationType["type"]) => void;
};

const NotificationContext = createContext<NotificationContextType | null>(null);

export const useNotify = () => {
    const context = useContext(NotificationContext);
    if (!context) throw new Error("Notification context not found");
    return context.notify;
};

export const NotificationProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [notifications, setNotifications] = useState<NotificationType[]>([]);

    const notify = useCallback((message: string, type: NotificationType["type"] = "info") => {
        const id = crypto.randomUUID();
        setNotifications((prev) => [...prev, { id, message, type }]);
    }, []);

    const remove = (id: string) => {
        setNotifications((prev) => prev.filter((n) => n.id !== id));
    };

    return (
        <NotificationContext.Provider value={{ notify }}>
            {children}
            <div className="fixed bottom-6 left-1/2 transform -translate-x-1/2 flex flex-col gap-2 items-center z-50">
                {notifications.map((n) => (
                    <NotificationItem
                        key={n.id}
                        message={n.message}
                        type={n.type}
                        onClose={() => remove(n.id)}
                    />
                ))}
            </div>
        </NotificationContext.Provider>
    );
};