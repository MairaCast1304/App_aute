import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.component.html'
})
export class ProfileComponent implements OnInit {
  user: any = { name: '', email: '' };
  newPassword = '';
  constructor(private svc: UserService, private auth: AuthService) {}

  ngOnInit(): void {
    const id = this.auth.getUserId();
    if (id) this.svc.getById(id).subscribe(u => this.user = u);
  }

  update() {
    if (!confirm('¿Seguro que deseas actualizar tu perfil?')) return;
    this.svc.updateProfile({ name: this.user.name, password: this.newPassword || null })
      .subscribe(()=> alert('Perfil actualizado'), e=> alert(e?.error?.error ?? 'Error'));
  }
}
