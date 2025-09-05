import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

export interface User {
  id: string;
  email: string;
  name: string;
  role: string;
}

@Injectable({ providedIn: 'root' })
export class UserService {
  private apiUrl = `${environment.apiUrl}/users`;
  constructor(private http: HttpClient) {}

  getAll(): Observable<User[]> { return this.http.get<User[]>(this.apiUrl); }
  getById(id: string): Observable<User> { return this.http.get<User>(`${this.apiUrl}/${id}`); }
  create(u: any) { return this.http.post(this.apiUrl, u); }
  update(id: string, u: any) { return this.http.put(`${this.apiUrl}/${id}`, u); }
  delete(id: string) { return this.http.delete(`${this.apiUrl}/${id}`); }
  updateProfile(payload: any) { return this.http.put(`${this.apiUrl}/profile`, payload); }
  unlock(id: string) { return this.http.post(`${this.apiUrl}/${id}/unlock`, {}); }
}
