import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  email = '';
  password = '';
  error = '';

  constructor(private auth: AuthService, private router: Router) {}

  onLogin() {
    this.auth.login(this.email, this.password).subscribe({
      next: () => {
        const role = this.auth.getUserRole();
        if (role === 'admin') this.router.navigate(['/users']);
        else this.router.navigate(['/profile']);
      },
      error: (err) =>
        this.error = err?.error?.error ?? 'Credenciales inválidas'
    });
  }
  goToRegister() {
    window.location.href = '/register';
  }
}