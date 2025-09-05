import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-user-edit',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-edit.component.html'
})
export class UserEditComponent implements OnInit {
  user: any = null;
  newPassword = '';
  constructor(private route: ActivatedRoute, private svc: UserService, private router: Router) {}
  ngOnInit(){
    const id = this.route.snapshot.paramMap.get('id')!;
    this.svc.getById(id).subscribe(u => this.user = u);
  }
  submit(){
    if (!this.user) return;
    if (!confirm('¿Guardar cambios?')) return;
    const payload: any = { email: this.user.email, name: this.user.name, role: this.user.role };
    if (this.newPassword) payload.password = this.newPassword;
    this.svc.update(this.user.id, payload).subscribe(()=>this.router.navigate(['/users']), e=>alert(e?.error?.error ?? 'Error'));
  }
}
