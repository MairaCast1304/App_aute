import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  name = ''; email = ''; password = ''; desiredRole = 'user'; adminCreationPassword = ''; error = '';
  constructor(private auth: AuthService, private router: Router) {}

  onRegister() {
    if (this.desiredRole === 'admin' && !this.adminCreationPassword) { this.error = 'Clave de creación de admin requerida'; return; }
    const payload = { name: this.name, email: this.email, password: this.password, desiredRole: this.desiredRole, adminCreationPassword: this.desiredRole === 'admin' ? this.adminCreationPassword : null };
    if (!confirm('Se creará la cuenta, ¿deseas continuar?')) return;
    this.auth.register(payload).subscribe({
      next: () => this.router.navigate(['/login']),
      error: e => this.error = (e?.error?.error ?? e?.error ?? 'Error en registro')
    });
  }
}
