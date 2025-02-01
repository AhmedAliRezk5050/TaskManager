import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import {MessageService} from "primeng/api";

export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const messageService = inject(MessageService);

   if (!authService.getToken()) {
     router.navigate(['/auth/login']);

     messageService.add({
       severity: 'warn',
       summary: 'Session Expired',
       detail: 'Please login again',
     });

     return false;
   }

  return true;
};
