import { Injectable } from '@angular/core';
import { signal, Signal, WritableSignal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LayoutService {
  private _collapsed: WritableSignal<boolean> = signal(false);

  get collapsed(): Signal<boolean> {
    return this._collapsed;
  }

  toggle(): void {
    this._collapsed.set(!this._collapsed());
  }
}
