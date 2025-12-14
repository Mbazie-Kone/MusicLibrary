import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MediaApiService, MediaItem } from '../../core/services/media-api.service';

@Component({
  selector: 'app-media-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './media-list.component.html',
  styleUrls: ['./media-list.component.css']
})
export class MediaListComponent implements OnInit {
  media: MediaItem[] = [];
  selected?: MediaItem;

  constructor(private api: MediaApiService) {}

  ngOnInit(): void {
    this.load();
  }

  load() {
    this.api.getAll().subscribe(items => (this.media = items));
  }

  select(item: MediaItem) {
    this.selected = item;
  }

  isVideo(item: MediaItem): boolean {
    return ['mp4', 'webm', 'ogg'].includes(item.fileType?.toLowerCase());
  }

  streamSrc(item: MediaItem): string {
    return this.api.streamUrl(item.id);
  }
}

