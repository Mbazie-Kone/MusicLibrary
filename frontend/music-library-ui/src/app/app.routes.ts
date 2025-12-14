import { Routes } from '@angular/router';
import { MediaUploadComponent } from './features/media-upload/media-upload.component';

export const routes: Routes = [
    { path: '', redirectTo: 'media/upload', pathMatch: 'full' },
    { path: 'media/upload', component: MediaUploadComponent }
];
