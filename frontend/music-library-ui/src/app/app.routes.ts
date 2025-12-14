import { Routes } from '@angular/router';
import { MediaUploadComponent } from './features/media-upload/media-upload.component';
import { MediaListComponent } from './features/media-list/media-list.component';

export const routes: Routes = [
    { path: '', redirectTo: 'media/upload', pathMatch: 'full' },
    { path: 'media', component: MediaListComponent },
    { path: 'media/upload', component: MediaUploadComponent }
];
