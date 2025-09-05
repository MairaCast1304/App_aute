import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { UserService, User } from '../../services/user.service';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './users.component.html'
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  constructor(private svc: UserService, public router: Router) {}
  ngOnInit(){ this.load(); }
  load(){ this.svc.getAll().subscribe(u => this.users = u); }
  edit(id: string){ this.router.navigate(['/users', id, 'edit']); }
  del(id: string){ if (confirm('¿Eliminar usuario?')) this.svc.delete(id).subscribe(()=>this.load()); }
  unlock(id: string){ if (confirm('¿Desbloquear usuario?')) this.svc.unlock(id).subscribe(()=>this.load()); }
}
