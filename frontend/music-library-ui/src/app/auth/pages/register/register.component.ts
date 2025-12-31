import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterLink } from "@angular/router";

@Component({
  standalone: true,
  selector: 'app-register',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  loading = false;
  successMessage?: string;
  errorMessage?: string;

  private readonly redirectDelayMs = 3000;

  form: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router)
  {
      this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  submit(): void {
    this.errorMessage = undefined;

    if (this.form.invalid) {
      this.errorMessage = 'Please fill in all required fields.';
      return;
    }

    this.loading = true;

    const { email, password } = this.form.value;

    this.authService.register(email!, password!)
      .subscribe({
        next: () => {
          this.successMessage ='Registration completed. You will be redirected to the login…';

          setTimeout(() => {
            this.router.navigate(['/login']);
          }, this.redirectDelayMs);
        },
        error: err => {
          this.errorMessage = err.error || 'Registration failed.';
          this.loading = false;
        }
      }
    );
  }
}
