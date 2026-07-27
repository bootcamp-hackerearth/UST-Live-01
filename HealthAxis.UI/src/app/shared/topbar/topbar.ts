import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  HostListener,
  computed,
  OnDestroy,
  inject,
  output,
  signal
} from '@angular/core';
import {
  Router,
  RouterLink
} from '@angular/router';

import {
  PortalNotification
} from '../../core/models/notification.model';
import { AuthService } from '../../core/services/auth.service';
import { DoctorService } from '../../core/services/doctor.service';
import {
  DoctorStatusStateService
} from '../../core/services/doctor-status-state.service';
import {
  NotificationService
} from '../../core/services/notification.service';
import { PatientService } from '../../core/services/patient.service';
import {
  getFriendlyErrorMessage
} from '../../core/utils/api-error.util';

const CLOCK_INTERVAL_IN_MS = 1000;
const ERROR_DURATION_IN_MS = 3000;
const NOTIFICATION_POLL_INTERVAL_IN_MS = 30000;
const MAX_VISIBLE_NOTIFICATION_COUNT = 99;

const SECONDS_PER_MINUTE = 60;
const MINUTES_PER_HOUR = 60;
const HOURS_PER_DAY = 24;
const DAYS_PER_WEEK = 7;

const DOCTOR_REFERENCE_START = 'with ';
const DOCTOR_REFERENCE_END = ' is ';

