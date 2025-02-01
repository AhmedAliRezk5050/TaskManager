import { Component } from '@angular/core';
import {Router, RouterModule} from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, InputTextModule, ButtonModule, ToastModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerData = { username: '', password: '' };

  constructor(private authService: AuthService, private router: Router) {}

  onRegister() {
    this.authService.register(this.registerData).subscribe({
      next: () => {
        // this.messageService.add({ severity: 'success', summary: 'Registration Successful', detail: 'Redirecting to login...' });
        setTimeout(() => this.router.navigate(['/auth/login']), 1500);
      },
      error: () => {
        // this.messageService.add({ severity: 'error', summary: 'Registration Failed', detail: 'Try again' });
      }
    });
  }
}
