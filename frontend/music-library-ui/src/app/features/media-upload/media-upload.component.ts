import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpEventType } from '@angular/common/http';
import { MediaApiService } from '../../core/services/media-api.service';

@Component({
  selector: 'app-media-upload',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './media-upload.component.html',
  styleUrls: ['./media-upload.component.css']
})
export class MediaUploadComponent {
  selectedFile: File | null = null;
  progress = 0;
  uploading = false;
  message = '';

  constructor(private mediaApi: MediaApiService) {}

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    this.selectedFile = input.files && input.files.length ? input.files[0] : null;
  }

  upload() {
    if (!this.selectedFile) return;

  this.uploading = true;
  this.progress = 0;
  this.message = '';

  this.mediaApi.upload(this.selectedFile).subscribe({
    next: event => {
      if (event.type === HttpEventType.UploadProgress && event.total) {
        this.progress = Math.round((100 * event.loaded) / event.total);
      }

      if (event.type === HttpEventType.Response) {
        this.message = 'Upload completed successfully';
        this.uploading = false;
      }
    },
    error: () => {
      this.message = 'Upload failed.';
      this.uploading = false;
    }
  });
}
}
