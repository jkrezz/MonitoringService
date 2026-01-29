/**
 * DTO для отправки запроса создания сессии.
 */
export interface SessionCreateDto {
  /** Идентификатор устройства, для которого создаётся сессия. */
  _id: string;
  /** Имя пользователя. */
  name: string;
  /** Время начала сессии. */
  startTime: string;
  /** Время окончания сессии. */
  endTime: string;
  /** Версия ПО. */
  version: string;
}
