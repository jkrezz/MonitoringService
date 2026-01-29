import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SessionCreateDto } from '../models/session.model';
import { DeviceDto } from '../models/device.model';

/**
 * Сервис для работы с API.
 * Отвечает за запрос списка устройств и отправку сессий на backend.
 */
@Injectable({ providedIn: 'root' })
export class MonitoringApiService {

  private readonly baseUrl = '';

  constructor(private readonly http: HttpClient) {}

  /**
   * Получает список доступных устройств.
   */
  getDevices(): Observable<DeviceDto[]> {
    return this.http.get<DeviceDto[]>(`${this.baseUrl}/api/Devices`);
  }

  /**
   * Создаёт сессию для выбранного устройства.
   * @param dto Данные новой сессии.
   */
  createSession(dto: SessionCreateDto): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/api/sessions`, dto);
  }
}

