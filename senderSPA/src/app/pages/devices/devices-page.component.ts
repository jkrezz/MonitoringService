import { Component, OnInit } from '@angular/core';
import { MonitoringApiService } from '../../services/monitoring-api.service';
import { SessionCreateDto } from '../../models/session.model';
import { DeviceDto } from '../../models/device.model';

/**
 * Страница отправки сессий для выбранного устройства.
 * Позволяет выбрать устройство, указать интервал и отправить данные в API.
 */
@Component({
  selector: 'app-devices-page',
  templateUrl: './devices-page.component.html',
  styleUrls: ['./devices-page.component.less']
})
export class DevicesPageComponent implements OnInit {
  /** Список устройств, полученных с backend. */
  devices: DeviceDto[] = [];

  /** Текущая сессия, подготавливаемая к отправке. */
  session: SessionCreateDto = {
    _id: '',
    name: '',
    startTime: '',
    endTime: '',
    version: ''
  };

  startLocal = '';
  endLocal = '';

  isLoadingDevices = false;

  isSending = false;

  error: string | null = null;

  success: string | null = null;

  constructor(private readonly api: MonitoringApiService) {}

  ngOnInit(): void {
    this.loadDevices();
  }

  /**
   * Загружает список устройств с сервера.
   */
  loadDevices(): void {
    this.isLoadingDevices = true;
    this.error = null;

    this.api.getDevices().subscribe({
      next: (devices) => {
        this.devices = devices ?? [];
        this.isLoadingDevices = false;
      },
      error: () => {
        this.isLoadingDevices = false;
        this.error = 'Не удалось загрузить список устройств';
      }
    });
  }

  onDeviceChange(deviceId: string): void {
    this.session._id = deviceId;

    const selectedDevice = this.devices.find((d) => d.id === deviceId);
    this.session.name = selectedDevice ? selectedDevice.name : '';
  }

  /**
   * Валидирует введённые данные и отправляет новую сессию в API.
   */
  send(): void {
    if (!this.session._id) {
      this.error = 'Выберите устройство';
      return;
    }

    if (!this.startLocal || !this.endLocal) {
      this.error = 'Укажите начало и окончание сессии';
      return;
    }

    const start = new Date(this.startLocal);
    const end = new Date(this.endLocal);

    if (end.getTime() <= start.getTime()) {
      this.error = 'Некорректный интервал: окончание должно быть позже начала';
      return;
    }

    this.session.startTime = start.toISOString();
    this.session.endTime = end.toISOString();

    this.isSending = true;
    this.error = null;
    this.success = null;

    this.api.createSession(this.session).subscribe({
      next: () => {
        this.isSending = false;
        this.success = 'Сессия успешно отправлена';

        this.loadDevices();

        this.session = {
          _id: this.session._id,
          name: '',
          startTime: '',
          endTime: '',
          version: ''
        };
        this.startLocal = '';
        this.endLocal = '';
      },
      error: () => {
        this.isSending = false;
        this.error = 'Не удалось отправить сессию';
      }
    });
  }
}

