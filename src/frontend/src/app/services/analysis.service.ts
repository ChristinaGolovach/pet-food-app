import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AnalysisResult } from '../models/analysis-result.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AnalysisService {
  private readonly apiUrl = `${environment.apiBaseUrl}/api/analysis`;

  constructor(private http: HttpClient) {}

  analyze(photo: File, allergens: string[]): Observable<AnalysisResult> {
    const formData = new FormData();
    
    formData.append('photo', photo);
    allergens.forEach(allergen => {
      formData.append('allergens', allergen);
    })
    
    return this.http.post<AnalysisResult>(this.apiUrl, formData);
  }
}
  