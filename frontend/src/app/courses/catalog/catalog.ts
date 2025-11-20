import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';
import { CourseService } from '../../core/services/course.service';
import { CourseSummary, PagedResult } from '../../core/models/course.models';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-catalog',
  standalone: false,
  templateUrl: './catalog.html',
  styleUrl: './catalog.scss',
})
export class CatalogComponent implements OnInit, OnDestroy {
  courses: CourseSummary[] = [];
  isLoading = false;
  searchTerm = '';
  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();

  // Pagination
  totalCount = 0;
  pageSize = 9;
  pageIndex = 0;
  pageSizeOptions = [6, 9, 12, 18];

  constructor(
    private courseService: CourseService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    // Setup search debounce
    this.searchSubject
      .pipe(
        debounceTime(300), // Wait 300ms after user stops typing
        distinctUntilChanged(),
        takeUntil(this.destroy$)
      )
      .subscribe((searchTerm) => {
        this.searchTerm = searchTerm;
        this.pageIndex = 0; // Reset to first page on new search
        this.loadCourses();
      });

    // Load initial courses
    this.loadCourses();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onSearchChange(searchValue: string): void {
    this.searchSubject.next(searchValue);
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadCourses();
  }

  loadCourses(): void {
    this.isLoading = true;
    const page = this.pageIndex + 1; // API uses 1-based pagination

    this.courseService.getCatalog(page, this.pageSize, this.searchTerm).subscribe({
      next: (result: PagedResult<CourseSummary>) => {
        this.courses = result.items;
        this.totalCount = result.totalCount;
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        const msg = error.error?.message || 'Failed to load courses.';
        this.snackBar.open(msg, 'Close', { duration: 5000, panelClass: 'error-snackbar' });
      },
    });
  }

  viewCourseDetails(courseId: number): void {
    this.router.navigate(['/courses', courseId]);
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
  }
}
