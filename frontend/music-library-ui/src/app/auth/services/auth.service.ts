import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

interface LoginResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  private readonly baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) { }

  /**
   * Register a new user
   */
  register(email: string, password: string): Observable<string> {
    return this.http.post(
      `${this.baseUrl}/register`, { email, password }, { responseType: 'text' }
    );
  }

  /**
   * Login user and store JWT token
   */
  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}/login`, { email, password })
    .pipe(tap(response => {
      localStorage.setItem('jwt', response.token);
    })
    );
  }

  /**
   * Remove JWT token
   */
  logout(): void {
    localStorage.removeItem('jwt');
  }

  /**
   * Get stored JWT token
   */
  getToken(): string | null {
    return localStorage.getItem('jwt');
  }

  /**
   * Check if user is authenticated
   */
  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}
