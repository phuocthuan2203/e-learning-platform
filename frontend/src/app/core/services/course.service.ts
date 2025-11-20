import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CourseSummary, CourseDetail, PagedResult } from '../models/course.models';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  private apiUrl = `${environment.apiUrl}/courses`;

  constructor(private http: HttpClient) {}

  /**
   * Get paginated course catalog with optional search
   * @param page Page number (1-based)
   * @param pageSize Number of items per page
   * @param search Optional search term
   * @returns Observable of paginated course summaries
   */
  getCatalog(page: number = 1, pageSize: number = 10, search: string = ''): Observable<PagedResult<CourseSummary>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    if (search && search.trim()) {
      params = params.set('search', search.trim());
    }

    return this.http.get<PagedResult<CourseSummary>>(this.apiUrl, { params });
  }

  /**
   * Get full course details including modules and lessons
   * @param id Course ID
   * @returns Observable of course details
   */
  getDetails(id: number): Observable<CourseDetail> {
    return this.http.get<CourseDetail>(`${this.apiUrl}/${id}`);
  }
}
