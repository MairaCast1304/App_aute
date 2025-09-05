import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  template: `
    <div *ngIf="!isOnline" class="offline-banner">Buscando red...</div>
    <router-outlet></router-outlet>
  `,
  styles: [`
    .offline-banner {
      background: #ffcc00;
      color: #000;
      text-align: center;
      padding: 10px;
      font-weight: bold;
    }
  `]
})
export class AppComponent {
  isOnline = navigator.onLine;
  constructor() {
    window.addEventListener('online',  () => this.isOnline = true);
    window.addEventListener('offline', () => this.isOnline = false);
  }
}
