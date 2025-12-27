import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loading = false;
  errorMessage?: string;

  form: FormGroup;

  constructor(private fb: FormBuilder, private authservice: AuthService, private router: Router){
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  submit(): void {
    this.errorMessage = undefined;

    if (this.form.invalid) {
      this.errorMessage = 'Please enter valid credentials.';
      return;
    }

    this.loading = true;

    const { email, password } = this.form.value;

    this.authservice.login(email!, password!)
      .subscribe({
        next: () => {
          this.loading = false;
          
          // Redirect UX
          this.router.navigate(['/']);
        },
        error: err => {
          this.errorMessage = err.error || 'Invalid credentials.';
          this.loading = false;
        }
      });
  }
}
