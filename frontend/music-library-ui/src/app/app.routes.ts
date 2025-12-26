import { Routes } from '@angular/router';
import { MediaUploadComponent } from './features/media-upload/media-upload.component';
import { MediaListComponent } from './features/media-list/media-list.component';
import { RegisterComponent } from './auth/pages/register/register.component';
import { LoginComponent } from './auth/pages/login/login.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    { path: 'login', component: LoginComponent},
    { path: 'register', component: RegisterComponent },
    { path: 'media', component: MediaListComponent, canActivate: [authGuard] },
    { path: 'media/upload', component: MediaUploadComponent, canActivate: [authGuard] },
    { path: '', redirectTo: 'login', pathMatch: 'full' }
];
