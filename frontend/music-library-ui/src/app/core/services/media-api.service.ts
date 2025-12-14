import { Injectable } from "@angular/core";
import { HttpClient, HttpEvent, HttpRequest } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment";

export interface MediaItem {
    id: number;
    title: string;
    fileType: string;
    fileSize: number;
    uploadedAt: string;
    status?: string; // Pending / Processing / Processed
}

@Injectable({ providedIn: 'root' })
export class MediaApiService {
    private readonly baseUrl = environment.apiBaseUrl;

    constructor(private http: HttpClient) {}

    upload(file: File): Observable<HttpEvent<any>> {
        const formData = new FormData();
        formData.append('file', file);

        const req = new HttpRequest('POST', `${this.baseUrl}/media/upload`, formData, { reportProgress: true });

        return this.http.request(req);
    }

    getAll(): Observable<MediaItem[]> {
        return this.http.get<MediaItem[]>(`${this.baseUrl}/media`);
    }

    streamUrl(id: number): string {
        return `${this.baseUrl}/media/${id}/stream`;
    }
}