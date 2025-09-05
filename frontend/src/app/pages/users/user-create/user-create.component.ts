import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-user-create',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-create.component.html'
})
export class UserCreateComponent {
  name=''; email=''; password=''; role='user';
  constructor(private svc: UserService, private router: Router) {}
  submit(){
    if (!confirm('¿Crear usuario?')) return;
    this.svc.create({ name:this.name, email:this.email, password:this.password, role:this.role })
      .subscribe(() => this.router.navigate(['/users']), e => alert(e?.error?.error ?? 'Error'));
  }
}

