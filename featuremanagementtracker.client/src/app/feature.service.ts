import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Feature } from './FeatureModels/feature.model';

@Injectable({
  providedIn: 'root'
})
export class FeatureService {

  private apiUrl = 'https://localhost:7165/api/feature'

  constructor(private http: HttpClient) { }

  getFeatures(): Observable<Feature[]> {
    return this.http.get<Feature[]>(this.apiUrl).pipe(
      catchError(this.handleError)
    );
  }

  getFeature(id: number): Observable<Feature> {
    return this.http.get<Feature>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  cleanEmptyStrings(obj: any): any {
    return Object.keys(obj).reduce((acc, key) => {
      acc[key] = obj[key] === "" ? null : obj[key];
      return acc;
    }, {} as any);
  }

  createFeature(feature: Feature): Observable<Feature> {

    const payload = this.cleanEmptyStrings(feature);

    return this.http.post<Feature>(this.apiUrl, payload).pipe(
      catchError(this.handleError)
    );
  }

  updateFeature(id: number, feature: Feature): Observable<Feature> {
    const payload = this.cleanEmptyStrings(feature);

    return this.http.put<Feature>(`${this.apiUrl}/${id}`, payload).pipe(
      catchError(this.handleError)
    );
  }

  deleteFeature(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    console.error('An Error Occured', error);
    return throwError(() => new Error('Something went wrong; please try again later.'));
  }
}
