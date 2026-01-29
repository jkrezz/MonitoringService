import { Component, OnInit } from '@angular/core';
import { MonitoringApiService } from '../../services/monitoring-api.service';
import { DeviceDto } from '../../models/device.model';

/**
 * Главная страница со списком устройств.
 * Позволяет загрузить список и скачать бэкап.
 */
@Component({
  selector: 'app-devices-page',
  templateUrl: './devices-page.component.html',
  styleUrls: ['./devices-page.component.less']
})
export class DevicesPageComponent implements OnInit {
  /** Список устройств, загруженных с backend. */
  devices: DeviceDto[] = [];

  isLoading = false;

  isDownloadingBackup = false;

  error: string | null = null;

  backupSuccess: string | null = null;

  constructor(private readonly api: MonitoringApiService) {}

  /** Есть ли в списке хотя бы одно устройство. */
  get hasDevices(): boolean {
    return this.devices && this.devices.length > 0;
  }

  /** Отсутствуют ли устройства в списке. */
  get noDevices(): boolean {
    return !this.devices || this.devices.length === 0;
  }

  ngOnInit(): void {
    this.load();
  }

  /**
   * Загружает список устройств с сервера.
   */
  load(): void {
    this.isLoading = true;
    this.error = null;
    this.backupSuccess = null;

    this.api.getDevices().subscribe({
      next: (devices) => {
        this.devices = devices ?? [];
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Не удалось загрузить список устройств';
        this.isLoading = false;
      }
    });
  }

  /**
   * Отправляет запрос на скачивание бэкапа.
   */
  downloadBackup(): void {
    this.isDownloadingBackup = true;
    this.error = null;
    this.backupSuccess = null;

    this.api.downloadBackup().subscribe({
      next: (resp) => {
        const contentDisposition = resp.headers.get('content-disposition') ?? '';
        const match = /filename\*?=(?:UTF-8''|")?([^\";]+)"?/i.exec(contentDisposition);
        const fileName = (match?.[1] ? decodeURIComponent(match[1]) : null) ?? 'backup.json';

        const blob = resp.body ?? new Blob([], { type: 'application/json' });
        const url = URL.createObjectURL(blob);

        const a = document.createElement('a');
        a.href = url;
        a.download = fileName;
        a.click();

        URL.revokeObjectURL(url);

        this.backupSuccess = `Бэкап сохранён: ${fileName}`;
        this.isDownloadingBackup = false;
      },
      error: () => {
        this.error = 'Не удалось скачать бэкап';
        this.isDownloadingBackup = false;
      }
    });
  }
}

