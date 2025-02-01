import { Routes } from '@angular/router';
import { authGuard } from './auth/guards/auth.guard';

export const routes: Routes = [
    {
        path: 'auth',
        loadChildren: () => import('./auth/auth.routes').then(m => m.authRoutes)
    },
    {
        path: 'dashboard',
        loadChildren: () => import('./dashboard/dashboard.routes').then(m => m.dashboardRoutes),
        canActivate: [authGuard]
    },
    { path: '**', redirectTo: 'auth/login' }
];
