export interface NotificationType {
  id: string;
  message: string;
  type?: "success" | "error" | "warning" | "info";
}