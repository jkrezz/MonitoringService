import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DeviceDto } from '../models/device.model';
import { SessionDto } from '../models/session.model';

/**
 * Сервис для работы с API.
 * Содержит методы для загрузки устройств, сессий и скачивания бэкапа.
 */
@Injectable({ providedIn: 'root' })
export class MonitoringApiService {

  private readonly baseUrl = '';

  constructor(private readonly http: HttpClient) {}

  /**
   * Получает список устройств.
   */
  getDevices(): Observable<DeviceDto[]> {
    return this.http.get<DeviceDto[]>(`${this.baseUrl}/api/Devices`);
  }

  /**
   * Получает список сессий для указанного устройства.
   * @param deviceId Идентификатор устройства.
   */
  getSessionsByDevice(deviceId: string): Observable<SessionDto[]> {
    return this.http.get<SessionDto[]>(`${this.baseUrl}/api/Devices/${deviceId}/sessions`);
  }

  /**
   * Скачивает JSON-бэкап устройств и сессий.
   */
  downloadBackup(): Observable<HttpResponse<Blob>> {
    return this.http.get(`${this.baseUrl}/api/backup`, {
      observe: 'response',
      responseType: 'blob'
    });
  }
}

