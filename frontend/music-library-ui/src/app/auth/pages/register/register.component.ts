import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  standalone: true,
  selector: 'app-register',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  loading = false;
  successMessage?: string;
  errorMessage?: string;

  form: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService)
  {
      this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  submit(): void {
    this.successMessage = undefined;
    this.errorMessage = undefined;

    if (this.form.invalid) {
      this.errorMessage = 'Please fill in all required fields.';
      return;
    }

    this.loading = true;

    const { email, password } = this.form.value;

    this.authService.register(email!, password!)
      .subscribe({
        next: msg => {
          this.successMessage = msg;
          this.form.reset();
          this.loading = false;
        },
        error: err => {
          this.errorMessage = err.error || 'Registration failed.';
          this.loading = false;
        }
      }
    );
  }
}