@Component({
  selector: 'app-topbar',
  imports: [
    DatePipe,
    RouterLink
  ],
  templateUrl: './topbar.html',
  styleUrl: './topbar.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Topbar implements OnDestroy {
  readonly authService = inject(AuthService);

  readonly doctorStatusState =
    inject(DoctorStatusStateService);

  private readonly router = inject(Router);
  private readonly doctorService = inject(DoctorService);
  private readonly patientService = inject(PatientService);

  private readonly notificationService =
    inject(NotificationService);

  readonly logoutRequested = output<void>();

  readonly currentTime = signal(new Date());
  readonly displayName = signal('');

  readonly isProfileMenuOpen = signal(false);
  readonly isNotificationMenuOpen = signal(false);

  readonly statusUpdating = signal(false);
  readonly statusError = signal('');

  readonly notifications =
    signal<PortalNotification[]>([]);

  readonly unreadNotificationCount = signal(0);

  readonly hasUnreadNotifications = computed(
    () =>
      this.unreadNotificationCount() > 0 ||
      this.notifications().some(
        (notification) => !notification.isRead
      )
  );

  readonly notificationsLoading = signal(false);
  readonly notificationError = signal('');
  readonly markAllUpdating = signal(false);

  readonly selectedNotification =
    signal<PortalNotification | null>(null);

  readonly notificationDialogError = signal('');

  private readonly clockTimerId =
    globalThis.setInterval(() => {
      this.currentTime.set(new Date());
    }, CLOCK_INTERVAL_IN_MS);

  private readonly notificationTimerId: number;

  constructor() {
    this.loadCurrentUser();
    this.loadUnreadNotificationCount();

    this.notificationTimerId =
      globalThis.setInterval(() => {
        this.loadUnreadNotificationCount();
      }, NOTIFICATION_POLL_INTERVAL_IN_MS);
  }

  ngOnDestroy(): void {
    globalThis.clearInterval(this.clockTimerId);

    globalThis.clearInterval(
      this.notificationTimerId
    );
  }

  @HostListener('document:click')
  closeMenus(): void {
    this.isProfileMenuOpen.set(false);
    this.isNotificationMenuOpen.set(false);
  }

  @HostListener('document:keydown.escape')
  handleEscapeKey(): void {
    if (this.selectedNotification()) {
      this.closeNotificationDialog();
      return;
    }

    this.closeMenus();
  }

  toggleProfileMenu(): void {
    this.isNotificationMenuOpen.set(false);

    this.isProfileMenuOpen.update(
      (isOpen) => !isOpen
    );
  }

  toggleNotificationMenu(): void {
    this.isProfileMenuOpen.set(false);

    const shouldOpen =
      !this.isNotificationMenuOpen();

    this.isNotificationMenuOpen.set(shouldOpen);
    this.notificationError.set('');

    if (shouldOpen) {
      this.loadNotifications();
    }
  }

  refreshNotifications(): void {
    this.loadNotifications();
    this.loadUnreadNotificationCount();
  }

  openNotificationDialog(
    notification: PortalNotification
  ): void {
    this.selectedNotification.set(notification);
    this.isNotificationMenuOpen.set(false);
    this.notificationDialogError.set('');

    if (!notification.isRead) {
      this.markNotificationAsRead(notification);
    }
  }

  closeNotificationDialog(): void {
    this.selectedNotification.set(null);
    this.notificationDialogError.set('');
  }

  markNotificationAsRead(
    notification: PortalNotification
  ): void {
    if (notification.isRead) {
      return;
    }

    this.notificationDialogError.set('');

    this.notificationService
      .markAsRead(notification.notificationId)
      .subscribe({
        next: () => {
          this.updateNotificationReadState(
            notification.notificationId
          );

          this.unreadNotificationCount.update(
            (currentCount) =>
              Math.max(0, currentCount - 1)
          );
        },
        error: (error: unknown) => {
          this.notificationDialogError.set(
            getFriendlyErrorMessage(
              error,
              'The notification opened, but it could not be marked as read.'
            )
          );
        }
      });
  }

  markAllNotificationsAsRead(): void {
    if (
      this.markAllUpdating() ||
      !this.hasUnreadNotifications()
    ) {
      return;
    }

    this.markAllUpdating.set(true);
    this.notificationError.set('');

    this.notificationService
      .markAllAsRead()
      .subscribe({
        next: () => {
          this.markAllUpdating.set(false);
          this.unreadNotificationCount.set(0);

          this.notifications.update(
            (currentNotifications) =>
              currentNotifications.map(
                (notification) => ({
                  ...notification,
                  isRead: true
                })
              )
          );

          this.selectedNotification.update(
            (notification) => {
              if (!notification) {
                return null;
              }

              return {
                ...notification,
                isRead: true
              };
            }
          );
        },
        error: (error: unknown) => {
          this.markAllUpdating.set(false);

          this.notificationError.set(
            getFriendlyErrorMessage(
              error,
              'Could not mark all notifications as read.'
            )
          );
        }
      });
  }

  getUnreadBadgeText(): string {
    const unreadCount =
      this.unreadNotificationCount();

    if (
      unreadCount > MAX_VISIBLE_NOTIFICATION_COUNT
    ) {
      return `${MAX_VISIBLE_NOTIFICATION_COUNT}+`;
    }

    return unreadCount.toString();
  }

  getNotificationTime(
    createdDate: string
  ): string {
    const parsedDate =
      this.parseUtcDate(createdDate);

    if (!parsedDate) {
      return '';
    }

    const createdTime = parsedDate.getTime();

    const elapsedSeconds = Math.max(
      0,
      Math.floor(
        (Date.now() - createdTime) / 1000
      )
    );

    if (elapsedSeconds < SECONDS_PER_MINUTE) {
      return 'Just now';
    }

    const elapsedMinutes = Math.floor(
      elapsedSeconds / SECONDS_PER_MINUTE
    );

    if (elapsedMinutes < MINUTES_PER_HOUR) {
      return `${elapsedMinutes} min ago`;
    }

    const elapsedHours = Math.floor(
      elapsedMinutes / MINUTES_PER_HOUR
    );

    if (elapsedHours < HOURS_PER_DAY) {
      return `${elapsedHours} hr ago`;
    }

    const elapsedDays = Math.floor(
      elapsedHours / HOURS_PER_DAY
    );

    if (elapsedDays < DAYS_PER_WEEK) {
      return elapsedDays === 1
        ? 'Yesterday'
        : `${elapsedDays} days ago`;
    }

    return parsedDate.toLocaleDateString();
  }

  getNotificationReceivedDate(
    createdDate: string
  ): Date | null {
    return this.parseUtcDate(createdDate);
  }

  getNotificationMessage(
    notification: PortalNotification
  ): string {
    const message = notification.message.trim();

    const doctorDisplayName =
      this.getDoctorDisplayName(notification);

    if (doctorDisplayName) {
      return this.replaceKnownDoctorName(
        message,
        notification.doctorName,
        doctorDisplayName
      );
    }

    return this.formatDoctorReference(
      message,
      notification.doctorSpecialisation
    );
  }

  getDoctorDisplayName(
    notification: PortalNotification
  ): string {
    const doctorName =
      notification.doctorName?.trim();

    if (!doctorName) {
      return '';
    }

    const cleanDoctorName =
      this.removeDoctorTitle(doctorName);

    const specialisation =
      notification.doctorSpecialisation?.trim();

    if (specialisation) {
      return `Dr. ${cleanDoctorName} (${specialisation})`;
    }

    return `Dr. ${cleanDoctorName}`;
  }

  getGreeting(): string {
    const hour =
      this.currentTime().getHours();

    if (hour < 12) {
      return 'Good Morning';
    }

    if (hour < 17) {
      return 'Good Afternoon';
    }

    return 'Good Evening';
  }

  getFirstName(): string {
    const name = this.displayName().trim();

    if (!name) {
      return this.authService.role() || 'User';
    }

    return name.split(/\s+/)[0];
  }

  getProfileName(): string {
    const name = this.displayName().trim();

    return name ||
      this.authService.role() ||
      'User';
  }

  getProfileInitial(): string {
    return this.getProfileName()
      .charAt(0)
      .toUpperCase();
  }

  getDoctorStatusText(): string {
    const status =
      this.doctorStatusState.isActive();

    if (status === null) {
      return 'Loading';
    }

    return status
      ? 'Available'
      : 'Unavailable';
  }

  profileRoute(): string {
    const role = this.authService.role();

    if (role === 'Doctor') {
      return '/doctor/profile';
    }

    if (role === 'Patient') {
      return '/patient/profile';
    }

    return '/login';
  }

  goToChangePassword(): void {
    this.isProfileMenuOpen.set(false);

    const role = this.authService.role();

    if (role === 'Doctor') {
      void this.navigateToPasswordSection(
        '/doctor/profile'
      );
      return;
    }

    if (role === 'Patient') {
      void this.navigateToPasswordSection(
        '/patient/profile'
      );
      return;
    }

    void this.router.navigate(['/login']);
  }

  requestLogout(): void {
    this.isProfileMenuOpen.set(false);
    this.isNotificationMenuOpen.set(false);
    this.closeNotificationDialog();
    this.logoutRequested.emit();
  }

  toggleDoctorStatus(): void {
    if (this.authService.role() !== 'Doctor') {
      return;
    }

    const currentStatus =
      this.doctorStatusState.isActive();

    if (currentStatus === null) {
      this.showStatusError(
        'Doctor status is still loading.'
      );
      return;
    }

    this.statusUpdating.set(true);
    this.statusError.set('');

    this.doctorService
      .updateMyStatus(!currentStatus)
      .subscribe({
        next: (response) => {
          this.statusUpdating.set(false);

          this.doctorStatusState.setStatus(
            response.isActive
          );
        },
        error: (error: unknown) => {
          this.statusUpdating.set(false);

          this.showStatusError(
            getFriendlyErrorMessage(
              error,
              'Could not update doctor status.'
            )
          );
        }
      });
  }

  private parseUtcDate(
    createdDate: string
  ): Date | null {
    const value = createdDate.trim();

    if (!value) {
      return null;
    }

    const includesTimeZone =
      /(?:z|[+-]\d{2}:\d{2})$/i.test(value);

    const utcValue = includesTimeZone
      ? value
      : `${value}Z`;

    const parsedDate = new Date(utcValue);

    return Number.isNaN(parsedDate.getTime())
      ? null
      : parsedDate;
  }

  private replaceKnownDoctorName(
    message: string,
    doctorName: string | null | undefined,
    doctorDisplayName: string
  ): string {
    const originalDoctorName = doctorName?.trim();

    if (!originalDoctorName) {
      return message;
    }

    const cleanDoctorName =
      this.removeDoctorTitle(originalDoctorName);

    const lowerCaseMessage =
      message.toLowerCase();

    const possibleNames = [
      originalDoctorName,
      cleanDoctorName,
      `Dr. ${cleanDoctorName}`,
      `Dr ${cleanDoctorName}`
    ];

    const matchedName = possibleNames.find(
      (name) =>
        lowerCaseMessage.includes(
          name.toLowerCase()
        )
    );

    if (!matchedName) {
      return message;
    }

    const nameStartIndex =
      lowerCaseMessage.indexOf(
        matchedName.toLowerCase()
      );

    const nameEndIndex =
      nameStartIndex + matchedName.length;

    return [
      message.slice(0, nameStartIndex),
      doctorDisplayName,
      message.slice(nameEndIndex)
    ].join('');
  }

  private formatDoctorReference(
    message: string,
    specialisation: string | null | undefined
  ): string {
    const lowerCaseMessage =
      message.toLowerCase();

    const referenceStartIndex =
      lowerCaseMessage.indexOf(
        DOCTOR_REFERENCE_START
      );

    if (referenceStartIndex < 0) {
      return message;
    }

    const doctorNameStartIndex =
      referenceStartIndex +
      DOCTOR_REFERENCE_START.length;

    const doctorNameEndIndex =
      lowerCaseMessage.indexOf(
        DOCTOR_REFERENCE_END,
        doctorNameStartIndex
      );

    if (doctorNameEndIndex < 0) {
      return message;
    }

    const doctorName = message
      .slice(
        doctorNameStartIndex,
        doctorNameEndIndex
      )
      .trim();

    if (!doctorName) {
      return message;
    }

    const cleanDoctorName =
      this.removeDoctorTitle(doctorName);

    const cleanSpecialisation =
      specialisation?.trim();

    const doctorDisplayName =
      cleanSpecialisation
        ? `Dr. ${cleanDoctorName} (${cleanSpecialisation})`
        : `Dr. ${cleanDoctorName}`;

    return [
      message.slice(0, doctorNameStartIndex),
      doctorDisplayName,
      message.slice(doctorNameEndIndex)
    ].join('');
  }

  private removeDoctorTitle(
    doctorName: string
  ): string {
    const trimmedName = doctorName.trim();
    const lowerCaseName = trimmedName.toLowerCase();

    if (lowerCaseName.startsWith('dr. ')) {
      return trimmedName.slice(4).trim();
    }

    if (lowerCaseName.startsWith('dr ')) {
      return trimmedName.slice(3).trim();
    }

    return trimmedName;
  }

  private updateNotificationReadState(
    notificationId: number
  ): void {
    this.notifications.update(
      (currentNotifications) =>
        currentNotifications.map(
          (notification) => {
            if (
              notification.notificationId !==
              notificationId
            ) {
              return notification;
            }

            return {
              ...notification,
              isRead: true
            };
          }
        )
    );

    this.selectedNotification.update(
      (notification) => {
        if (
          notification?.notificationId !==
          notificationId
        ) {
          return notification;
        }

        return {
          ...notification,
          isRead: true
        };
      }
    );
  }

  private loadNotifications(): void {
    if (!this.isNotificationRole()) {
      return;
    }

    this.notificationsLoading.set(true);
    this.notificationError.set('');

    this.notificationService
      .getMyNotifications()
      .subscribe({
        next: (notifications) => {
          this.notifications.set(notifications);

          const visibleUnreadCount =
            notifications.filter(
              (notification) =>
                !notification.isRead
            ).length;

          this.unreadNotificationCount.update(
            (currentCount) =>
              Math.max(
                currentCount,
                visibleUnreadCount
              )
          );

          this.notificationsLoading.set(false);
        },
        error: (error: unknown) => {
          this.notificationsLoading.set(false);

          this.notificationError.set(
            getFriendlyErrorMessage(
              error,
              'Could not load notifications.'
            )
          );
        }
      });
  }

  private loadUnreadNotificationCount(): void {
    if (!this.isNotificationRole()) {
      this.unreadNotificationCount.set(0);
      return;
    }

    this.notificationService
      .getUnreadCount()
      .subscribe({
        next: (response) => {
          this.unreadNotificationCount.set(
            Math.max(0, response.unreadCount)
          );
        },
        error: () => {
          // A polling failure should not interrupt the portal.
        }
      });
  }

  private isNotificationRole(): boolean {
    const role = this.authService.role();

    return role === 'Patient' ||
      role === 'Doctor';
  }

  private async navigateToPasswordSection(
    route: string
  ): Promise<void> {
    const navigated =
      await this.router.navigate([route]);

    if (!navigated) {
      return;
    }

    globalThis.setTimeout(() => {
      document
        .getElementById('change-password')
        ?.scrollIntoView({
          behavior: 'smooth',
          block: 'start'
        });
    });
  }

  private loadCurrentUser(): void {
    const role = this.authService.role();

    if (role === 'Patient') {
      this.loadPatientDetails();
      return;
    }

    if (role === 'Doctor') {
      this.loadDoctorDetails();
    }
  }

  private loadPatientDetails(): void {
    this.patientService
      .getMyProfile()
      .subscribe({
        next: (patient) => {
          this.displayName.set(
            patient.fullName
          );
        },
        error: () => {
          this.displayName.set('Patient');
        }
      });
  }

  private loadDoctorDetails(): void {
    this.doctorService
      .getMyDoctorProfile()
      .subscribe({
        next: (doctor) => {
          this.displayName.set(
            doctor.fullName
          );

          this.doctorStatusState.setStatus(
            Boolean(doctor.isActive)
          );
        },
        error: () => {
          this.displayName.set('Doctor');
          this.doctorStatusState.clearStatus();
        }
      });
  }

  private showStatusError(
    message: string
  ): void {
    this.statusError.set(message);

    globalThis.setTimeout(() => {
      this.statusError.set('');
    }, ERROR_DURATION_IN_MS);
  }
}