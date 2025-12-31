import { Routes } from '@angular/router';
import { MediaUploadComponent } from './features/media-upload/media-upload.component';
import { MediaListComponent } from './features/media-list/media-list.component';
import { RegisterComponent } from './auth/pages/register/register.component';
import { LoginComponent } from './auth/pages/login/login.component';
import { authGuard } from './core/guards/auth.guard';
import { guestGuard } from './core/guards/auth.guard';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { AppLayoutComponent } from './layouts/app-layout/app-layout.component';

export const routes: Routes = [

    // AUTH PAGES (NO NAVBAR)
    { 
        path: '',
        component: AuthLayoutComponent,
        children: [
            { path: '', redirectTo: 'login', pathMatch: 'full' },
            { path: 'login', component: LoginComponent, canActivate: [guestGuard] },
            { path: 'register', component: RegisterComponent, canActivate: [guestGuard] }
        ]
    },

    // APP PAGES (WITH NAVBAR)
    {
        path: 'media',
        canActivateChild: [authGuard],
        component: AppLayoutComponent,
        children: [
            { path: '', component: MediaListComponent },
            { path: 'upload', component: MediaUploadComponent }
        ]
    },

    // GLOBAL FALLBACK
    {
        path: '**',
        redirectTo: 'login'
    }
];