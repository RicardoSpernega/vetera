import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  private readonly pendingRequests = signal(0);
  readonly isLoading = signal(false);

  start(): void {
    this.pendingRequests.update((value) => value + 1);
    this.isLoading.set(this.pendingRequests() > 0);
  }

  stop(): void {
    this.pendingRequests.update((value) => Math.max(0, value - 1));
    this.isLoading.set(this.pendingRequests() > 0);
  }
}
