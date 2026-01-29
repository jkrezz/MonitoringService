/**
 * DTO сессии устройства, возвращаемое с backend.
 */
export interface SessionDto {
  /** Время начала сессии. */
  startTime: string;
  /** Время окончания сессии */
  endTime: string;
  /** Версия ПО. */
  version?: string;
}

