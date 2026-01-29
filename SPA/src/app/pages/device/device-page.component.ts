import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { MonitoringApiService } from '../../services/monitoring-api.service';
import { SessionDto } from '../../models/session.model';

/**
 * Страница с деталями устройства и списком его сессий.
 * Загружает и отображает сессии по идентификатору устройства из маршрута.
 */
@Component({
  selector: 'app-device-page',
  templateUrl: './device-page.component.html',
  styleUrls: ['./device-page.component.less']
})
export class DevicePageComponent implements OnInit, OnDestroy {
  /** Идентификатор устройства из параметров маршрута. */
  deviceId: string | null = null;
  /** Список сессий выбранного устройства. */
  sessions: SessionDto[] = [];

  isLoading = false;

  error: string | null = null;

  private sub?: Subscription;

  /** Есть ли сессии у устройства. */
  get hasSessions(): boolean {
    return this.sessions && this.sessions.length > 0;
  }

  /** Отсутствуют ли сессии у устройства. */
  get noSessions(): boolean {
    return !this.sessions || this.sessions.length === 0;
  }

  constructor(
    private readonly route: ActivatedRoute,
    private readonly api: MonitoringApiService
  ) {}

  ngOnInit(): void {
    this.sub = this.route.paramMap.subscribe((p) => {
      this.deviceId = p.get('id');
      this.load();
    });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  /**
   * Загружает сессии для текущего устройства.
   * При отсутствии идентификатора показывает ошибку.
   */
  load(): void {
    if (!this.deviceId) {
      this.sessions = [];
      this.error = 'Некорректный идентификатор устройства';
      return;
    }

    this.isLoading = true;
    this.error = null;

    this.api.getSessionsByDevice(this.deviceId).subscribe({
      next: (sessions) => {
        this.sessions = sessions ?? [];
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Не удалось загрузить сессии устройства';
        this.isLoading = false;
      }
    });
  }

  formatDate(iso: string): string {
    const d = new Date(iso);
    if (Number.isNaN(d.getTime())) return iso;
    return d.toLocaleString();
  }

  /**
   * Рассчитывает продолжительность сессии в миллисекундах.
   * @param s Сессия.
   */
  durationMs(s: SessionDto): number | null {
    const start = new Date(s.startTime).getTime();
    const end = new Date(s.endTime).getTime();
    if (Number.isNaN(start) || Number.isNaN(end)) return null;
    return end - start;
  }

  /**
   * Возвращает продолжительность сессии в секундах (округлённую).
   * @param s Сессия.
   */
  getDuration(s: SessionDto): number | null {
    const ms = this.durationMs(s);
    if (ms === null) return null;
    return Math.round(ms / 1000);
  }
}

